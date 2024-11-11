namespace test_builders.asset_source;

public class UnknownSizedMessageSource
{
    public static IEnumerable<string> TestSourceCode
    {
        get
        {
            using TextReader readerSimple = new StreamReader("assets/unknown-size-messages.h");
            yield return readerSimple.ReadToEnd();
        }
    }

}