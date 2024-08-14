using System;

namespace CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IVariableArrayMember : IMember
    {
        IMember ArraySizeMember();
        IMember ArrayMember();
        bool IsUnknownSize();
    }
}