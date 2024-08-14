using System.Text.Json;
using Interfaces;
using CougarMessage.Grammar;

namespace CougarMessage.Parser.Builders.Interfaces
{
    public interface CougarMessageObjectBuilder : ParserObjectBuilder, ICougarParserListener
    {
    }
}