using CougarMessage.Parser.MessageTypes.Interfaces;

namespace adapter_interface;

public interface IAttributeAdapter
{
    string Name { get; }
    List<string> Values { get; }
    IAttribute.AttributeType Type { get; }
}
