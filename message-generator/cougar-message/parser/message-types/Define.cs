using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes
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

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Value
        {
            get => _value;
            set => _value = value;
        }

        public int NumericValue => IsNumeric ? _numericValue : 0;

        public string BaseName
        {
            get 
            { 
                if (Name.StartsWith("JTP_"))
                {
                    return Name.Substring(4);
                }
                return Name;
            }
        }

        public bool IsNumeric => _numericValue > 0;

        public bool IsExpression => false;

        public bool IsString => false;

        public bool Evaluate(List<IDefine> defines)
        {
            if (!string.IsNullOrEmpty(_value))
            {
                var defineFound = defines.FirstOrDefault(define => define.Name == _value);
                if (defineFound != null && defineFound.IsNumeric)
                {
                    _numericValue = defineFound.NumericValue;
                    return true;
                }
            }
            return false;
        }
    }
}