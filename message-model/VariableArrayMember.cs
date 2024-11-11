using System;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes
{
    public class VariableArrayMember : ArrayMember, IVariableArrayMember
    {
        private IMember? m_memberArraySize;
        public VariableArrayMember(Member memberFrom, IMember? memberArraySize) 
            : base(memberFrom)
        {
            m_memberArraySize = memberArraySize;
        }

        public IMember? ArraySizeMember
        {
            get => m_memberArraySize;
            set => m_memberArraySize = value;
        }

        public bool IsUnknownSize
        {
            get => m_memberArraySize?.Name == Message.ERROR_ARRAY_SIZE_MEMBER;
        }

        public new bool IsVariableLengthArray
        {
            get => true;
        }

        public new int NumericArraySize
        {
            get => 1;
        }

    }
}

