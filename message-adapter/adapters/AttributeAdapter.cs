using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Adapter
{
    public class AttributeAdapter
    {
        protected IAttribute _attrAdapt;

        public AttributeAdapter(IAttribute attrAdapt)
        {
            _attrAdapt = attrAdapt;
        }

        public IAttribute AttrAdapt => _attrAdapt;
        public string Name => _attrAdapt.Name;

        public List<string> Values =>_attrAdapt.Values
                .Select(value => string.Join(" ", value))
                .ToList();

        public IAttribute.AttributeType Type => _attrAdapt.Type;
    }
}