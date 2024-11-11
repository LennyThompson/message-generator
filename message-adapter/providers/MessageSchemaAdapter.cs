using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CougarMessage.Adapter;
using CougarMessage.Adapter;
using CougarMessage.Parser.MessageTypes.Interfaces;

public interface IComponentMessageAdapter
{
    string ComponentName { get; }
    string ReferenceName { get; }
    List<MessageAdapter> Messages { get; }
    List<MessageAdapter> GeneratedMessages { get; }
    List<MessageAdapter> UniqueGeneratedMessages { get; }
}

class ComponentMessageAdapter : IComponentMessageAdapter
{
    public ComponentMessageAdapter(KeyValuePair<string, List<IMessage>> messages)
    {
        _messages = messages;
    }

    public string ComponentName => _messages.Key;

    public string ReferenceName => _messages.Key.Equals("WILDCAT") ? "_WILDCAT" : _messages.Key;

    public List<MessageAdapter> Messages => _messages.Value.Select(MessageAdapterFactory.CreateMessageAdapter).ToList();

    public List<MessageAdapter> GeneratedMessages => _messages.Value.SelectMany(message => message.GeneratedMessages)
        .DistinctBy(msg => msg.Name)
        .Select(MessageAdapterFactory.CreateMessageAdapter)
        .ToList();

    public List<MessageAdapter> UniqueGeneratedMessages
    {
        get
        {
            var setMessageNames = _messages.Value.Select(msg => msg.Name).ToHashSet();
            return GeneratedMessages.Where(message => !setMessageNames.Contains(message.Name)).ToList();
        }
    }

    private KeyValuePair<string, List<IMessage>> _messages;
}

class MessageDependsComparer : IComparer<MessageAdapter>
{
    public int Compare(MessageAdapter? msg1, MessageAdapter? msg2)
    {
        if (msg1 == null && msg2 == null)
        {
            return 0;
        }
        else if (msg1 == null && msg2 != null)
        {
            return -1;
        }
        else if (msg1 != null && msg2 == null)
        {
            return 1;
        }
        else
        {
            if (msg1!.GetDependsOnMessageType(msg2!))
            {
                return 1;
            }
            else if (msg2!.GetDependsOnMessageType(msg1!))
            {
                return -1;
            }
        }
        return 0;
    }
}

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
            .Select(index => m_messageSchema.Messages[index])
            .Select(message => MessageAdapterFactory.CreateMessageAdapter(message))
            .ToList();
        m_nSplitGroupIndex++;
        if (m_nSplitGroupIndex > 0)
        {
            m_strCurrentMessageFnName = m_strNextMessageFnName;
            m_listMessageFns.Add(m_strCurrentMessageFnName);
            m_strNextMessageFnName = GET_MESSAGE_FINDER_EX + m_nSplitGroupIndex;
        }
        if (m_nMessageEndIndex == m_messageSchema.Messages.Count)
        {
            m_strNextMessageFnName = "";
        }
    }

    public string PackageName
    {
        get;
        set;
    }
    public List<MessageAdapter> Messages => MessageAdapterFactory.CreateMessageAdapters(m_messageSchema);

    public List<MessageAdapter> NonDependentMessages => MessageAdapterFactory.CreateMessageAdapters(m_messageSchema)
                .Where(msgAdapt => msgAdapt.HasDefine)
                .Where(msgAdapt => !msgAdapt.HasMessageTypeMember)
                .OrderBy(msgAdapt => msgAdapt.DefineId)
                .ToList();

    public List<MessageAdapter> DependentMessages => MessageAdapterFactory.CreateMessageAdapters(m_messageSchema)
                .Where(msgAdapt => msgAdapt.HasDefine)
                .Where(msgAdapt => msgAdapt.HasMessageTypeMember)
                .Where(msgAdapt => !msgAdapt.HasDependentMessages)
                .OrderBy(msgAdapt => msgAdapt.DefineId)
                .ToList();

    public List<MessageAdapter> MessagesDependencies
        => MessageAdapterFactory.CreateMessageAdapters(m_messageSchema)
                .Where(msgAdapt => msgAdapt.HasDefine)
                .Where(msgAdapt => msgAdapt.HasMessageTypeMember)
                .Where(msgAdapt => msgAdapt.HasDependentMessages)
                .OrderBy(msg => msg, new MessageDependsComparer())
                .ToList();

    public List<MessageAdapter> OrderedMessages
        => MessageAdapterFactory.CreateMessageAdapters(m_messageSchema)
                .Where(msgAdapt => msgAdapt.HasDefine)
                .OrderBy(msg => msg, new MessageDependsComparer())
                .ToList();

    public int MessagesCount => m_messageSchema.Messages.Count;

    public List<MessageAdapter> MessagesSorted => MessageAdapterFactory.CreateMessageAdapters(m_messageSchema)
            .OrderBy(msgAdapt => msgAdapt.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

    public List<MessageAdapter> MessagesSortedByDefine => MessageAdapterFactory.CreateMessageAdapters(m_messageSchema)
                .OrderBy(msgAdapt => msgAdapt.DefineId)
                .ToList();

    public List<MessageAdapter> SplitMessages
    {
        get
        {
            if (m_listMessages != null)
            {
                return m_listMessages;
            }

            return Messages;
        }
    }

    public List<string> AllProtocolFiles => m_messageSchema.ProtocolSource;

    public string MessageHelperName => m_strCurrentMessageFnName;

    public string NextMessageHelperName => m_strNextMessageFnName;

    public bool HasNextMessageHelperName => !string.IsNullOrEmpty(m_strNextMessageFnName);

    public List<string> MessageHelperFnNames => m_listMessageFns;

    public List<MessageAdapter> NonMessages => MessageAdapterFactory.CreateSimpleMessageAdapters(
                m_messageSchema.Messages
                    .Where(msg => msg.IsNonMessage)
                    .ToList()
            );

    public List<MessageAdapter> UnusedMessages => MessageAdapterFactory.CreateSimpleMessageAdapters(m_messageSchema.UnusedMessages);

    public List<MessageAdapter> DefineMessages => MessagesSorted
            .Where(msgAdapt => msgAdapt.HasDefine)
            .ToList();

    public List<MessageAdapter> DefineMessagesById =>  MessagesSortedByDefine.Where(msgAdapt => msgAdapt.HasDefine).ToList();

    public List<DefineAdapter> Defines => DefineAdapterFactory.CreateDefineAdapters(m_messageSchema);
    public List<DefineAdapter> JavaDefines => Defines.Where(defAdapt => defAdapt.IsJavaDefine).ToList();

    public List<EnumAdapter> Enums => m_messageSchema.Enums.Select(enumDef => new EnumAdapter(enumDef)).ToList();

    public List<IComponentMessageAdapter> Consumers => ((MessageSchema)m_messageSchema).ConsumerMap.Where(entry => !string.IsNullOrEmpty(entry.Key))
                .Select(entry => BuildMessageAdapter(entry))
                .ToList();

    private IComponentMessageAdapter BuildMessageAdapter(KeyValuePair<string, List<IMessage>> entry)
    {
        return new ComponentMessageAdapter(entry);
    }

    public List<IComponentMessageAdapter> Generators => ((MessageSchema)m_messageSchema).GeneratorMap.Select(entry => BuildMessageAdapter(entry)).ToList();

    public List<IComponentMessageAdapter> WabFilterComponents => ((MessageSchema)m_messageSchema).WabfilterMap.Select(entry => BuildMessageAdapter(entry)).ToList();

    public IComponentMessageAdapter? WabHOSTFilterComponent => ((MessageSchema)m_messageSchema).WabfilterMap
                .Where(entry => entry.Key.Equals(IWabFilterAttribute.Target.Host.ToString()))
                .Select(entry => BuildMessageAdapter(entry))
                .FirstOrDefault();

    public IComponentMessageAdapter? WabSITEFilterComponent => ((MessageSchema)m_messageSchema).WabfilterMap
                .Where(entry => entry.Key.Equals(IWabFilterAttribute.Target.Site.ToString()))
                .Select(entry => BuildMessageAdapter(entry))
                .FirstOrDefault();

    public List<string> AllComponents
    {
        get
        {
            var listComponents = ((MessageSchema)m_messageSchema).GeneratorMap.Keys
                .Where(key => !string.IsNullOrEmpty(key)).ToList();
            listComponents.AddRange(((MessageSchema)m_messageSchema).ConsumerMap.Keys
                .Where(key => !string.IsNullOrEmpty(key)));
            return listComponents.Distinct().ToList();
        }
    }

    public List<IComponentMessageAdapter> AllComponentAdapters
    {
        get
        {
            var listComponents = Generators;
            listComponents.AddRange(Consumers);
            return listComponents.DistinctBy(component => component.ComponentName).ToList();
        }
    }

    public List<string> GetWabFilters => ((MessageSchema)m_messageSchema).WabfilterMap.Keys.ToList();

    public string GenerateDate => DateTime.Now.ToString();

    public string HeaderName => HEADER_NAME + HEADER_EXT;
    public string CppName => CPP_NAME + CPP_EXT;
    public string UnknownCppName => UNKNOWN_CPP_NAME;
    public string MessageMapCppName => MESSAGE_MAP_CPP_NAME + CPP_EXT;
    public string BaseCppName =>  CPP_NAME;

    public string CppExtension => CPP_EXT;

    public string HelperHeaderName => HELPER_HEADER_NAME + HEADER_EXT;

    public string EnumHelperHeaderName() { return ENUM_HELPER_HEADER_NAME + HEADER_EXT; }

    public string BuilderHeaderName() { return MESSAGE_BUILDER_HEADER_NAME + HEADER_EXT; }

    public string NonmessageHeaderName() { return NONMESSAGE_HEADER_NAME + HEADER_EXT; }

    public string HelperCppName(int nFileIndex)
    {
        return nFileIndex > 0 ? HELPER_CPP_NAME + nFileIndex + CPP_EXT : HELPER_CPP_NAME + CPP_EXT;
    }

    public string EnumHelperCppName => ENUM_HELPER_CPP_NAME + CPP_EXT;

    public string BuilderCppName() => MESSAGE_BUILDER_CPP_NAME + CPP_EXT;

    public string JsonTemplateDirectory() => JSON_TEMPLATE_DIRECTORY;

    public string JsonTemplateExtension() => JSON_TEMPLATE_EXTENSION;

    public string SchemaNamespace() => SCHEMA_NAMESPACE;

    public string CMakeListFileName => CMAKE_LIST_FILENAME;

    public string UtilsImplName => COUGAR_UTILS_NAME + CPP_EXT;

    public string JsonName => HEADER_NAME + ".json";

    public string CsvName => HEADER_NAME + ".csv";
}

