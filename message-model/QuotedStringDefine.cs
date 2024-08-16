using System;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes
{
    public class QuotedStringDefine : Define
    {
        public QuotedStringDefine(IDefine defineFrom)
        {
            Name = defineFrom.Name;
            Value = defineFrom.Value;
        }

        public new string Value
        {
            set => base.Value = UnquoteString(value);
        }

        public new bool IsString
        {
            get => true;
        }

        private static string UnquoteString(string input)
        {
            // Implement the unquoting logic here
            // This is a placeholder implementation
            return input.Trim('"');
        }
    }
}