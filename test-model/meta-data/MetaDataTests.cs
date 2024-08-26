using CougarMessage.Metadata;

namespace test_model;

public class TestAdditionalAttribute
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCaseSource(typeof(AttributeJsonInputData))]
    public void TestAdditionalAttrSerialisationFrom(AdditionalAttribute attribute)
    {
        Assert.NotNull(attribute.Name);
        Assert.NotNull(attribute.Parameters);
        if (attribute.Parameters.Count > 0)
        {
            attribute.Parameters.ForEach(Assert.NotNull);
        }
    }
    
    [Test]
    [TestCaseSource(typeof(MemberMetadataJsonInput))]
    public void TestMemberMetadataSerialisationFrom(MemberMetadata memberMetatdata)
    {
        Assert.NotNull(memberMetatdata.Name);
        if (memberMetatdata.HasAdditionalAttribute)
        {
            Assert.NotNull(memberMetatdata.AdditionalAttr);
            Assert.NotNull(memberMetatdata.AdditionalAttr.Name);
            Assert.NotNull(memberMetatdata.AdditionalAttr.Parameters);
        }

        Assert.True(memberMetatdata.IsCSharp);

        if (memberMetatdata.HasConversion)
        {
            Assert.NotNull(memberMetatdata.Conversion);
        }

        if (memberMetatdata.HasTypeUpdate)
        {
            Assert.NotNull(memberMetatdata.TypeUpdate);
        }

        if (memberMetatdata.HasSerialiser)
        {
            Assert.NotNull(memberMetatdata.Serialiser);
        }
    }

    [Test]
    [TestCaseSource(typeof(TypeMetaDataJsonInput))]
    public void TestMetadataSerialisationFrom(TypeMetaData metaData)
    {
        Assert.NotNull(metaData);
    }
}