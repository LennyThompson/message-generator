using System;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes
{
    public class VariableArrayMember(IMember memberArray, IMember? memberArraySize) : IVariableArrayMember
    {
        public IMember? ArraySizeMember
        {
            get => memberArraySize;
        }

        public IMember ArrayMember
        {
            get => memberArray;
        }

        public bool IsUnknownSize
        {
            get => memberArraySize?.Name == Message.ERROR_ARRAY_SIZE_MEMBER;
        }

        public string Name
        {
            get => memberArray.Name;
        }

        public string ShortName
        {
            get => memberArray.ShortName;
        }

        public string StrippedName
        {
            get => memberArray.StrippedName;
            set => memberArray.StrippedName = value;
        }

        public string? Prefix
        {
            get => memberArray.Prefix;
        }

        public string Type
        {
            get => memberArray.Type;
        }

        public IMessage? MessageType
        {
            get => memberArray.MessageType;
        }

        public IEnum? EnumType
        {
            get => memberArray.EnumType;
        }

        public bool IsArray
        {
            get => true;
        }

        public bool IsArrayPointer
        {
            get => memberArray.IsArrayPointer;
        }

        public bool IsVariableLengthArray
        {
            get => true;
        }

        public string ArraySize
        {
            get => memberArray.ArraySize!;
        }

        public int NumericArraySize
        {
            get => 1;
        }

        public List<IAttribute>? Attributes
        {
            get => memberArray.Attributes;
        }

        public int OriginalByteSize
        {
            // We can't possibly know this...
            get => memberArray.OriginalByteSize;
        }

        public string? ShortFieldDescription
        {
            get => memberArray.ShortFieldDescription;
        }

        public string? LongFieldDescription
        {
            get => memberArray.LongFieldDescription;
        }
    }
}

