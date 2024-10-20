using CougarMessage.Metadata;

namespace test_model;

public class TypeMetaDataJsonInput : JsonInputData<TypeMetaData>
{
    protected override string SourcePath => "assets";
    protected override string SourceFile => "type-metadata";

}