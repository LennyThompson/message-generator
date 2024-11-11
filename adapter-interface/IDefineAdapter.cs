namespace adapter_interface;

public interface IDefineAdapter
{
    string Name { get; }
    string PlainName { get; }
    string Value { get; }
    int NumericValue { get; }
    bool HasStringValue { get; }
    bool HasNumericValue { get; }
    bool HasExpressionValue { get; }
}