namespace adapter_interface;


public interface IMemberAdapter
{
    string Name { get; }
    string Type { get; }
    ITypeAdapter TypeAdapter();
    bool HasMessageType { get; }
    bool HasEnumType { get; }
    IMessageAdapter? MessageType { get; }
    IEnumAdapter? EnumType { get; }
    bool HasFieldDescriptionAttribute { get; }
    IAttributeAdapter? FieldDescriptionAttribute { get; }
    int NumericArraySize { get; }
    bool HasNumericArraySize { get; }
    bool IsArray { get; }
    bool IsDeclaredArray { get; }
    bool IsArrayPointer { get; }
    bool IsVariableLengthArray { get; }
    bool HasArraySizeDefine { get; }
    IDefineAdapter? ArraySizeDefine { get; }
    string CppArrayType { get; }
    bool GenerateArrayMember { get; }
    string ArraySize { get; }
    string ShortName { get; }
    string StrippedName { get; }
    bool IsString { get; }
    bool IsParameterString { get; }
    bool IsHashString { get; }
    bool IsFiletime { get; }
    bool IsBoolean { get; }
    string SingleValueCppType { get; }
    string CSharpBinaryConversion { get; }
    bool HasValueConversion { get; }
    string ValueConversion { get; }
    int NameLength { get; }
    int ValueAsStringLength { get; }
    string OriginalType { get; }
    int OriginalByteSize { get; }
    bool HasAdditionalAttribute { get; }
    string AdditionalAttribute { get; }
    bool HasMetadata { get; }
    IMemberMetadata Metadata { get; }
}
