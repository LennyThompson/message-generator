using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.CougarMessage.Parser.MessageTypes
{
    public class Attribute : IAttribute
    {
        protected string m_strName;
        protected List<List<string>> m_listValues;
        protected AttributeType m_attrType;

        public Attribute()
        {
            m_listValues = new List<List<string>>();
            m_attrType = AttributeType.ANY;
        }

        public void SetName(string strName)
        {
            m_strName = strName;
        }

        public void SetType(AttributeType attrType)
        {
            m_attrType = attrType;
        }

        public void AddValues(List<string> listValues)
        {
            m_listValues.Add(new List<string>(listValues));
        }

        public string Name()
        {
            return m_strName;
        }

        public List<List<string>> Values()
        {
            return m_listValues;
        }

        public AttributeType Type()
        {
            return m_attrType;
        }

        public virtual void AddValue(string name)
        {
            m_listValues.Add(new List<string> { name });
        }
    }
}