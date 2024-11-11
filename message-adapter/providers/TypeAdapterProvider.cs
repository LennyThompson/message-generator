using adapter_interface;
using adapter_interface.providers;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace message_adapter.providers;

public class TypeAdapterProvider
    : ITypeAdapterProvider
{
    public Dictionary<string, IMessage> CustomTypes { get; set; }
    public ITypeAdapter GetTypeAdapter(string strType)
    {
        throw new NotImplementedException();
    }
}