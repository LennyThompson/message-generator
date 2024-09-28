using System.Text.Json;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.Builders.Interfaces
{
    public interface IAttributeBuilder
    {
        IAttribute Attribute { get; }
    }
}