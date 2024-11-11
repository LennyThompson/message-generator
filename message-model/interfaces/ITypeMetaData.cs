namespace CougarMessage.Metadata;

public interface ITypeMetaData
{
    string Name { get; set; }
    AdditionalAttribute? AdditionalAttr { get; set; }
    List<MemberMetadata>? MetaMembers { get; set; }
}