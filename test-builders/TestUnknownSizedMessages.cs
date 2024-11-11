using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using CougarMessages.Parser;
using test_builders.asset_source;

namespace test_builders;

[TestFixture]
public class TestUnknownSizedMessages
{
    [Test]
    [TestCaseSource(typeof(UnknownSizedMessageSource), nameof(UnknownSizedMessageSource.TestSourceCode))]
    public void TestVariableStringMessage(string strSource)
    {
        StringReader reader = new StringReader(strSource);
        CougarMessageListener cougarListener = CougarMessageListener.RunGrammar(reader, null);

        IMessageSchema schema = cougarListener.Schema();

        Assert.That(schema.Messages.Count, Is.EqualTo(3));

        IMessage msgUnregister = schema.Messages[0];
        Assert.That(msgUnregister.Name, Is.EqualTo("SPostOfficePrivateUnregister"));
        Assert.That(msgUnregister.Members.Count, Is.EqualTo(5));

        IMember memberData = msgUnregister.Members[4];
        Assert.That(memberData.Name, Is.EqualTo("m_abyData"));
        Assert.That(memberData.IsArray, Is.True);
        ArrayMember arrayMember = (ArrayMember)memberData;
        Assert.That(arrayMember.IsVariableLengthArray, Is.True);
        Assert.That(memberData, Is.InstanceOf<VariableArrayMember>());
        VariableArrayMember memberVarArrayType = (VariableArrayMember)memberData;
        Assert.That(memberVarArrayType.IsUnknownSize, Is.True);

        IMessage msgPersistent = schema.Messages[1];
        Assert.That(msgPersistent.Name, Is.EqualTo("SPostOfficePrivateSetPersistentData"));
        Assert.That(msgPersistent.Members.Count, Is.EqualTo(2));

        IMember memberData2 = msgPersistent.Members[1];
        Assert.That(memberData2.Name, Is.EqualTo("m_byData"));
        Assert.That(memberData.IsArray, Is.True);
        arrayMember = (ArrayMember)memberData;
        Assert.That(arrayMember.IsVariableLengthArray, Is.True);
        Assert.That(memberData2, Is.InstanceOf<VariableArrayMember>());
        Assert.That(((VariableArrayMember)memberData).IsUnknownSize, Is.True);

        IMessage msgUpdatePatronComment = schema.Messages[2];
        Assert.That(msgUpdatePatronComment.Name, Is.EqualTo("SUpdatePatronComment"));
        Assert.That(msgUpdatePatronComment.Members.Count, Is.EqualTo(6));

        IMember memberComment = msgUpdatePatronComment.Members[5];
        Assert.That(memberComment.Name, Is.EqualTo("m_szComment"));
        Assert.That(memberComment.IsArray, Is.True);
        ArrayMember arrayMember2 = (ArrayMember)memberComment;
        Assert.That(arrayMember2.IsVariableLengthArray, Is.True);
        Assert.That(arrayMember2, Is.InstanceOf<VariableArrayMember>());
        Assert.That(((VariableArrayMember)arrayMember2).IsUnknownSize, Is.False);
        Assert.That(((VariableArrayMember)arrayMember2).ArraySizeMember, Is.Not.Null);
        Assert.That(((VariableArrayMember)arrayMember2).ArraySizeMember.Name, Is.EqualTo("m_dwLength"));

        /*
        // Test the associated adapters

        MessageAdapter adaptUnregisterMsg = MessageAdapterFactory.CreateMessageAdapter(msgUnregister);
        Assert.That(adaptUnregisterMsg.HasVariableLengthArrayMember, Is.True);
        MemberAdapter memberAdaptData = adaptUnregisterMsg.Members[4];
        Assert.That(memberAdaptData, Is.Not.Null);
        TypeAdapter typeAdapt = memberAdaptData.TypeAdapter;
        Assert.That(typeAdapt.IsArray, Is.True);
        Assert.That(typeAdapt.IsString, Is.False);
        Assert.That(typeAdapt.CppType, Is.EqualTo("uint8_t"));
        Assert.That(typeAdapt.CppParameterType, Is.EqualTo("const std::vector<uint8_t>&"));
        Assert.That(typeAdapt.ArraySize, Is.EqualTo("JTP_VARIABLE_SIZE_ARRAY"));
        Assert.That(((VariableArrayTypeAdapter)typeAdapt).IsUnknownArraySize, Is.True);

        MessageAdapter adaptPatronComment = MessageAdapterFactory.CreateMessageAdapter(schema.Messages[2]);
        MemberAdapter memberAdaptComment = adaptPatronComment.Members[5];
        typeAdapt = memberAdaptData.TypeAdapter;
        Assert.That(typeAdapt.IsVariableLengthArray, Is.True);
        */

        List<string> listComponents = ((MessageSchema)schema).UniqueComponentList;

        Assert.That(listComponents.Count, Is.EqualTo(3));
        Assert.That(listComponents, Does.Contain("CSSPOPB"));
        Assert.That(listComponents, Does.Contain("POMCLIENT"));
        Assert.That(listComponents, Does.Contain("POM"));
    }
}