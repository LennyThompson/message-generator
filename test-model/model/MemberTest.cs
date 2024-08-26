using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Attribute = CougarMessage.Parser.MessageTypes.Attribute;

namespace test_model.model;

[TestFixture]
public class MemberTest
{
    [Test]
    public void TestSimpleMember()
    {
        // 	USHORT		m_usSiteID;								//@fielddesc	m_usSiteID | Site ID | Site ID of where the machine is
        Member member = new Member();
        
        member.Type = "USHORT";
        member.Name = "m_usSiteID";
        Attribute attribute = new();
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_usSiteID";
        attribute.AddValue("Site ID");
        attribute.AddValue("Site ID of where the machine is");
        member.AddAttribute(attribute);
        
        Assert.That(member.Type, Is.EqualTo("USHORT"));
        Assert.That(member.Name, Is.EqualTo("m_usSiteID"));
        Assert.That(member.ShortName, Is.EqualTo("usSiteID"));
        Assert.That(member.StrippedName, Is.EqualTo("SiteID"));
        Assert.That(member.Attributes, Is.Not.Null);
        Assert.That(member.Attributes!.Count, Is.EqualTo(1));
        Assert.That(member.Attributes![0].Type, Is.EqualTo(IAttribute.AttributeType.FieldDesc));
        Assert.That(member.Attributes![0].Name, Is.EqualTo("m_usSiteID"));
        Assert.That(member.Attributes![0].Values.Count(), Is.EqualTo(2));
    }
    [Test]
    public void TestArrayStringMember()
    {
        // 	char		m_szComponent[JTP_NAME_SIZE];	//@fielddesc	m_szComponent | Component name | source component
        Member member = new Member();
        
        member.Type = "char";
        member.Name = "m_szComponent";
        Attribute attribute = new();
        
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_szComponent";
        attribute.AddValue("Somponent name");
        attribute.AddValue("source component");
        member.AddAttribute(attribute);

        
    }
}