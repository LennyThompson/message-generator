using CougarMessage.Parser.MessageTypes.Interfaces;

namespace adapter_interface.providers;

public interface ITypeAdapterProvider
{
    Dictionary<string, IMessage> CustomTypes { get; set; }
    ITypeAdapter GetTypeAdapter(string strType);
}