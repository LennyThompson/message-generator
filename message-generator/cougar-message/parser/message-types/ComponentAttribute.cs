using System;
using System.Collections.Generic;
using System.Linq;

namespace CougarMessage.Parser.MessageTypes
{
    public class ComponentAttribute : Attribute
    {
        public override void AddValue(string value)
        {
            if (m_listValues.Count > 0)
            {
                m_listValues[0] = new List<string> { m_listValues[0][0] + "," + value };
            }
            else
            {
                m_listValues.Add(new List<string> { value });
            }
        }
    }
}