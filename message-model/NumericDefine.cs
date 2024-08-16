using System.Text.Json;
using System.Text.Json.Serialization;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes
{
    public class NumericDefine : Define
    {
        public NumericDefine(IDefine defineFrom)
        {
            Name = defineFrom.Name;
            Value = defineFrom.Value;
        }

        public new int NumericValue
        {
            get => _numericValue;
            set => _numericValue = value;
        }

        public override bool IsNumeric
        {
            get => true;
        }
    }
}