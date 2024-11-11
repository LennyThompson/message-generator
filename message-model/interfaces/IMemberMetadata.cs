namespace CougarMessage.Metadata;

public interface IMemberMetadata
{
    string Name { get; set; }
    AdditionalAttribute? AdditionalAttr { get; set; }
    string? RemoveAttribute { get; set; }
    string? TypeUpdate { get; set; }
    string? TargetType { get; set; }
    string? Conversion { get; set; }
    string? Serialiser { get; set; }
    bool HasConversion { get; }
    bool HasAdditionalAttribute { get; }
    bool HasRemoveAttribute { get; }
    bool HasTypeUpdate { get; }
    bool HasSerialiser { get; }
    bool IsCSharp { get; }
    bool IsCPP { get; }
    bool GetIsJava { get; }
    bool GetIsDart { get; }
    bool GetIsJavaScript { get; }
}