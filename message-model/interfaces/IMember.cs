using System;
using System.Collections.Generic;

namespace CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IMember
    {
        string Name { get; }
        string ShortName { get; }
        string StrippedName { get; set; }
        string? Prefix { get; }
        string Type { get; }
        IMessage? MessageType { get; }
        IEnum? EnumType { get; }
        bool IsArray { get; }
        List<IAttribute>? Attributes { get; }
        int OriginalByteSize { get; }

        string? ShortFieldDescription { get; }
        string? LongFieldDescription { get; }
    }

    public interface IArrayMember
    {
        bool IsArrayPointer { get; }
        bool IsVariableLengthArray { get; }
        string? ArraySize { get; }
        int NumericArraySize { get; }
    }
}