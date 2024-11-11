namespace adapter_interface;

public interface IEnumValueAdapter
{
    string Name { get; }
    string Value { get; }
}
public interface IEnumAdapter
{
    string Name { get; }
    string ShortName { get; }
    List<IEnumValueAdapter> Values { get; }
}
