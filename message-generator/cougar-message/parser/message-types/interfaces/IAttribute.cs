using System;
using System.Collections.Generic;

namespace CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IAttribute
    {
        public enum AttributeType 
        { 
            Description, 
            Category, 
            Generator, 
            Consumer, 
            AlertLevel, 
            Reason, 
            WabFilter, 
            FieldDesc, 
            Any 
        };

        string Name { get; }
        List<List<string>> Values { get; }
        AttributeType Type { get; }
        void AddValue(string name);
    }
}