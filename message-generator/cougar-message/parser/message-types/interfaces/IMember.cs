using System;
using System.Collections.Generic;

namespace Net.CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IMember
    {
        string Name { get; }
        string ShortName { get; }
        string StrippedName { get; set; }
        string Prefix { get; }
        string Type { get; }
        IMessage MessageType { get; }
        IEnum EnumType { get; }
        bool IsArray { get; }
        bool IsArrayPointer { get; }
        bool IsVariableLengthArray { get; }
        string ArraySize { get; }
        int NumericArraySize { get; }
        List<IAttribute> Attributes { get; }
        int OriginalByteSize { get; }

        string ShortFieldDescription();
        string LongFieldDescription();
    }
}