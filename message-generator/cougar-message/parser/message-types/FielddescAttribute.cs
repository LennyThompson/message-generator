using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Parser.MessageTypes
{
    public class FielddescAttribute : Attribute, IFielddescAttribute
    {
        private string _fieldName;

        public FielddescAttribute()
        {
        }

        public void SetFieldName(string fieldName)
        {
            _fieldName = fieldName;
        }

        public string FieldName()
        {
            return _fieldName;
        }

        public string ShortDescription()
        {
            return ValueAsString(0);
        }

        public string LongDescription()
        {
            return ValueAsString(1);
        }

        private string ValueAsString(int index)
        {
            return index < Values().Count ? string.Join(" ", Values()[index]) : "";
        }
    }
}