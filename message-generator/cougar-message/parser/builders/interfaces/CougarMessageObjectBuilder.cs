using System.Text.Json;
using Net.Interfaces;
using Net.CougarMessage.Grammar;

namespace Net.CougarMessage.Parser.Builders.Interfaces
{
    public interface ICougarMessageObjectBuilder : IParserObjectBuilder, ICougarParserListener
    {
    }
}