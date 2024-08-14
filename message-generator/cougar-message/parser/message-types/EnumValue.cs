using System;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes
{
    public class EnumValue(int ordinal) : IEnumValue
    {
        private bool _hasValue = false;
        private int _value;

        public string Name { get; set; } = "";

        public bool HasValue => _hasValue;

        public int Value
        {
            get => _hasValue ? _value : ordinal;
            set
            {
                _hasValue = true;
                _value = value;
               
            }
        }

        public int Ordinal => ordinal;
    }
}