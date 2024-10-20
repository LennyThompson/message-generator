using CougarMessage.Parser.MessageTypes;
using NuGet.Frameworks;

namespace test_model.model;

[TestFixture]
public class EnumTest
{
    [Test]
    public void TestSimpleEnum()
    {
        /*
        enum ESourceOfFunds
        {
            Undefined							= 0,
            EGMCredits							= 1,
            InHouseLPJackpot					= 2,
            InHouseExternalJackpot				= 3,
            WideAreaLPJackpot					= 4,
            WideAreaExternalJackpot				= 5,
        };
        */
        EnumDefinition enumDef = new();
        enumDef.Name = "ESourceOfFunds";
        enumDef.AddValue(new EnumValue(0) { Name = "Undefined", Value = 0 });
        enumDef.AddValue(new EnumValue(1) { Name = "EGMCredits", Value = 1 });
        enumDef.AddValue(new EnumValue(2) { Name = "InHouseLPJackpot", Value = 2 });
        enumDef.AddValue(new EnumValue(3) { Name = "InHouseExternalJackpot", Value = 3 });
        enumDef.AddValue(new EnumValue(4) { Name = "WideAreaLPJackpot", Value = 4 });
        enumDef.AddValue(new EnumValue(5) { Name = "WideAreaExternalJackpot", Value = 5 });
        
        Assert.That(enumDef.Name, Is.EqualTo("ESourceOfFunds"));
        Assert.That(enumDef.Values.Count, Is.EqualTo(6));
    }
}