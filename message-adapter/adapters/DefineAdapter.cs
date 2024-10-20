using System.Text.Json;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Adapter
{
    public class DefineAdapter
    {
        private readonly IDefine _defineAdapt;

        public DefineAdapter(IDefine define)
        {
            _defineAdapt = define;
        }

        public string Name => _defineAdapt.Name;

        public string PlainName => _defineAdapt.BaseName;

        public string Value => _defineAdapt.Value;

        public int NumericValue => _defineAdapt.NumericValue;

        public bool HasStringValue => _defineAdapt.IsString;

        public bool HasNumericValue => _defineAdapt.IsNumeric;

        public bool HasExpressionValue => _defineAdapt.IsExpression;

        public bool IsJavaDefine =>
            HasStringValue || HasNumericValue || HasExpressionValue;

        public string JavaDefineValue
        {
            get
            {
                if (HasExpressionValue)
                {
                    return System.Text.RegularExpressions.Regex.Replace(Value, @"J(TP|DS)_", "CougarDefines.J$1_");
                }

                return Value;
            }
        }
    }
}