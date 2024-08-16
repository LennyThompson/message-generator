using System;

namespace CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IVariableArrayMember : IMember
    {
        IMember? ArraySizeMember { get; }
        IMember ArrayMember { get; }
        bool IsUnknownSize { get; }
    }
}