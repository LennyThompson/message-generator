using System.Text.Json;
using System.Text.Json.Serialization;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Parser.MessageTypes
{
    public class NumericDefine : Define
    {
        private int _value;

        public NumericDefine(IDefine defineFrom)
        {
            Name = defineFrom.Name;
            Value = defineFrom.Value;
        }

        public void SetNumericValue(int value)
        {
            _value = value;
        }

        public override int NumericValue()
        {
            return _value;
        }

        public override bool IsNumeric()
        {
            return true;
        }
    }
}