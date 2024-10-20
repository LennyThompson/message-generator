using CougarMessage.Metadata;

namespace test_model;

public class MemberMetadataJsonInput : JsonInputData<MemberMetadata>
{
    protected override string SourcePath => "assets";
    protected override string SourceFile => "member-metadata";

}