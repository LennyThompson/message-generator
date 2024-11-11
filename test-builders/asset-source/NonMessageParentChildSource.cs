namespace test_builders.asset_source;

public class NonMessageParentChildSource
{
    public static IEnumerable<string> TestSourceCode
    {
        get
        {
            using TextReader readerSimple = new StreamReader("assets/non-message-parent-child.h");
            yield return readerSimple.ReadToEnd();
        }
    }

}