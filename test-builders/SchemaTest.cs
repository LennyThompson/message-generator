using CougarMessage.Parser.MessageTypes.Interfaces;
using CougarMessages.Parser;
using test_builders.asset_source;

namespace test_builders;

[TestFixture]
public class SchemaTest
{
    [Test]
    [TestCaseSource(typeof(SchemaTestSource), nameof(SchemaTestSource.TestSourceCode))]
    public void TestSimpleCougarMessage(string strSchemaTest)
    {
        StringReader reader = new StringReader(strSchemaTest);
        CougarMessageListener cougarListener = CougarMessageListener.RunGrammar(reader, null);

        IMessageSchema schema = cougarListener.Schema();

        Assert.IsNotNull(schema);
        
        List<IMessage> listMessages = schema.Messages;
        Assert.That(listMessages.Count, Is.EqualTo(7));
        Assert.That(schema.Defines.Count, Is.EqualTo(6));
        
        Assert.That(listMessages[0].Name, Is.EqualTo("SUnsuspendEGM"));
        Assert.That(listMessages[0] .HasStrippedNameMemberClash, Is.False);
        Assert.That(listMessages[1] .Name, Is.EqualTo("SEGMSuspended"));
        Assert.That(listMessages[1] .HasStrippedNameMemberClash, Is.False);
        Assert.That(listMessages[2] .Name, Is.EqualTo("SEGMUnsuspended"));
        Assert.That(listMessages[2] .HasStrippedNameMemberClash, Is.False);
        Assert.That(listMessages[3] .Name, Is.EqualTo("SSetJackpotContributionMessageTimer"));
        Assert.That(listMessages[3] .HasStrippedNameMemberClash, Is.False);
        Assert.That(listMessages[4] .Name, Is.EqualTo("SEGMMeters"));
        Assert.That(listMessages[4] .HasStrippedNameMemberClash, Is.False);
        Assert.That(listMessages[5] .Name, Is.EqualTo("SEGMMeters_V2"));
        Assert.That(listMessages[5] .HasStrippedNameMemberClash, Is.False);
        Assert.That(listMessages[6] .Name, Is.EqualTo("SSetJackpotContributionMessageTimer_V2"));
        Assert.That(listMessages[6] .HasStrippedNameMemberClash, Is.False);

        IMessage msgType = listMessages[0];
        Assert.That(msgType.Members.Count, Is.EqualTo(3));
        IMember memberEnum = msgType.Members[1];
        Assert.That(memberEnum .Name, Is.EqualTo("m_dwEGMSerialNumber"));
        Assert.That(memberEnum.EnumType, Is.Not.Null);
        Assert.That(memberEnum.EnumType.Name, Is.EqualTo("EEGMDeviceType"));
        Assert.That(memberEnum.EnumType.Values.Count, Is.EqualTo(2));
        Assert.That(memberEnum.EnumType.Values[0] .Name, Is.EqualTo("EGMDevice_Egm"));
        Assert.That(memberEnum.EnumType.Values[0].HasValue, Is.True);
        Assert.That(memberEnum.EnumType.Values[0].Value, Is.EqualTo(0));
        Assert.That(memberEnum.EnumType.Values[0].Ordinal, Is.EqualTo(0));
        Assert.That(memberEnum.EnumType.Values[1] .Name, Is.EqualTo("EGMDevice_NoteAcceptor"));
        Assert.That(memberEnum.EnumType.Values[1].HasValue, Is.True);
        Assert.That(memberEnum.EnumType.Values[1].Value, Is.EqualTo(5));
        Assert.That(memberEnum.EnumType.Values[1].Ordinal, Is.EqualTo(1));

        IMember memberMsg = listMessages[6].Members.FirstOrDefault(member => member.Name == "m_usTimerValue");
        Assert.That(memberMsg, Is.Not.Null);
        Assert.That(memberMsg.MessageType, Is.Not.Null);
        Assert.That(memberMsg.MessageType.Name, Is.EqualTo("SEGMMeters"));

        // The member closest to the top of the struct memory

        Stack<IMember> stackMembers = new Stack<IMember>();
        Assert.That(listMessages[6].FindTopMostMember(member => member.ShortFieldDescription.IndexOf("Site ID") >= 0, stackMembers), Is.True);
        Assert.That(stackMembers.Count, Is.EqualTo(2));
        Assert.That(stackMembers.ElementAt(0) .Name, Is.EqualTo("m_usTimerValue"));
        Assert.That(stackMembers.ElementAt(1) .Name, Is.EqualTo("m_usJackpotSiteID"));
        stackMembers.Clear();

        // The member highest in the struct hierarchy

        Assert.That(listMessages[6].FindMember(member => member.ShortFieldDescription.IndexOf("Site ID") >= 0, stackMembers), Is.True);
        Assert.That(stackMembers.Count, Is.EqualTo(1));
        Assert.That(stackMembers.Peek() .Name, Is.EqualTo("m_usFloorControllerSiteID"));
        Assert.That(listMessages[4].IsNonMessage, Is.True);
        Assert.That(listMessages[5].IsNonMessage, Is.False);

        Assert.That(schema.Enums.Count, Is.EqualTo(1));

    /*
    MessageSchemaAdapter schemaAdapter = new MessageSchemaAdapter(schema);
    List<MessageAdapter> listMessageAdapters = schemaAdapter.GetMessages();

    Assert.That(listMessageAdapters.Count, Is.EqualTo(7));

    MessageAdapter msgAdapt = listMessageAdapters[0];
    Assert.That(msgAdapt.GetName(), Is.EqualTo("SUnsuspendEGM"));
    Assert.That(msgAdapt.GetPlainName(), Is.EqualTo("UnsuspendEGM"));
    Assert.That(msgAdapt.GetHasNumericDefine(), Is.True);
    Assert.That(msgAdapt.GetDefineId(), Is.EqualTo(102));
    Assert.That(msgAdapt.GetPreprocessorDefineId(), Is.EqualTo("JTP_UnsuspendEGM"));
    Assert.That(msgAdapt.GetDefine().GetName(), Is.EqualTo("JTP_UnsuspendEGM"));
    Assert.That(msgAdapt.GetDefine().GetPlainName(), Is.EqualTo("UnsuspendEGM"));
    Assert.That(msgAdapt.GetSiteIdMember().Count, Is.EqualTo(1));
    Assert.That(msgAdapt.GetSiteIdMember()[0].GetName(), Is.EqualTo("m_usSiteID"));

    msgAdapt = listMessageAdapters[listMessageAdapters.Count - 1];
    Assert.That(msgAdapt.GetName(), Is.EqualTo("SSetJackpotContributionMessageTimer_V2"));
    Assert.That(msgAdapt.GetPlainName(), Is.EqualTo("SetJackpotContributionMessageTimer_V2"));
    Assert.That(msgAdapt.GetDefineId(), Is.EqualTo(89));
    Assert.That(msgAdapt.GetDefine().GetName(), Is.EqualTo("JTP_SetJackpotContributionMessageTimer_V2"));
    Assert.That(msgAdapt.GetDefine().GetPlainName(), Is.EqualTo("SetJackpotContributionMessageTimer_V2"));
    Assert.That(msgAdapt.GetSiteIdMember().Count, Is.EqualTo(2));
    Assert.That(msgAdapt.GetSiteIdMember()[0].GetName(), Is.EqualTo("m_usTimerValue"));
    Assert.That(msgAdapt.GetSiteIdMember()[1].GetName(), Is.EqualTo("m_usJackpotSiteID"));
    */
            
    }
}