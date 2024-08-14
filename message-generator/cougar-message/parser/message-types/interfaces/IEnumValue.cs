using System;

namespace CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IEnumValue
    {
        string Name { get; }
        bool HasValue { get; }
        int Value { get; }
        int Ordinal { get; }
    }
}