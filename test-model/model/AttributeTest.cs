using CougarMessage.Parser.MessageTypes.Interfaces;

namespace test_model.model;

using CougarMessage.Parser.MessageTypes;

[TestFixture]
public class AttributeTest
{

    [Test]
    public void TestDescriptionAttribute()
    {
        //@description	SNotifyJackpotHit | Notify Jackpot Hit |
        Attribute attr = new Attribute();
        attr.Name = "description";
        attr.Type = IAttribute.AttributeType.Description;
        attr.Values.Add(new List<string>());
        attr.Values[0].Add("SNotifyJackpotHit");
        attr.Values.Add(new List<string>());
        attr.Values[1].Add("Notify");
        attr.Values[1].Add("Jackpot");
        attr.Values[1].Add("Hit");
        
        Assert.True(attr.Type == IAttribute.AttributeType.Description);
        Assert.That(attr.Values.Count, Is.EqualTo(2));
        Assert.That(attr.Values[0].Count, Is.EqualTo(1));
        Assert.That(attr.Values[1].Count, Is.EqualTo(3));
    }

    [Test]
    public void TestCategoryAttribute()
    {
        //@category		JACKPOT
        Attribute attr = new Attribute();
        attr.Name = "category";
        attr.Type = IAttribute.AttributeType.Category;
        attr.Values.Add(new List<string>());
        attr.Values[0].Add("JACKPOT");
        
        Assert.That(attr.Type, Is.EqualTo(IAttribute.AttributeType.Category));
        Assert.That(attr.Values.Count, Is.EqualTo(1));
        Assert.That(attr.Values[0].Count, Is.EqualTo(1));
    }

    [Test]
    public void TestGeneratorAttribute()
    {
        //@generator	JPC
        Attribute attr = new Attribute();
        attr.Name = "generator";
        attr.Type = IAttribute.AttributeType.Generator;
        attr.Values.Add(new List<string>());
        attr.Values[0].Add("JPC");
        
        Assert.That(attr.Type, Is.EqualTo(IAttribute.AttributeType.Generator));
        Assert.That(attr.Values.Count, Is.EqualTo(1));
        Assert.That(attr.Values[0].Count, Is.EqualTo(1));
    }

    [Test]
    public void TestConsumerAttribute()
    {
        //@consumer		JPD
        Attribute attr = new Attribute();
        attr.Name = "consumer";
        attr.Type = IAttribute.AttributeType.Consumer;
        attr.Values.Add(new List<string>());
        attr.Values[0].Add("JPD");
        
        Assert.That(attr.Type, Is.EqualTo(IAttribute.AttributeType.Consumer));
        Assert.That(attr.Values.Count, Is.EqualTo(1));
        Assert.That(attr.Values[0].Count, Is.EqualTo(1));
    }

    [Test]
    public void TestAlertLevelAttribute()
    {
        //@reason		sent by jpc to signal jp hit
        Attribute attr = new Attribute();
        attr.Name = "alertlevel";
        attr.Type = IAttribute.AttributeType.AlertLevel;
        attr.Values.Add(new List<string>());
        attr.Values[0].Add("ADVISORY");
        
        Assert.That(attr.Type, Is.EqualTo(IAttribute.AttributeType.AlertLevel));
        Assert.That(attr.Values.Count, Is.EqualTo(1));
        Assert.That(attr.Values[0].Count, Is.EqualTo(1));
    }

    [Test]
    public void TestReasonAttribute()
    {
        //@reason		sent by jpc to signal jp hit
        Attribute attr = new Attribute();
        attr.Name = "reason";
        attr.Type = IAttribute.AttributeType.Reason;
        attr.Values.Add(new List<string>());
        attr.Values[0].Add("sent");
        attr.Values[0].Add("by");
        attr.Values[0].Add("jpc");
        attr.Values[0].Add("to");
        attr.Values[0].Add("signal");
        attr.Values[0].Add("jp");
        attr.Values[0].Add("hit");
        
        Assert.That(attr.Type, Is.EqualTo(IAttribute.AttributeType.Reason));
        Assert.That(attr.Values.Count, Is.EqualTo(1));
        Assert.That(attr.Values[0].Count, Is.EqualTo(7));
    }

    [Test]
    public void TestWabFilterAttribute()
    {
        //@wabfilter	SUBSITE LHOST HOST GHOST
        Attribute attr = new Attribute();
        attr.Name = "wabfilter";
        attr.Type = IAttribute.AttributeType.WabFilter;
        attr.Values.Add(new List<string>());
        attr.Values[0].Add("SUBSITE");
        attr.Values[0].Add("LHOST");
        attr.Values[0].Add("HOST");
        attr.Values[0].Add("GHOST");
        
        Assert.That(attr.Type, Is.EqualTo(IAttribute.AttributeType.WabFilter));
        Assert.That(attr.Values.Count, Is.EqualTo(1));
        Assert.That(attr.Values[0].Count, Is.EqualTo(4));
    }

    [Test]
    public void TestWabFilterAttributeAsWabFilter()
    {
        //@wabfilter	SUBSITE LHOST HOST GHOST

        WabFilterAttribute attr = new WabFilterAttribute();
        attr.Name = "wabfilter";
        attr.Type = IAttribute.AttributeType.WabFilter;

        attr.SetSubSite(new string[]{});
        attr.SetLHost(new string[]{});
        attr.SetHost(new string[]{});
        attr.SetGroupHost(new string[]{});
        
        Assert.That(attr.Type, Is.EqualTo(IAttribute.AttributeType.WabFilter));
        Assert.That(attr.Values.Count, Is.EqualTo(0));
        Assert.True(attr.IsHost);
        Assert.That(attr.HostTarget, Is.Not.Null);
        Assert.That(attr.HostTarget.Target, Is.EqualTo(IWabFilterAttribute.Target.Host));
        Assert.That(attr.HostTarget.MemberPath, Is.Not.Null);
        Assert.That(attr.HostTarget.MemberPath, Is.Empty);
        Assert.True(attr.IsLHost);
        Assert.That(attr.LHostTarget, Is.Not.Null);
        Assert.That(attr.LHostTarget.Target, Is.EqualTo(IWabFilterAttribute.Target.LHost));
        Assert.That(attr.LHostTarget.MemberPath, Is.Not.Null);
        Assert.That(attr.LHostTarget.MemberPath, Is.Empty);
        Assert.True(attr.IsGroupHost);
        Assert.That(attr.GroupHostTarget, Is.Not.Null);
        Assert.That(attr.GroupHostTarget.Target, Is.EqualTo(IWabFilterAttribute.Target.Ghost));
        Assert.That(attr.GroupHostTarget.MemberPath, Is.Not.Null);
        Assert.That(attr.GroupHostTarget.MemberPath, Is.Empty);
        Assert.True(attr.IsSubSite);
        Assert.That(attr.SubSiteTarget, Is.Not.Null);
        Assert.That(attr.SubSiteTarget.Target, Is.EqualTo(IWabFilterAttribute.Target.Subsite));
        Assert.That(attr.SubSiteTarget.MemberPath, Is.Not.Null);
        Assert.That(attr.SubSiteTarget.MemberPath, Is.Empty);
    }
    
    [Test]
    public void TestWabFilterAttributeAsWabFilterWithTarget()
    {
        //@wabfilter	SITE GHOST:m_usFloorControllerSiteID HOST:m_usFloorControllerSiteID

        WabFilterAttribute attr = new WabFilterAttribute();
        attr.Name = "wabfilter";
        attr.Type = IAttribute.AttributeType.WabFilter;

        attr.SetSite(new string[]{});
        attr.SetGroupHost(new string[]{"m_usFloorControllerSiteID"});
        attr.SetHost(new string[]{"m_usFloorControllerSiteID"});
        
        Assert.That(attr.Type, Is.EqualTo(IAttribute.AttributeType.WabFilter));
        Assert.That(attr.Values.Count, Is.EqualTo(0));
        Assert.True(attr.IsHost);
        Assert.That(attr.HostTarget, Is.Not.Null);
        Assert.That(attr.HostTarget.Target, Is.EqualTo(IWabFilterAttribute.Target.Host));
        Assert.That(attr.HostTarget.MemberPath, Is.Not.Null);
        Assert.That(attr.HostTarget.MemberPath.Length, Is.EqualTo(1));
        Assert.That(attr.HostTarget.MemberPath[0], Is.EqualTo("m_usFloorControllerSiteID"));
        Assert.True(attr.IsGroupHost);
        Assert.That(attr.GroupHostTarget, Is.Not.Null);
        Assert.That(attr.GroupHostTarget.Target, Is.EqualTo(IWabFilterAttribute.Target.Ghost));
        Assert.That(attr.GroupHostTarget.MemberPath, Is.Not.Null);
        Assert.That(attr.GroupHostTarget.MemberPath.Length, Is.EqualTo(1));
        Assert.That(attr.GroupHostTarget.MemberPath[0], Is.EqualTo("m_usFloorControllerSiteID"));
        Assert.True(attr.IsSite);
        Assert.That(attr.SiteTarget, Is.Not.Null);
        Assert.That(attr.SiteTarget.Target, Is.EqualTo(IWabFilterAttribute.Target.Site));
        Assert.That(attr.SiteTarget.MemberPath, Is.Not.Null);
        Assert.That(attr.SiteTarget.MemberPath, Is.Empty);
    }

    [Test]
    public void TestWabFilterAttributeAsWabFilterWithChildTarget()
    {
        //@wabfilter	SITE HOST:info.m_usSiteID

        WabFilterAttribute attr = new WabFilterAttribute();
        attr.Name = "wabfilter";
        attr.Type = IAttribute.AttributeType.WabFilter;

        attr.SetSite(new string[]{});
        attr.SetHost(new string[]{"info", "m_usSiteID" });
        
        Assert.That(attr.Type, Is.EqualTo(IAttribute.AttributeType.WabFilter));
        Assert.That(attr.Values.Count, Is.EqualTo(0));
        Assert.True(attr.IsHost);
        Assert.That(attr.HostTarget, Is.Not.Null);
        Assert.That(attr.HostTarget.Target, Is.EqualTo(IWabFilterAttribute.Target.Host));
        Assert.That(attr.HostTarget.MemberPath, Is.Not.Null);
        Assert.That(attr.HostTarget.MemberPath.Length, Is.EqualTo(2));
        Assert.That(attr.HostTarget.MemberPath[0], Is.EqualTo("info"));
        Assert.That(attr.HostTarget.MemberPath[1], Is.EqualTo("m_usSiteID"));
        Assert.False(attr.IsGroupHost);
        Assert.True(attr.IsSite);
        Assert.That(attr.SiteTarget, Is.Not.Null);
        Assert.That(attr.SiteTarget.Target, Is.EqualTo(IWabFilterAttribute.Target.Site));
        Assert.That(attr.SiteTarget.MemberPath, Is.Not.Null);
        Assert.That(attr.SiteTarget.MemberPath, Is.Empty);
    }
}
