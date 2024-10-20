namespace test_builders.asset_source;

public class SchemaTestSource
{
    public static IEnumerable<string> TestSourceCode
    {
        get
        {
            using TextReader readerSimple = new StreamReader("assets/SchemaTest.h");
            yield return readerSimple.ReadToEnd();
        }
    }
}

