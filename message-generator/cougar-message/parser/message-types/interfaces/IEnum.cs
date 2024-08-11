using System.Collections.Generic;

namespace Net.CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IEnum
    {
        string Name { get; }
        List<IEnumValue> Values { get; }
    }
}