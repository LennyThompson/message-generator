using System.Collections.Generic;
using CougarMessage.Metadata;
using CougarMessage.Parser.MessageTypes;

namespace CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IMessageSchema
    {
        List<IMessage> Messages { get; }
        List<IMessage> UnusedMessages { get; }
        List<IEnum> Enums { get; }
        List<IDefine> Defines { get; }
        IMessage? FindMessage(string strName);
        IEnum? FindEnum(string strName);

        bool DoTreeShaking();

        void ApplyComponentModel(CougarComponentModel componentModel);

        List<string> ProtocolSource { get; }
        string Name();

        List<TypeMetaData>? MetaData
        {
            get;
            set;
        }
        List<string> UniqueComponentList { get; }
        Dictionary<string, List<IMessage>> ConsumerMap { get; }
        Dictionary<string, List<IMessage>> AllComponentsMap { get; }
        Dictionary<string, List<IMessage>> WabfilterMap { get; }
        Dictionary<string, List<IMessage>> GeneratorMap { get; }

    }
}