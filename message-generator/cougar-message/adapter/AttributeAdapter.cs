using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Adapter
{
    public class AttributeAdapter
    {
        protected IAttribute _attrAdapt;

        public AttributeAdapter(IAttribute attrAdapt)
        {
            _attrAdapt = attrAdapt;
        }

        public string GetName()
        {
            return _attrAdapt.Name();
        }

        public List<string> GetValues()
        {
            return _attrAdapt.Values()
                .Select(value => string.Join(" ", value))
                .ToList();
        }

        public IAttribute.AttributeType GetType()
        {
            return _attrAdapt.Type();
        }
    }
}