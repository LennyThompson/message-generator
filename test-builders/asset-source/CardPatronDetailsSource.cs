namespace test_builders.asset_source;

public class CardPatronDetailsSource
{
    public static IEnumerable<string> TestSourceCode
    {
        get
        {
            using TextReader readerSimple = new StreamReader("assets/card-patron-details.h");
            yield return readerSimple.ReadToEnd();
        }
    }

}