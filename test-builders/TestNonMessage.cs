using CougarMessage.Parser.MessageTypes.Interfaces;
using CougarMessages.Parser;
using test_builders.asset_source;

namespace test_builders;

[TestFixture]
public class TestNonMessage
{
    [Test]
    [TestCaseSource(typeof(NonMessageParentChildSource), nameof(NonMessageParentChildSource.TestSourceCode))]
    public void TestNonMessageParentChild(string strSource)
    {
        // TODO: work out what this test should really be doing...
        /*
        StringReader reader = new StringReader(strSource);
        CougarMessageListener cougarListener = CougarMessageListener.RunGrammar(reader, null);

        IMessageSchema schema = cougarListener.Schema();

        Assert.That(schema.Messages.Count, Is.EqualTo(2));

        IMessage msgParent = schema.FindMessage("SParent");

        Assert.That(msgParent, Is.Not.Null);
        Assert.That(msgParent.Members.Count, Is.EqualTo(4));

        IMessage msgChild = schema.FindMessage("SChild");

        Assert.That(msgChild, Is.Not.Null);
        Assert.That(msgChild.Members.Count, Is.EqualTo(4));
        IMember member = msgChild.Members[1];
        Assert.That(member.Name, Is.EqualTo("m_strSQLstring"));
        Assert.That(member.Type, Is.EqualTo("std::string"));
        //Assert.That(member.IsStdType, Is.True);
        Assert.That(member.MessageType, Is.Null);
        Assert.That(member.IsArray, Is.False);

        member = msgChild.Members[2];
        Assert.That(member.Name, Is.EqualTo("m_listValues"));
        //Assert.That(member.Type, Is.EqualTo("List<int>"));
        //Assert.That(member.IsStdType, Is.True);
        Assert.That(member.MessageType, Is.Null);
        Assert.That(member.IsArray, Is.True);
        Assert.That(member.IsVariableLengthArray, Is.True);

        member = msgChild.Members[3];
        Assert.That(member.Name, Is.EqualTo("m_mapStrings"));
        //Assert.That(member.Type, Is.EqualTo("Dictionary<int, string>"));
        //Assert.That(member.IsStdType, Is.True);
        Assert.That(member.MessageType, Is.Null);
        Assert.That(member.IsArray, Is.True);
        Assert.That(member.IsVariableLengthArray, Is.True);
        /*
        stdType = (IMemberStdType)member;
        Assert.That(stdType.TypeParameters.Length, Is.EqualTo(2));
        Assert.That(stdType.TypeParameters[0].Name, Is.EqualTo("int"));
        Assert.That(stdType.TypeParameters[1].Name, Is.EqualTo("string"));
        #1#

        /*
        MessageAdapter messageAdapter = MessageAdapterFactory.CreateMessageAdapter(msgParent);
        MemberAdapter memberAdapter = messageAdapter.Members[2];
        TypeAdapter typeAdapter = memberAdapter.TypeAdapter;
        Assert.That(typeAdapter.HasMessageType, Is.True);
        Assert.That(typeAdapter.MessageType.IsNonMessage, Is.True);
    #1#
    */
    }

}