using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using CougarMessages.Parser;
using test_builders.asset_source;

namespace test_builders;

[TestFixture]
public class TestVariableSizedArrays
{
[Test]
[TestCaseSource(typeof(VariableSizedArraysSource), nameof(VariableSizedArraysSource.TestSourceCode))]
public void TestVariableStringMessage(string strSource)
{
    StringReader reader = new StringReader(strSource);
    CougarMessageListener cougarListener = CougarMessageListener.RunGrammar(reader, null);

    IMessageSchema schema = cougarListener.Schema();

    Assert.That(schema.Messages.Count, Is.EqualTo(3));

    IMessage? msgLogEvent = schema.FindMessage("SLogEvent_V2");

    Assert.That(msgLogEvent, Is.Not.Null);
    Assert.That(msgLogEvent.Members.Count, Is.EqualTo(2));
    IMember memberArray = msgLogEvent.Members[1];
    Assert.That(memberArray.Name, Is.EqualTo("m_szSQLstring"));
    Assert.That(memberArray.IsVariableLengthArray, Is.True);
    VariableArrayMember variableArrayMember = memberArray as VariableArrayMember;
    Assert.That(variableArrayMember, Is.Not.Null);
    Assert.That(variableArrayMember.ArraySizeMember, Is.Not.Null);
    Assert.That(variableArrayMember.ArraySizeMember.Name, Is.EqualTo("m_dwLength"));

    msgLogEvent = schema.FindMessage("SLogEvent_V3");
    Assert.That(msgLogEvent, Is.Not.Null);
    Assert.That(msgLogEvent.Members.Count, Is.EqualTo(3));
    memberArray = msgLogEvent.Members[2];
    Assert.That(memberArray.Name, Is.EqualTo("m_szSQLstring"));
    Assert.That(memberArray.IsVariableLengthArray, Is.True);
    variableArrayMember = memberArray as VariableArrayMember;
    Assert.That(variableArrayMember, Is.Not.Null);
    Assert.That(variableArrayMember.ArraySizeMember, Is.Not.Null);
    Assert.That(variableArrayMember.ArraySizeMember.Name, Is.EqualTo("m_dwLength"));

    /*
    var msgAdapter = MessageAdapterFactory.CreateMessageAdapter(msgLogEvent);
    Assert.That(msgAdapter.HasVariableLengthArrayMember, Is.True);
    var memberAdapter = msgAdapter.Members[2];
    var typeAdapt = memberAdapter.TypeAdapter;
    Assert.That(typeAdapt.IsArray, Is.False);
    Assert.That(typeAdapt.IsString, Is.True);
    Assert.That(typeAdapt.IsVariableLengthArray, Is.True);
    var variableTypeApapt = (VariableArrayTypeAdapter)typeAdapt;
    Assert.That(variableTypeApapt.HasSizeMember, Is.True);
    Assert.That(variableTypeApapt.SizeMember.Name, Is.EqualTo("m_dwLength"));

    */
    IMessage? msgUpDateDisplay = schema.FindMessage("SUpdateDisplaySlot");
    Assert.That(msgUpDateDisplay, Is.Not.Null);
    Assert.That(msgUpDateDisplay.Members.Count, Is.EqualTo(5));
    memberArray = msgUpDateDisplay.Members[4];
    Assert.That(memberArray.Name, Is.EqualTo("m_strText"));
    Assert.That(memberArray.IsVariableLengthArray, Is.True);
    variableArrayMember = memberArray as VariableArrayMember;
    Assert.That(variableArrayMember, Is.Not.Null);
    Assert.That(variableArrayMember.ArraySizeMember, Is.Not.Null);
    Assert.That(variableArrayMember.ArraySizeMember.Name, Is.EqualTo("m_wTextLength"));
}

   
}