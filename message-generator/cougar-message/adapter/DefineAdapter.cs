using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Adapter
{
    public class DefineAdapter
    {
        private readonly IDefine _defineAdapt;

        public DefineAdapter(IDefine define)
        {
            _defineAdapt = define;
        }

        public string GetName()
        {
            return _defineAdapt.Name();
        }

        public string GetPlainName()
        {
            return _defineAdapt.BaseName();
        }

        public string GetValue()
        {
            return _defineAdapt.Value();
        }

        public int GetNumericValue()
        {
            return _defineAdapt.NumericValue();
        }

        public bool HasStringValue => _defineAdapt.IsString();

        public bool HasNumericValue => _defineAdapt.IsNumeric();

        public bool HasExpressionValue => _defineAdapt.IsExpression();

        public bool IsJavaDefine =>
            HasStringValue || HasNumericValue || HasExpressionValue;

        public string GetJavaDefineValue()
        {
            if (HasExpressionValue)
            {
                return System.Text.RegularExpressions.Regex.Replace(GetValue(), @"J(TP|DS)_", "CougarDefines.J$1_");
            }
            return GetValue();
        }
    }
}