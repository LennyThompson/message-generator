using CougarMessage.Parser.MessageTypes.Interfaces;
using CougarMessages.Parser;
using test_builders.asset_source;

namespace test_builders;

[TestFixture]
public class TestMessageHeirarchy
{
    [Test]
    [TestCaseSource(typeof(MessageHeirarchySource), nameof(MessageHeirarchySource.TestSourceCode))]
    public void TestMessageHierarchyMemberPats(string strSource)
    {
        var reader = new StringReader(strSource);
        var cougarListener = CougarMessageListener.RunGrammar(reader, null);

        var schema = cougarListener.Schema();

        Assert.That(schema.Messages.Count, Is.EqualTo(3));

        var msgCashierAdj = schema.FindMessage("SCashierAdjustmentRequest_V2");

        Assert.That(msgCashierAdj.Members.Count, Is.EqualTo(12));

        var listMessagePaths = new List<Stack<IMember>>();
        Assert.That(msgCashierAdj.FindAllMembers(member => member.Name == "m_dwCardID", listMessagePaths), Is.True);

        Assert.That(listMessagePaths.Count, Is.EqualTo(3));

        Assert.That(listMessagePaths[0].Count, Is.EqualTo(3));
        Assert.That(listMessagePaths[0].Pop().Name, Is.EqualTo("m_adjustmentCashier"));
        Assert.That(listMessagePaths[0].Pop().Name, Is.EqualTo("m_adjustmentCashier"));
        Assert.That(listMessagePaths[0].Pop().Name, Is.EqualTo("m_dwCardID"));
        Assert.That(listMessagePaths[1].Count, Is.EqualTo(2));
        Assert.That(listMessagePaths[1].Pop().Name, Is.EqualTo("m_adjustmentCashier"));
        Assert.That(listMessagePaths[1].Pop().Name, Is.EqualTo("m_dwCardID"));
        Assert.That(listMessagePaths[2].Count, Is.EqualTo(1));
        Assert.That(listMessagePaths[2].Pop().Name, Is.EqualTo("m_dwCardID"));
    }
    
}