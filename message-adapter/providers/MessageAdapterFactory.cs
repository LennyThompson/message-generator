using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using CougarMessage.Metadata;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Microsoft.Extensions.Logging;

namespace CougarMessage.Adapter
{
    public class MessageAdapterFactory
    {
        //TODO: make this a DI dependency
        static private ILogger _log = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("MessageAdapterFactory");
        public static MessageAdapter CreateMessageAdapter(IMessage messageAdapt)
        {
            if (messageAdapt.IsNonMessage)
            {
                return new NonMessageAdapter(messageAdapt);
            }
            return new MessageAdapter(messageAdapt);
        }

        public static List<MessageAdapter> CreateMessageAdapters(IMessageSchema schemaFrom)
        {
            var listMessages = schemaFrom.Messages.Select(CreateMessageAdapter).ToList();
            listMessages.ForEach(message => message.BuildMemberAdapters(listMessages));
            listMessages.ForEach(message => message.UpdateDependentMessages());
            listMessages.ForEach(messageAdapter =>
            {
                try
                {
                    string strMsgName = messageAdapter.Name;
                    var typeMetaDatas = ((MessageSchema)schemaFrom).MetaData;
                    if (typeMetaDatas != null)
                    {
                        var listTypeData = typeMetaDatas
                            .Where(meta => meta.Name.CompareTo(strMsgName) == 0 || meta.Name.CompareTo("*") == 0)
                            .ToList();
                        if (listTypeData.Any())
                        {
                            var metaData = new TypeMetaData
                            {
                                Name = strMsgName,
                                MetaMembers = listTypeData.SelectMany(meta => meta.MetaMembers).ToList()
                            };
                            messageAdapter.SetMetaData(metaData);
                        }
                    }
                }
                catch (Exception exc)
                {
                    _log.LogError(exc.Message);
                }
            });

            return listMessages;
        }

        public static List<MessageAdapter> CreateSimpleMessageAdapters(List<IMessage> listMessages)
        {
            var listMessageAdapters = listMessages.Select(CreateMessageAdapter).ToList();
            listMessageAdapters.ForEach(message => message.BuildMemberAdapters(listMessageAdapters));
            return listMessageAdapters;
        }
    }
}

