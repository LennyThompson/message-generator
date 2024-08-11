using System.Collections.Generic;
using Net.CougarMessage.Parser.MessageTypes;
using Net.Interfaces;

namespace Net.CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IMessageSchema : ISchemaBase
    {
        List<IMessage> Messages();
        List<IMessage> UnusedMessages();
        List<IEnum> Enums();
        List<IDefine> Defines();
        IMessage FindMessage(string strName);
        IEnum FindEnum(string strName);

        bool DoTreeShaking();

        void ApplyComponentModel(CougarComponentModel componentModel);

        List<string> ProtocolSource();
    }
}