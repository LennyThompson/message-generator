using CougarMessage.Metadata;

namespace test_model;

public class AttributeJsonInputData : JsonInputData<AdditionalAttribute>
{
    protected override string SourcePath => "assets";
    protected override string SourceFile => "additional-attributes";
}