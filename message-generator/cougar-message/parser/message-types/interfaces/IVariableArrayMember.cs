using System;

namespace Net.CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IVariableArrayMember : IMember
    {
        IMember ArraySizeMember();
        IMember ArrayMember();
        bool IsUnknownSize();
    }
}