using CougarMessage.Parser.MessageTypes.Interfaces;
using CougarMessages.Parser;
using test_builders.asset_source;

namespace test_builders;

[TestFixture]
public class TestStrippedNameClash
{
    [Test]
    [TestCaseSource(typeof(CardPatronDetailsSource), nameof(CardPatronDetailsSource.TestSourceCode))]
    public void testStrippedNameClash(string strSource)
    {
        var reader = new StringReader(strSource);
        var cougarListener = CougarMessageListener.RunGrammar(reader, null);

        var schema = cougarListener.Schema();

        Assert.That(schema.Messages.Count, Is.EqualTo(1));
        var msg = schema.Messages[0];
        Assert.That(msg, Is.Not.Null);
        Assert.That(msg.Members.Count, Is.EqualTo(8));
        var member = msg.Members[3];
        Assert.That(member, Is.Not.Null);
        Assert.That(member.StrippedName, Is.EqualTo("Status"));
        member = msg.Members[5];
        Assert.That(member.Name, Is.EqualTo("m_memberStatus"));
        Assert.That(member.Prefix, Is.EqualTo("member"));
        Assert.That(member.ShortName, Is.EqualTo("memberStatus"));
        Assert.That(member, Is.Not.Null);
        Assert.That(member.StrippedName, Is.EqualTo("MemberStatus"));

        member = msg.Members[7];
        Assert.That(member, Is.Not.Null);
        Assert.That(member.StrippedName, Is.EqualTo("RequestStatus"));

        var attrWabfilter = msg.WabFilterAttribute;

        Assert.That(attrWabfilter, Is.Not.Null);
        Assert.That(attrWabfilter, Is.InstanceOf<IWabFilterAttribute>());
        var wabFilter = (IWabFilterAttribute)attrWabfilter;
        Assert.That(wabFilter.IsSite, Is.False);
        Assert.That(wabFilter.IsHost, Is.True);

        var wabFilterTarget = ((IWabFilterAttribute)attrWabfilter).HostTarget;
        Assert.That(wabFilterTarget, Is.Not.Null);
        Assert.That(wabFilterTarget.Filter, Is.EqualTo("Host:m_usSiteID"));
        Assert.That(wabFilterTarget.Name, Is.EqualTo("Host"));
        Assert.That(wabFilterTarget.MemberPath[0], Is.EqualTo("m_usSiteID"));

        int nByteOffset = wabFilterTarget.MemberPath
            .Sum(path => 
                msg.Members
                    .TakeWhile(memberTest => memberTest.Name != path)
                    .Sum(member => member.OriginalByteSize)
            );
        Assert.That(nByteOffset, Is.EqualTo(4));
    }

    
}