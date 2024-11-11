using System;

namespace CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IVariableArrayMember : IArrayMember
    {
        IMember? ArraySizeMember { get; set; }
        bool IsUnknownSize { get; }
    }
}