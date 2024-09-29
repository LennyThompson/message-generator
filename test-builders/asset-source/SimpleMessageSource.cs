namespace test_builders.asset_source;

public class SimpleMessageSource
{
    public static IEnumerable<string> TestSourceCode
    {
        get
        {
            using TextReader readerSimple = new StreamReader("assets/SimpleMessage.h");
            yield return readerSimple.ReadToEnd();
        }
    }
}

public class ExtendedMessageSource
{
    public static IEnumerable<string> TestSourceCode
    {
        get
        {
            using TextReader reader = new StreamReader("assets/ExtendedMessage.h");
            yield return reader.ReadToEnd();
        }
    }
}

public class CombinedMessageSource
{
    public static IEnumerable<string> TestSourceCode
    {
        get
        {
            using TextReader readerSimple = new StreamReader("assets/SimpleMessage.h");
            using TextReader readerExtended = new StreamReader("assets/ExtendedMessage.h");
            yield return readerSimple.ReadToEnd() + "\n" + readerExtended.ReadToEnd();
        }
    }
}

