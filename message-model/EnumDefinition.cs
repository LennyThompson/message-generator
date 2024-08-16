using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes
{
    public class EnumDefinition : IEnum
    {
        private string _name = "";
        private List<IEnumValue> _values = new();

        public void AddValue(IEnumValue enumValue)
        {
            if (_values.Any(enumDef => string.Compare(enumDef.Name, enumValue.Name, StringComparison.Ordinal) == 0))
            {
                int temp = _values.Count;
            }
            _values.Add(enumValue);
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public List<IEnumValue> Values => _values;
    }
}