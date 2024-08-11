using System;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Parser.MessageTypes
{
    public class VariableArrayMember : IVariableArrayMember
    {
        private IMember _memberArray;
        private IMember _memberArraySize;

        public VariableArrayMember(IMember memberArray, IMember memberArraySize)
        {
            _memberArray = memberArray;
            _memberArraySize = memberArraySize;
        }

        public IMember ArraySizeMember()
        {
            return _memberArraySize;
        }

        public IMember ArrayMember()
        {
            return _memberArray;
        }

        public bool IsUnknownSize()
        {
            return _memberArraySize.Name() == Message.ERROR_ARRAY_SIZE_MEMBER;
        }

        public string Name()
        {
            return _memberArray.Name();
        }

        public string ShortName()
        {
            return _memberArray.ShortName();
        }

        public string StrippedName()
        {
            return _memberArray.StrippedName();
        }

        public string Prefix()
        {
            return _memberArray.Prefix();
        }

        public string Type()
        {
            return _memberArray.Type();
        }

        public IMessage MessageType()
        {
            return _memberArray.MessageType();
        }

        public IEnum EnumType()
        {
            return _memberArray.EnumType();
        }

        public bool IsArray()
        {
            return true;
        }

        public bool IsArrayPointer()
        {
            return _memberArray.IsArrayPointer();
        }

        public bool IsVariableLengthArray()
        {
            return true;
        }

        public string ArraySize()
        {
            return _memberArray.ArraySize();
        }

        public int NumericArraySize()
        {
            return 1;
        }

        public List<IAttribute> Attributes()
        {
            return _memberArray.Attributes();
        }

        public int OriginalByteSize()
        {
            // We can't possibly know this...
            return _memberArray.OriginalByteSize();
        }

        public string ShortFieldDescription()
        {
            return _memberArray.ShortFieldDescription();
        }

        public string LongFieldDescription()
        {
            return _memberArray.LongFieldDescription();
        }

        public void SetStrippedName(string strStrippedName)
        {
            _memberArray.SetStrippedName(strStrippedName);
        }
    }
}

