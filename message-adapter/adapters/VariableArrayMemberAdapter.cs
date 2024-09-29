using System;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Adapter;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Adapter
{
    public class VariableArrayMemberAdapter : MemberAdapter
    {
        private MemberAdapter _memberArraySizeAdapter;

        public VariableArrayMemberAdapter(IVariableArrayMember memberAdapt, List<MessageAdapter> listMessages)
            : base(memberAdapt.ArrayMember(), listMessages)
        {
            if (memberAdapt.ArraySizeMember().Name != Message.ERROR_ARRAY_SIZE_MEMBER)
            {
                _memberArraySizeAdapter = MemberAdapterFactory.CreateMemberAdapter(memberAdapt.ArraySizeMember(), listMessages);
            }
        }

        public bool IsVariableLengthArray => true;

        public MemberAdapter SizeMember => _memberArraySizeAdapter;

        public bool HasSizeMember => _memberArraySizeAdapter != null;

        public bool IsUnknownArraySize => _memberArraySizeAdapter == null;

        public string ArraySize
        {
            get
            {
                if (_memberArraySizeAdapter == null)
                {
                    return base.ArraySize();
                }
                return _memberArraySizeAdapter.Name;
            }
        }

        public bool IsArray
        {
            get
            {
                switch (MemberAdapt.Type.ToUpper())
                {
                    case "CHAR":
                        return false;
                    default:
                        return true;
                }
            }
        }

        public bool IsString
        {
            get
            {
                switch (MemberAdapt.Type.ToUpper())
                {
                    case "CHAR":
                        return true;
                    default:
                        return false;
                }
            }
        }

        public override string GetCSharpType()
        {
            switch (MemberAdapt.Type.ToUpper())
            {
                case "CHAR":
                    return "string";
                default:
                    return base.GetCSharpType();
            }
        }

        public string GetCppArrayType()
        {
            return IsString ? "string" : "List<>";
        }

        public bool IsParameterString => IsString;

        public bool IsHashString => IsString;

        public string GetCppType()
        {
            return IsString ? "string" : base.GetCppType();
        }

        public string GetCppFullType()
        {
            string returnType = GetCppType();
            if (!IsString)
            {
                returnType = $"List<{returnType}>";
            }
            return returnType;
        }

        public string GetCppParameterType()
        {
            string returnType = GetCppFullType();

            if (HasMessageType)
            {
                returnType = GetMessageType().GetPointerType();
                returnType = $"List<{returnType}>";
            }
            return $"{returnType}";
        }
    }
}

