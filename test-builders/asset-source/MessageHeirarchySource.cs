namespace test_builders.asset_source;

public class MessageHeirarchySource
{
    public static IEnumerable<string> TestSourceCode
    {
        get
        {
            using TextReader readerSimple = new StreamReader("assets/contrived-message-heirarchy.h");
            yield return readerSimple.ReadToEnd();
        }
    }

}