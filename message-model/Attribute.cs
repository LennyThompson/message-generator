using System;
using System.Collections.Generic;
using System.Linq;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes
{
    public class Attribute : IAttribute
    {
        protected string m_strName = "";
        protected List<List<string>> m_listValues = new List<List<string>>();
        protected IAttribute.AttributeType m_attrType = IAttribute.AttributeType.Any;
        

        public string Name
        {
            get => m_strName;
            set => m_strName = value;
        }

        public IAttribute.AttributeType Type
        {
            get => m_attrType;
            set => m_attrType = value;
        }

        public void AddValues(List<string> listValues)
        {
            m_listValues.Add(new List<string>(listValues));
        }

        public List<List<string>> Values
        {
            get => m_listValues;
            set => m_listValues = value;
        }

        public virtual void AddValue(string name)
        {
            m_listValues.Add(new List<string> { name });
        }
    }
}