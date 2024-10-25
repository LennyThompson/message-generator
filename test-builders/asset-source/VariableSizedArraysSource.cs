namespace test_builders.asset_source;

public class VariableSizedArraysSource
{
    public static IEnumerable<string> TestSourceCode
    {
        get
        {
            using TextReader readerSimple = new StreamReader("assets/variable-sized-string-messages.h");
            yield return readerSimple.ReadToEnd();
        }
    }
}
