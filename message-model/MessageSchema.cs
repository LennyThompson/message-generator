using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CougarMessage.Metadata;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;

interface IComponentMessage
{
    string Component { get; }
    IMessage Message { get; }
}

public class MessageSchema : IMessageSchema
{
    private static string DEFAULT_NAME = "default";

    private Dictionary<string, IMessage> m_mapMessages;
    private List<IDefine> m_listDefines;
    private Dictionary<string, IEnum> m_mapEnums;
    private Dictionary<string, IMessage> m_mapUnusedMessages;
    private List<string> m_listProtocolFiles;
    private List<TypeMetaData>? m_listMetaData;

    public MessageSchema()
    {
        m_mapMessages = new Dictionary<string, IMessage>();
        m_listDefines = new List<IDefine>();
        m_mapEnums = new Dictionary<string, IEnum>();
        m_mapUnusedMessages = new Dictionary<string, IMessage>();
        m_listProtocolFiles = new();
    }

    public void AddMessage(IMessage messageAdd)
    {
        m_mapMessages[messageAdd.Name] = messageAdd;
    }

    public void AddDefine(IDefine defineAdd)
    {
        if (!m_listDefines.Any(define => define.Name.Equals(defineAdd.Name)))
        {
            m_listDefines.Add(defineAdd);
        }
    }

    public void AddEnum(IEnum enumAdd)
    {
        m_mapEnums[enumAdd.Name] = enumAdd;
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
                .SelectMany(message => message.Members
                    .Where(member => member.MessageType != null)
                    .Select(member => member.MessageType!.Name))
                .ToList();

            List<IMessage> listUnused = m_mapMessages.Values
                .Where(message => message.IsNonMessage && !listMessageTypes.Contains(message.Name))
                .ToList();

            listUnused.ForEach(message =>
            {
                m_mapUnusedMessages[message.Name] = message;
                m_mapMessages.Remove(message.Name);
            });
        }
        return m_mapUnusedMessages.Count > 0;
    }

    public void ApplyComponentModel(CougarComponentModel componentModel)
    {
        componentModel.Components?
            .ForEach
            (
                component =>
                {
                    component.ConsumerMessages?
                        .ForEach
                        (
                            message =>
                            {
                                IMessage? msgFound = Messages.FirstOrDefault
                                (
                                    msg =>
                                        msg.Define != null && msg.Define.Name != null && msg.Define.Name.Equals(message)
                                );
                                if (msgFound != null)
                                {
                                    msgFound.AddConsumer(component.Name);
                                }
                            }
                        );
                    if (component.GeneratorMessages != null)
                    {
                        component.GeneratorMessages.ForEach(generated =>
                        {
                            if (generated.OutMessages != null && generated.InMessage != null)
                            {
                                IMessage? msgFound = Messages.FirstOrDefault(msg =>
                                    msg.Define?.Name != null && msg.Define.Name.Equals(generated.InMessage));

                                if (msgFound != null)
                                {
                                    if (generated.TraceMembers != null)
                                    {
                                        generated.TraceMembers.ForEach(trace =>
                                            msgFound.AddTraceMember(trace, generated.ExternalKey));
                                    }

                                    if (generated.TimestampFilter != null)
                                    {
                                        msgFound.TimestampFilter = generated.TimestampFilter;
                                    }

                                    msgFound.AddGeneratedMessages
                                    (
                                        Messages.Where
                                        (
                                            msg =>
                                                generated.OutMessages
                                                    .Any
                                                    (
                                                        outMsg =>
                                                            msg.Define?.Name != null && msg.Define.Name.Equals(outMsg)
                                                    )
                                        )
                                        .ToList()
                                    );
                                }
                            }
                            else
                            {
                                if (generated.OutMessages != null && generated.OutMessages.Any())
                                {
                                    // TODO - add messages generated without a source to the message type.
                                }
                            }
                        });
                    }
                });
    }

    List<string> IMessageSchema.ProtocolSource => m_listProtocolFiles;

    public List<IMessage> Messages
    {
        get => m_mapMessages.Values.OrderBy(message => message.Ordinal).ToList();
    }

    public List<IMessage> UnusedMessages
    {
        get => m_mapUnusedMessages.Values.ToList();
    }

    public List<IEnum> Enums
    {
        get => m_mapEnums.Values.ToList();
    }

    public List<IDefine> Defines
    {
        get => m_listDefines;
    }

    public IMessage? FindMessage(string strName)
    {
        return m_mapMessages.GetValueOrDefault(strName);
    }

    public IEnum? FindEnum(string strName)
    {
        return m_mapEnums.GetValueOrDefault(strName);
    }

    public string Name()
    {
        return DEFAULT_NAME;
    }

    public List<TypeMetaData>? MetaData
    {
        get => m_listMetaData;
        set => m_listMetaData = value;
    }

    public void SetMetaData(List<TypeMetaData> listMetaData)
    {
        m_listMetaData = listMetaData;
    }

    public List<string> UniqueComponentList
    {
        get
        {
            return m_mapMessages.Values
                .Where(message => message is { Generators: not null, Consumers: not null })
                .SelectMany(message => message.Generators!.Concat(message.Consumers!))
                .Distinct()
                .ToList();
        }
    }

    public Dictionary<string, List<IMessage>> ConsumerMap
    {
        get
        {
            return Messages
                .Where(message => message.Consumers != null)
                .SelectMany(message =>
                    message.Consumers!.Select(component => new ComponentMessageImpl(component, message)))
                .GroupBy(componentMessage => componentMessage.Component,
                    componentMessage => componentMessage.Message,
                    (key, value) => new { Key = key, Value = value.ToList() })
                .ToDictionary(item => item.Key, item => item.Value);
        }
    }

    public Dictionary<string, List<IMessage>> AllComponentsMap
    {
        get
        {
            Dictionary<string, List<IMessage>> mapAll = new Dictionary<string, List<IMessage>>();

            GeneratorMap.ToList().ForEach(pair =>
            {
                if (mapAll.ContainsKey(pair.Key))
                {
                    mapAll[pair.Key].AddRange(pair.Value);
                }
                else
                {
                    mapAll[pair.Key] = pair.Value;
                }

                mapAll[pair.Key] = mapAll[pair.Key].DistinctBy(message => message.Name).ToList();
            });

            ConsumerMap.ToList().ForEach(pair =>
            {
                if (mapAll.ContainsKey(pair.Key))
                {
                    mapAll[pair.Key].AddRange(pair.Value);
                }
                else
                {
                    mapAll[pair.Key] = pair.Value;
                }

                mapAll[pair.Key] = mapAll[pair.Key].DistinctBy(message => message.Name).ToList();
            });

            return mapAll;
        }
    }

    public Dictionary<string, List<IMessage>> WabfilterMap
    {
        get
        {
            return Messages
                .Where(message => message.WabFilters != null)
                .SelectMany(message =>
                    message.WabFilters!.Select(filter => new ComponentMessageImpl(filter, message)))
                .GroupBy
                (
                    componentMessage => componentMessage.Component,
                    componentMessage => componentMessage.Message,
                    (key, value) => new { Key = key, Value = value.ToList() }
                )
                .ToDictionary(item => item.Key, item => item.Value);
        }
    }

    public Dictionary<string, List<IMessage>> GeneratorMap
    {
        get
        {
            return Messages
                .Where(message => message.Generators != null)
                .SelectMany(message =>
                    message.Generators!.Select(component => new ComponentMessageImpl(component, message)))
                .GroupBy
                (
                    componentMessage => componentMessage.Component,
                    componentMessage => componentMessage.Message,
                    (key, value) => new { Key = key, Value = value.ToList() }
                )
                .ToDictionary(item => item.Key, item => item.Value);
        }
    }

    private class ComponentMessageImpl(string component, IMessage message) : IComponentMessage
    {
        public string Component => component;

        public IMessage Message => message;
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

