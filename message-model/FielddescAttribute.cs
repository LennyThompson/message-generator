using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes
{
    public class FielddescAttribute : Attribute, IFielddescAttribute
    {
        public string FieldName { get; set; } = "";

        public string ShortDescription => ValueAsString(0);

        public string LongDescription => ValueAsString(1);

        private string ValueAsString(int index)
        {
            return index < Values.Count ? string.Join(" ", Values[index]) : "";
        }
    }
}