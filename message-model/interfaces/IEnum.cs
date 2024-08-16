using System.Collections.Generic;

namespace CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IEnum
    {
        string Name { get; }
        List<IEnumValue> Values { get; }
    }
}