using System;
using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes
{
    public class QuotedStringDefine : Define
    {
        public QuotedStringDefine(IDefine defineFrom)
        {
            SetName(defineFrom.Name());
            SetValue(defineFrom.Value());
        }

        public override void SetValue(string strValue)
        {
            base.SetValue(UnquoteString(strValue));
        }

        public override bool IsString()
        {
            return true;
        }

        private static string UnquoteString(string input)
        {
            // Implement the unquoting logic here
            // This is a placeholder implementation
            return input.Trim('"');
        }
    }
}