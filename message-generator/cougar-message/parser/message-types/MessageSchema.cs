using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public interface IMessageSchema
{
    bool DoTreeShaking();
    void ApplyComponentModel(CougarComponentModel componentModel);
    List<string> ProtocolSource();
    List<IMessage> Messages();
    List<IMessage> UnusedMessages();
    List<IEnum> Enums();
    List<IDefine> Defines();
    IMessage FindMessage(string strName);
    IEnum FindEnum(string strName);
    string Name();
    List<TypeMetaData> GetMetaData();
    void SetMetaData(List<TypeMetaData> listMetaData);
    List<string> GetUniqueComponentList();
    Map<string, List<IMessage>> GetConsumerMap();
    Map<string, List<IMessage>> GetAllComponentsMap();
    Map<string, List<IMessage>> GetWabfilterMap();
    Map<string, List<IMessage>> GetGeneratorMap();
}

public class MessageSchema : IMessageSchema
{
    private static string DEFAULT_NAME = "default";

    private Dictionary<string, IMessage> m_mapMessages;
    private List<IDefine> m_listDefines;
    private Dictionary<string, IEnum> m_mapEnums;
    private Dictionary<string, IMessage> m_mapUnusedMessages;
    private List<string> m_listProtocolFiles;
    private List<TypeMetaData> m_listMetaData;

    public MessageSchema()
    {
        m_mapMessages = new Dictionary<string, IMessage>();
        m_listDefines = new List<IDefine>();
        m_mapEnums = new Dictionary<string, IEnum>();
        m_mapUnusedMessages = new Dictionary<string, IMessage>();
    }

    public void AddMessage(IMessage messageAdd)
    {
        m_mapMessages[messageAdd.Name()] = messageAdd;
    }

    public void AddDefine(IDefine defineAdd)
    {
        if (!m_listDefines.Any(define => define.Name().Equals(defineAdd.Name())))
        {
            m_listDefines.Add(defineAdd);
        }
    }

    public void AddEnum(IEnum enumAdd)
    {
        m_mapEnums[enumAdd.Name()] = enumAdd;
    }

    public void SetProtocolSource(List<string> listProtocolFiles)
    {
        m_listProtocolFiles = listProtocolFiles.Select(filename => Path.GetFileName(filename)).ToList();
    }

    public bool DoTreeShaking()
    {
        if (m_mapUnusedMessages.Count == 0)
        {
            List<string> listMessageTypes = m_mapMessages.Values
                .SelectMany(message => message.Members()
                    .Where(member => member.MessageType() != null)
                    .Select(member => member.MessageType().Name()))
                .ToList();

            List<IMessage> listUnused = m_mapMessages.Values
                .Where(message => message.IsNonMessage() && !listMessageTypes.Contains(message.Name()))
                .ToList();

            listUnused.ForEach(message =>
            {
                m_mapUnusedMessages[message.Name()] = message;
                m_mapMessages.Remove(message.Name());
            });
        }
        return m_mapUnusedMessages.Count > 0;
    }

    public void ApplyComponentModel(CougarComponentModel componentModel)
    {
        componentModel.GetComponents().ForEach(component =>
        {
            component.GetConsumed().ForEach(message =>
            {
                IMessage msgFound = Messages().FirstOrDefault(msg =>
                    msg.Define() != null && msg.Define().Name() != null && msg.Define().Name().Equals(message));
                if (msgFound != null)
                {
                    msgFound.AddConsumer(component.Name());
                }
            });
            if (component.GetGenerated() != null)
            {
                component.GetGenerated().ForEach(generated =>
                {
                    if (!generated.GetGeneratedMessages().Any())
                    {
                        if (generated.GetInMessage() != null)
                        {
                            IMessage msgFound = Messages().FirstOrDefault(msg =>
                                msg.Define() != null && msg.Define().Name() != null && msg.Define().Name().Equals(generated.GetInMessage()));

                            if (!generated.GetTraceMembers().Any())
                            {
                                generated.GetTraceMembers().ForEach(trace => msgFound.AddTraceMember(trace, generated.GetExternalKey()));
                            }
                            if (generated.GetTimestampFilter() != null)
                            {
                                msgFound.SetTimestampFilter(generated.GetTimestampFilter());
                            }
                            if (msgFound == null)
                            {
                                int n = 100;
                            }
                            msgFound.AddGeneratedMessages(Messages().Where(msg =>
                                generated.GetGeneratedMessages().Any(outMsg =>
                                    msg.Define() != null && msg.Define().Name() != null && msg.Define().Name().Equals(outMsg))).ToList());
                        }
                    }
                    else
                    {
                        if (generated.GetGeneratedMessages() != null && generated.GetGeneratedMessages().Any())
                        {
                            // TODO - add messages generated without a source to the message type.
                        }
                    }
                });
            }
        });
    }

    public List<string> ProtocolSource()
    {
        return m_listProtocolFiles;
    }

    public List<IMessage> Messages()
    {
        return m_mapMessages.Values.OrderBy(message => message.Ordinal()).ToList();
    }

    public List<IMessage> UnusedMessages()
    {
        return m_mapUnusedMessages.Values.ToList();
    }

    public List<IEnum> Enums()
    {
        return m_mapEnums.Values.ToList();
    }

    public List<IDefine> Defines()
    {
        return m_listDefines;
    }

    public IMessage FindMessage(string strName)
    {
        return m_mapMessages.GetValueOrDefault(strName);
    }

    public IEnum FindEnum(string strName)
    {
        return m_mapEnums.GetValueOrDefault(strName);
    }

    public string Name()
    {
        return DEFAULT_NAME;
    }

    public List<TypeMetaData> GetMetaData()
    {
        return m_listMetaData;
    }

    public void SetMetaData(List<TypeMetaData> listMetaData)
    {
        m_listMetaData = listMetaData;
    }

    public List<string> GetUniqueComponentList()
    {
        return m_mapMessages.Values
            .SelectMany(message => message.Generators().Concat(message.Consumers()))
            .Distinct()
            .ToList();
    }

    interface IComponentMessage
    {
        string Component();
        IMessage Message();
    }

    public Dictionary<string, List<IMessage>> GetConsumerMap()
    {
        return Messages().SelectMany(message => message.Consumers().Select(component => new IComponentMessageImpl(component, message)))
            .GroupBy(componentMessage => componentMessage.Component(), componentMessage => componentMessage.Message(), (key, value) => new { Key = key, Value = value.ToList() })
            .ToDictionary(item => item.Key, item => item.Value);
    }

    public Dictionary<string, List<IMessage>> GetAllComponentsMap()
    {
        Dictionary<string, List<IMessage>> mapAll = new Dictionary<string, List<IMessage>>();

        GetGeneratorMap().ToList().ForEach(pair =>
        {
            mapAll.Merge(pair.Key, pair.Value, (list1, list2) =>
            {
                List<IMessage> listDistinctMsgs = new List<IMessage>(list1);
                listDistinctMsgs.AddRange(list2);
                return listDistinctMsgs.DistinctBy(message => message.Name()).ToList();
            });
        });

        GetConsumerMap().ToList().ForEach(pair =>
        {
            mapAll.Merge(pair.Key, pair.Value, (list1, list2) =>
            {
                List<IMessage> listDistinctMsgs = new List<IMessage>(list1);
                listDistinctMsgs.AddRange(list2);
                return listDistinctMsgs.DistinctBy(message => message.Name()).ToList();
            });
        });

        return mapAll;
    }

    public Dictionary<string, List<IMessage>> GetWabfilterMap()
    {
        return Messages().SelectMany(message => message.WabFilters().Select(filter => new IComponentMessageImpl(filter, message)))
            .GroupBy(componentMessage => componentMessage.Component(), componentMessage => componentMessage.Message(), (key, value) => new { Key = key, Value = value.ToList() })
            .ToDictionary(item => item.Key, item => item.Value);
    }

    public Dictionary<string, List<IMessage>> GetGeneratorMap()
    {
        return Messages().SelectMany(message => message.Generators().Select(component => new IComponentMessageImpl(component, message)))
            .GroupBy(componentMessage => componentMessage.Component(), componentMessage => componentMessage.Message(), (key, value) => new { Key = key, Value = value.ToList() })
            .ToDictionary(item => item.Key, item => item.Value);
    }

    private class IComponentMessageImpl : IComponentMessage
    {
        private readonly string component;
        private readonly IMessage message;

        public IComponentMessageImpl(string component, IMessage message)
        {
            this.component = component;
            this.message = message;
        }

        public string Component()
        {
            return component;
        }

        public IMessage Message()
        {
            return message;
        }
    }
}

public static class DictionaryExtensions
{
    public static void Merge<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, List<TValue> value, Func<List<TValue>, List<TValue>, List<TValue>> mergeFunc)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = mergeFunc(dictionary[key], value);
        }
        else
        {
            dictionary[key] = value;
        }
    }
}

public static class EnumerableExtensions
{
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }
}

