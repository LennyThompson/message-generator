using System;
using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Parser.MessageTypes
{
    public class EnumValue : IEnumValue
    {
        private string _name;
        private bool _hasValue = false;
        private int _value;
        private int _ordinal;

        public EnumValue(int ordinal)
        {
            _ordinal = ordinal;
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public void SetValue(int value)
        {
            _hasValue = true;
            _value = value;
        }

        public string Name()
        {
            return _name;
        }

        public bool HasValue()
        {
            return _hasValue;
        }

        public int Value()
        {
            return _hasValue ? _value : _ordinal;
        }

        public int Ordinal()
        {
            return _ordinal;
        }
    }
}