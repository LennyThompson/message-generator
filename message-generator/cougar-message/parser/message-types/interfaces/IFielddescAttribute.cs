using System;

namespace CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IFielddescAttribute : IAttribute
    {
        string FieldName { get; }
        string ShortDescription { get; }
        string LongDescription { get; }
    }
}