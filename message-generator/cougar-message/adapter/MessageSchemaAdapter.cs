using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CougarMessage.Adapter;
using message_generator.cougar_message.adapter;
using CougarMessage.Adapter;
using CougarMessage.Parser.MessageTypes.Interfaces;

public class MessageSchemaAdapter
{
    public const string DART_EXT = ".dart";
    private static string HEADER_EXT = ".h";
    private static string CPP_EXT = ".cpp";
    public static string JAVA_EXT = ".java";
    private static string HEADER_NAME = "cougar-messages";
    private static string MESSAGE_MAP_CPP_NAME = "cougar-message-map";
    private static string CPP_NAME = "cougar-messages";
    private static string UNKNOWN_CPP_NAME = "cougar-message-unknown";
    private static string NONMESSAGE_HEADER_NAME = "nonmessage-hash";
    private static string HELPER_HEADER_NAME = "message-helper";
    private static string HELPER_CPP_NAME = "message-helper";
    private static string ENUM_HELPER_HEADER_NAME = "enum-message-helper";
    private static string ENUM_HELPER_CPP_NAME = "enum-message-helper";
    private static string MESSAGE_BUILDER_HEADER_NAME = "message-builder";
    private static string MESSAGE_BUILDER_CPP_NAME = "message-builder";
    private static string JSON_TEMPLATE_DIRECTORY = "json-template/";
    private static string JSON_TEMPLATE_EXTENSION = ".json_tmpl";
    private static string CMAKE_LIST_FILENAME = "cougarMessages_CMakeList.txt";
    private static string COUGAR_UTILS_NAME = "cougar-utils";

    private static string SCHEMA_NAMESPACE = "cougar_messages";

    private static string GET_MESSAGE_FINDER = "getMessageFinder";
    private static string GET_MESSAGE_FINDER_EX = GET_MESSAGE_FINDER + "Ex";

    private IMessageSchema m_messageSchema;
    private List<MessageAdapter> m_listMessages;
    private int m_nMessageStartIndex = 0;
    private int m_nMessageEndIndex = 0;
    private string m_strCurrentMessageFnName = GET_MESSAGE_FINDER;
    private string m_strNextMessageFnName = GET_MESSAGE_FINDER_EX;
    private int m_nSplitGroupIndex = -1;
    private List<string> m_listMessageFns;
    private string m_strPackageName;

    public MessageSchemaAdapter(IMessageSchema schemaAdapt)
    {
        m_messageSchema = schemaAdapt;
        m_listMessageFns = new List<string>();
        m_listMessageFns.Add(GET_MESSAGE_FINDER);
    }

    public void SetMessageGroup(int nStartIndex, int nEndIndex)
    {
        m_nMessageStartIndex = nStartIndex;
        m_nMessageEndIndex = nEndIndex;
        m_listMessages = Enumerable.Range(m_nMessageStartIndex, m_nMessageEndIndex - m_nMessageStartIndex)
            .Select(index => m_messageSchema.Messages()[index])
            .Select(MessageAdapterFactory.CreateMessageAdapter)
            .ToList();
        m_nSplitGroupIndex++;
        if (m_nSplitGroupIndex > 0)
        {
            m_strCurrentMessageFnName = m_strNextMessageFnName;
            m_listMessageFns.Add(m_strCurrentMessageFnName);
            m_strNextMessageFnName = GET_MESSAGE_FINDER_EX + m_nSplitGroupIndex;
        }
        if (m_nMessageEndIndex == m_messageSchema.Messages().Count)
        {
            m_strNextMessageFnName = "";
        }
    }

    public void SetPackageName(string strPackageName)
    {
        m_strPackageName = strPackageName;
    }
    public string GetPackageName()
    {
        return m_strPackageName;
    }

    public List<MessageAdapter> GetMessages()
    {
        return MessageAdapterFactory.CreateMessageAdapters(m_messageSchema);
    }

    public List<MessageAdapter> GetNonDependentMessages()
    {
        return MessageAdapterFactory.CreateMessageAdapters(m_messageSchema)
                .Where(MessageAdapter.GetHasDefine)
                .Where(messageAdapter => !messageAdapter.GetHasMessageTypeMember())
                .OrderBy(MessageAdapter.GetDefineId)
                .ToList();
    }

    public List<MessageAdapter> GetDependentMessages()
    {
        return MessageAdapterFactory.CreateMessageAdapters(m_messageSchema)
                .Where(MessageAdapter.GetHasDefine)
                .Where(MessageAdapter.GetHasMessageTypeMember)
                .Where(msg => !msg.GetHasDependentMessages())
                .OrderBy(MessageAdapter.GetDefineId)
                .ToList();
    }

    public List<MessageAdapter> GetMessagesDependencies()
    {
        return MessageAdapterFactory.CreateMessageAdapters(m_messageSchema)
                .Where(MessageAdapter.GetHasDefine)
                .Where(MessageAdapter.GetHasMessageTypeMember)
                .Where(MessageAdapter.GetHasDependentMessages)
                .OrderBy(new Comparison<MessageAdapter>((msg1, msg2) =>
                {
                    if (msg1.GetDependsOnMessageType(msg2))
                    {
                        return 1;
                    }
                    else if (msg2.GetDependsOnMessageType(msg1))
                    {
                        return -1;
                    }
                    return 0;
                }))
                .ToList();
    }

    public List<MessageAdapter> GetOrderedMessages()
    {
        return MessageAdapterFactory.CreateMessageAdapters(m_messageSchema)
                .Where(MessageAdapter.GetHasDefine)
                .OrderBy(new Comparison<MessageAdapter>((msg1, msg2) =>
                {
                    if (msg1.GetDependsOnMessageType(msg2))
                    {
                        return 1;
                    }
                    else if (msg2.GetDependsOnMessageType(msg1))
                    {
                        return -1;
                    }
                    return 0;
                }))
                .ToList();
    }

    public int GetMessagesCount()
    {
        return m_messageSchema.Messages().Count;
    }

    public List<MessageAdapter> GetMessagesSorted()
    {
        return MessageAdapterFactory.CreateMessageAdapters(m_messageSchema)
            .OrderBy(MessageAdapter.GetName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public List<MessageAdapter> GetMessagesSortedByDefine()
    {
        return MessageAdapterFactory.CreateMessageAdapters(m_messageSchema)
                .OrderBy(MessageAdapter.GetDefineId)
                .ToList();
    }

    public List<MessageAdapter> GetSplitMessages()
    {
        if (m_listMessages != null)
        {
            return m_listMessages;
        }
        return GetMessages();
    }

    public List<string> GetAllProtocolFiles()
    {
        return m_messageSchema.ProtocolSource();
    }

    public string GetMessageHelperName()
    {
        return m_strCurrentMessageFnName;
    }

    public string GetNextMessageHelperName()
    {
        return m_strNextMessageFnName;
    }

    public bool GetHasNextMessageHelperName()
    {
        return !string.IsNullOrEmpty(m_strNextMessageFnName);
    }

    public List<string> GetMessageHelperFnNames()
    {
        return m_listMessageFns;
    }

    public List<MessageAdapter> GetNonMessages()
    {
        return MessageAdapterFactory.CreateSimpleMessageAdapters(
                m_messageSchema.Messages()
                    .Where(IMessage.IsNonMessage)
                    .ToList()
            );
    }

    public List<MessageAdapter> GetUnusedMessages()
    {
        return MessageAdapterFactory.CreateSimpleMessageAdapters(m_messageSchema.UnusedMessages());
    }

    public List<MessageAdapter> GetDefineMessages()
    {
        return GetMessagesSorted()
            .Where(MessageAdapter.GetHasDefine)
            .ToList();
    }

    public List<MessageAdapter> GetDefineMessagesById()
    {
        return GetMessagesSortedByDefine().Where(MessageAdapter.GetHasDefine).ToList();
    }

    public List<DefineAdapter> GetDefines()
    {
        return DefineAdapterFactory.CreateDefineAdapters(m_messageSchema);
    }
    public List<DefineAdapter> GetJavaDefines()
    {
        return GetDefines().Where(DefineAdapter.GetIsJavaDefine).ToList();
    }

    public List<EnumAdapter> GetEnums()
    {
        return m_messageSchema.Enums().Select(enumDef => new EnumAdapter(enumDef)).ToList();
    }

    public interface IComponentMessageAdapter
    {
        string GetComponentName();
        string GetReferenceName();
        List<MessageAdapter> GetMessages();
        List<MessageAdapter> GetGeneratedMessages();
        List<MessageAdapter> GetUniqueGeneratedMessages();
    }

    public List<IComponentMessageAdapter> GetConsumers()
    {
        return ((MessageSchema)m_messageSchema).GetConsumerMap().Where(entry => !string.IsNullOrEmpty(entry.Key))
                .Select(entry => BuildMessageAdapter(entry))
                .ToList();
    }

    private IComponentMessageAdapter BuildMessageAdapter(KeyValuePair<string, List<IMessage>> entry)
    {
        return new IComponentMessageAdapter()
        {
            GetComponentName = () => entry.Key,
            GetReferenceName = () => entry.Key.Equals("WILDCAT") ? "_WILDCAT" : entry.Key,
            GetMessages = () => entry.Value.Select(MessageAdapterFactory.CreateMessageAdapter).ToList(),
            GetGeneratedMessages = () => entry.Value.SelectMany(message => message.GetGeneratedMessages())
                                            .DistinctBy(msg => msg.Name)
                                            .Select(MessageAdapterFactory.CreateMessageAdapter)
                                            .ToList(),
            GetUniqueGeneratedMessages = () =>
            {
                var setMessageNames = entry.Value.Select(IMessage.Name).ToHashSet();
                return GetGeneratedMessages().Where(message => !setMessageNames.Contains(message.GetName())).ToList();
            }
        };
    }

    public List<IComponentMessageAdapter> GetGenerators()
    {
        return ((MessageSchema)m_messageSchema).GetGeneratorMap().Select(entry => BuildMessageAdapter(entry)).ToList();
    }

    public List<IComponentMessageAdapter> GetWabFilterComponents()
    {
        return ((MessageSchema)m_messageSchema).GetWabfilterMap().Select(entry => BuildMessageAdapter(entry)).ToList();
    }

    public IComponentMessageAdapter GetWabHOSTFilterComponent()
    {
        return ((MessageSchema)m_messageSchema).GetWabfilterMap()
                .Where(entry => entry.Key.Equals(IWabFilterAttribute.Target.HOST.ToString()))
                .Select(entry => BuildMessageAdapter(entry))
                .FirstOrDefault();
    }

    public IComponentMessageAdapter GetWabSITEFilterComponent()
    {
        return ((MessageSchema)m_messageSchema).GetWabfilterMap()
                .Where(entry => entry.Key.Equals(IWabFilterAttribute.Target.SITE.ToString()))
                .Select(entry => BuildMessageAdapter(entry))
                .FirstOrDefault();
    }

    public List<string> GetAllComponents()
    {
        var listComponents = ((MessageSchema)m_messageSchema).GetGeneratorMap().Keys.Where(key => !string.IsNullOrEmpty(key)).ToList();
        listComponents.AddRange(((MessageSchema)m_messageSchema).GetConsumerMap().Keys.Where(key => !string.IsNullOrEmpty(key)));
        return listComponents.Distinct().ToList();
    }

    public List<IComponentMessageAdapter> GetAllComponentAdapters()
    {
        var listComponents = GetGenerators();
        listComponents.AddRange(GetConsumers());
        return listComponents.DistinctBy(component => component.GetComponentName()).ToList();
    }

    public List<string> GetWabFilters()
    {
        return ((MessageSchema)m_messageSchema).GetWabfilterMap().Keys.ToList();
    }

    public string GetGenerateDate()
    {
        return DateTime.Now.ToString();
    }

    public string GetHeaderName()
    {
        return HEADER_NAME + HEADER_EXT;
    }

    public string GetCppName()
    {
        return CPP_NAME + CPP_EXT;
    }

    public string GetUnknownCppName()
    {
        return UNKNOWN_CPP_NAME + CPP_EXT;
    }

    public string GetMessageMapCppName()
    {
        return MESSAGE_MAP_CPP_NAME + CPP_EXT;
    }

    public string GetBaseCppName()
    {
        return CPP_NAME;
    }

    public string GetCppExtension()
    {
        return CPP_EXT;
    }

    public string GetHelperHeaderName() { return HELPER_HEADER_NAME + HEADER_EXT; }

    public string GetEnumHelperHeaderName() { return ENUM_HELPER_HEADER_NAME + HEADER_EXT; }

    public string GetBuilderHeaderName() { return MESSAGE_BUILDER_HEADER_NAME + HEADER_EXT; }

    public string GetNonmessageHeaderName() { return NONMESSAGE_HEADER_NAME + HEADER_EXT; }

    public string GetHelperCppName(int nFileIndex)
    {
        return nFileIndex > 0 ? HELPER_CPP_NAME + nFileIndex + CPP_EXT : HELPER_CPP_NAME + CPP_EXT;
    }

    public string GetEnumHelperCppName() { return ENUM_HELPER_CPP_NAME + CPP_EXT; }

    public string GetBuilderCppName() { return MESSAGE_BUILDER_CPP_NAME + CPP_EXT; }

    public string GetJsonTemplateDirectory() { return JSON_TEMPLATE_DIRECTORY; }

    public string GetJsonTemplateExtension() { return JSON_TEMPLATE_EXTENSION; }

    public string GetSchemaNamespace() { return SCHEMA_NAMESPACE; }

    public string GetCMakeListFileName()
    {
        return CMAKE_LIST_FILENAME;
    }

    public string GetUtilsImplName()
    {
        return COUGAR_UTILS_NAME + CPP_EXT;
    }

    public string GetJsonName()
    {
        return HEADER_NAME + ".json";
    }

    public string GetCsvName()
    {
        return HEADER_NAME + ".csv";
    }
}

