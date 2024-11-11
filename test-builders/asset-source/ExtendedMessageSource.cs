namespace test_builders.asset_source;

public class ExtendedMessageSource
{
    public static IEnumerable<string> TestSourceCode
    {
        get
        {
            using TextReader readerSimple = new StreamReader("assets/ExtendedMessage.h");
            yield return readerSimple.ReadToEnd();
        }
    }

}