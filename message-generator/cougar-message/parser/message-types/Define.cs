using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Parser.MessageTypes
{
    public class Define : IDefine
    {
        private string _name;
        private string _value;
        protected int _numericValue;

        public Define()
        {
            _numericValue = -1;
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public void SetValue(string value)
        {
            _value = value;
        }

        public string Name()
        {
            return _name;
        }

        public string Value()
        {
            return _value;
        }

        public int NumericValue()
        {
            return IsNumeric() ? _numericValue : 0;
        }

        public string BaseName()
        {
            if (_name.StartsWith("JTP_"))
            {
                return _name.Substring(4);
            }
            return _name;
        }

        public bool IsNumeric()
        {
            return _numericValue > 0;
        }

        public bool IsExpression()
        {
            return false;
        }

        public bool IsString()
        {
            return false;
        }

        public bool Evaluate(List<IDefine> defines)
        {
            if (!string.IsNullOrEmpty(_value))
            {
                var defineFound = defines.FirstOrDefault(define => define.Name() == _value);
                if (defineFound != null && defineFound.IsNumeric())
                {
                    _numericValue = defineFound.NumericValue();
                    return true;
                }
            }
            return false;
        }
    }
}