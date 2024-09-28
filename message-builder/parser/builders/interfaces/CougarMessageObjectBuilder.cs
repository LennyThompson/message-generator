using System.Text.Json;
using Interfaces;

namespace CougarMessage.Parser.Builders.Interfaces
{
    public interface CougarMessageObjectBuilder : ParserObjectBuilder, ICougarParserListener
    {
    }
}