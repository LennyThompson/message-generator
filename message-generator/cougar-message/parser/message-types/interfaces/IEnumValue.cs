using System;

namespace Net.CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IEnumValue
    {
        string Name { get; }
        bool HasValue { get; }
        int Value { get; }
        int Ordinal { get; }
    }
}