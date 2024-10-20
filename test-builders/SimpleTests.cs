using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using CougarMessages.Parser;
using test_builders.asset_source;

namespace test_builders;

[TestFixture]
public class TestSimpleCougarMessages
{

    [Test]
    [TestCaseSource(typeof(SimpleMessageSource), nameof(SimpleMessageSource.TestSourceCode))]
    public void TestSimpleCougarMessage(string strSimpleMessage)
    {
        StringReader reader = new StringReader(strSimpleMessage);
        CougarMessageListener cougarListener = CougarMessageListener.RunGrammar(reader, null);

        IMessageSchema schema = cougarListener.Schema();

        Assert.IsNotNull(schema);
        Assert.That(schema.Messages.Count(), Is.EqualTo(3));
        IMessage message = schema.Messages[0];
        Assert.That(message.Name, Is.EqualTo("SVariationInformation"));

        Assert.That(message.Members.Count(), Is.EqualTo(3));
        IMember member = message.Members[0];
        Assert.IsNotNull(member);
        Assert.That(member.Name, Is.EqualTo("m_byVariationNumber"));
        Assert.That(member.ShortName, Is.EqualTo("byVariationNumber"));
        Assert.That(member.StrippedName, Is.EqualTo("VariationNumber"));
        
        Assert.That(member.Type, Is.EqualTo("BYTE"));
        Assert.That(member.Attributes?.Count, Is.EqualTo(1));
        Assert.IsTrue(member.Attributes[0] is FielddescAttribute);
        FielddescAttribute fielddescAttr = (FielddescAttribute)(member.Attributes[0]);
        Assert.That(fielddescAttr.FieldName, Is.EqualTo("m_byVariationNumber"));
        Assert.That(fielddescAttr.Values.Count(), Is.EqualTo(2));
        Assert.That(fielddescAttr.Values[0].Count(), Is.EqualTo(2));
        Assert.That(fielddescAttr.Values[0][0], Is.EqualTo("Variation"));
        Assert.That(fielddescAttr.Values[0][1], Is.EqualTo("Number"));
        Assert.That(fielddescAttr.Values[1].Count(), Is.EqualTo(0));

        Assert.IsFalse(message.HasStrippedNameMemberClash);
        Assert.IsFalse(member.IsArray);
        Assert.IsNull(member.MessageType);

        member = message.Members[2];
        Assert.IsNotNull(member);
        Assert.That(member.Name, Is.EqualTo("m_ulReturnToPlayer"));
        Assert.That(member.StrippedName, Is.EqualTo("ReturnToPlayer"));
        Assert.That(member.Type, Is.EqualTo("ULONG"));
        Assert.That(member.Attributes?.Count, Is.EqualTo(1));
        Assert.IsFalse(member.IsArray);
        Assert.IsNull(member.MessageType);

        Assert.That(message.Attributes?.Count, Is.EqualTo(3));

        IAttribute attr = message.Attributes[0];
        Assert.IsNotNull(attr);
        Assert.That(attr.Name, Is.EqualTo("description"));
        Assert.That(attr.Values.Count, Is.EqualTo(2));
        Assert.That(attr.Values[0][0], Is.EqualTo("SVariationInformation"));
        Assert.That(attr.Values[1].Count, Is.EqualTo(2));
        Assert.That(attr.Values[1][0], Is.EqualTo("Variation"));
        Assert.That(attr.Values[1][1], Is.EqualTo("Information"));
        Assert.That(message.PrimaryDescription, Is.EqualTo("Variation Information"));

        attr = message.Attributes[1];
        Assert.IsNotNull(attr);
        Assert.That(attr.Name, Is.EqualTo("category"));
        Assert.That(attr.Values.Count, Is.EqualTo(1));
        Assert.That(attr.Values[0][0], Is.EqualTo("NONMESSAGE"));
        Assert.That(message.Category, Is.EqualTo("NONMESSAGE"));

        attr = message.Attributes[2];
        Assert.IsNotNull(attr);
        Assert.That(attr.Name, Is.EqualTo("reason"));
        Assert.That(attr.Values.Count, Is.EqualTo(0));
        Assert.That(message.Reason, Is.Null);

        message = schema.Messages[1];
        Assert.That(message.Name, Is.EqualTo("SCSSEquationUpdated"));

        Assert.That(message.Members.Count, Is.EqualTo(9));
        member = message.Members[1];
        Assert.That(member.Name, Is.EqualTo("m_llID"));
        Assert.That(member.Attributes?.Count, Is.EqualTo(1));
        Assert.IsTrue(member.Attributes[0] is IFielddescAttribute);
        
        //    char        m_szDescription[JTP_DESCRIPTION_SIZE];        //@fielddesc m_szDescription | Description |

        member = message.Members[5];
        Assert.That(member.Name, Is.EqualTo("m_szDescription"));
        Assert.That(member.StrippedName, Is.EqualTo("Description"));
        Assert.That(member.Type, Is.EqualTo("char"));
        Assert.IsTrue(member.IsArray);
        Assert.IsFalse(member.IsVariableLengthArray);
        Assert.That(member.ArraySize, Is.EqualTo("JTP_DESCRIPTION_SIZE"));
        Assert.That(member.NumericArraySize, Is.EqualTo(30));

        member = message.Members[8];
        Assert.That(member.Name, Is.EqualTo("m_szEnabledFlag"));
        Assert.That(member.StrippedName, Is.EqualTo("EnabledFlag"));
        Assert.That(member.Type, Is.EqualTo("char"));
        Assert.IsTrue(member.IsArray);
        Assert.IsFalse(member.IsVariableLengthArray);
        Assert.That(member.ArraySize, Is.EqualTo("JTP_CSS_ENABLEDFLAG_SIZE"));
        Assert.That(member.NumericArraySize, Is.EqualTo(12));

        message = schema.Messages[2];
        Assert.That(message.Name, Is.EqualTo("SConfigMongoQuery"));

        attr = message.Attributes?[0];
        Assert.IsNotNull(attr);
        Assert.That(attr.Name, Is.EqualTo("category"));
        Assert.That(attr.Values.Count, Is.EqualTo(1));
        Assert.That(attr.Values[0][0], Is.EqualTo("NONMESSAGE"));
        Assert.That(message.Category, Is.EqualTo("NONMESSAGE"));

    }
    
    [Test]
    [TestCaseSource(typeof(ExtendedMessageSource), nameof(ExtendedMessageSource.TestSourceCode))]
    public void TestExtendedCougarMessage(string strExtendedSource)
    {
        using (var reader = new StringReader(strExtendedSource))
        {
            CougarMessageListener cougarListener = CougarMessageListener.RunGrammar(reader, null);
            var schema = cougarListener.Schema();

            Assert.That(schema, Is.Not.Null);
            Assert.That(schema.Messages.Count, Is.EqualTo(3));
            var message = schema.Messages[2];
            Assert.That(message.Name, Is.EqualTo("SUpdatedEGMGameMeters"));

            Assert.That(message.Members.Count, Is.EqualTo(6));
            Assert.That(message.HasStrippedNameMemberClash, Is.False);
            var member = message.Members[0];
            Assert.That(member, Is.Not.Null);
            Assert.That(member.Name, Is.EqualTo("m_usSiteID"));
            Assert.That(member.Type, Is.EqualTo("USHORT"));
            Assert.That(member.ShortFieldDescription, Is.EqualTo("Site ID"));
            Assert.That(member.IsArray, Is.False);

            member = message.Members[1];
            Assert.That(member, Is.Not.Null);
            Assert.That(member.Name, Is.EqualTo("m_dwEGMSerialNumber"));
            Assert.That(member.Type, Is.EqualTo("DWORD"));
            Assert.That(member.ShortFieldDescription, Is.EqualTo("EGM Serial Number"));
            Assert.That(member.IsArray, Is.False);

            member = message.Members[2];
            Assert.That(member, Is.Not.Null);
            Assert.That(member.Name, Is.EqualTo("m_dwGameVersionNumber"));
            Assert.That(member.Type, Is.EqualTo("DWORD"));
            Assert.That(member.ShortFieldDescription, Is.EqualTo("Game Version Number"));
            Assert.That(member.IsArray, Is.False);

            member = message.Members[3];
            Assert.That(member, Is.Not.Null);
            Assert.That(member.Name, Is.EqualTo("m_dwVariation"));
            Assert.That(member.Type, Is.EqualTo("DWORD"));
            Assert.That(member.ShortFieldDescription, Is.EqualTo("Variation"));
            Assert.That(member.IsArray, Is.False);

            member = message.Members[4];
            Assert.That(member, Is.Not.Null);
            Assert.That(member.Name, Is.EqualTo("m_ftTime"));
            Assert.That(member.Type, Is.EqualTo("FILETIME"));
            Assert.That(member.ShortFieldDescription, Is.EqualTo("Time"));
            Assert.That(member.IsArray, Is.False);

            member = message.Members[5];
            Assert.That(member, Is.Not.Null);
            Assert.That(member.Name, Is.EqualTo("m_sVariation"));
            Assert.That(member.Type, Is.EqualTo("SVariationInformation"));
            Assert.That(member.ShortFieldDescription, Is.EqualTo("Variation"));
            Assert.That(member.IsArray, Is.True);
            Assert.That(member.IsVariableLengthArray, Is.False);
            Assert.That(member.ArraySize, Is.EqualTo("JTP_MAX_VARIATIONS"));

            Assert.That(message.Attributes?.Count, Is.EqualTo(7));

            Assert.That(message.PrimaryDescription, Is.EqualTo("Update EGM Game Meters"));
            Assert.That(message.ExtendedDescription, Is.EqualTo("This structure contains all the possible meters that may be returned from an EGM ( or nonsense )"));
            Assert.That(message.Category, Is.EqualTo("EGM"));
            Assert.That(message.Reason, Is.EqualTo("sent from fcc at site to update meters on higher level fcc's"));
            Assert.That(message.Consumer, Is.EqualTo("FCC"));
            Assert.That(message.Generator, Is.EqualTo("FCC"));
            Assert.That(message.WabFilter, Is.EqualTo("Subsite Site Ghost:m_usSiteID"));
            Assert.That(message.AlertLevel, Is.EqualTo("ADVISORY"));
            
            Stack<IMember> stackMembers = new();
            Assert.That
            (
                message.FindTopMostMember
                (
                    member => member.Name.Contains("Turnover"),
                    stackMembers
                ), 
                Is.True
            );

            Assert.That(stackMembers.Count, Is.EqualTo(3));
            IMember memberStack = stackMembers.Pop();
            Assert.That(memberStack.Name, Is.EqualTo("m_sVariation"));
            memberStack = stackMembers.Pop();
            Assert.That(memberStack.Name, Is.EqualTo("m_sMeters"));
            memberStack = stackMembers.Pop();
            Assert.That(memberStack.Name, Is.EqualTo("m_dwTurnover"));

            // Test defines
            
            Assert.That(schema.Defines.Count, Is.EqualTo(10));
            int nDefineIndex = 3;
            var define = schema.Defines[nDefineIndex++];
            Assert.That(define.Name, Is.EqualTo("JTP_ThirdPartyFloorControllerLockRequest"));
            Assert.That(define.NumericValue, Is.EqualTo(1188));

            define = schema.Defines[nDefineIndex++];
            Assert.That(define.Name, Is.EqualTo("JTP_ThirdPartyFloorControllerLockReply"));
            Assert.That(define.NumericValue, Is.EqualTo(1189));

            define = schema.Defines[nDefineIndex++];
            Assert.That(define.Name, Is.EqualTo("JTP_ThirdPartyFloorControllerUnlockRequest"));
            Assert.That(define.NumericValue, Is.EqualTo(1190));

            define = schema.Defines[nDefineIndex++];
            Assert.That(define.Name, Is.EqualTo("JTP_ThirdPartyFloorControllerUnlockReply"));
            Assert.That(define.NumericValue, Is.EqualTo(1191));

            define = schema.Defines[nDefineIndex++];
            Assert.That(define.Name, Is.EqualTo("JTP_ThirdPartyFloorControllerRequestAllLocks"));
            Assert.That(define.NumericValue, Is.EqualTo(1192));

            define = schema.Defines[nDefineIndex++];
            Assert.That(define.Name, Is.EqualTo("JTP_ThirdPartyFloorControllerAllLocksRequested"));
            Assert.That(define.NumericValue, Is.EqualTo(1193));

            define = schema.Defines[nDefineIndex++];
            Assert.That(define.Name, Is.EqualTo("JTP_ThirdPartyFloorControllerLockStateChanged"));
            Assert.That(define.NumericValue, Is.EqualTo(1194));

        }
    }

    [Test]
    [TestCaseSource(typeof(CombinedMessageSource), nameof(CombinedMessageSource.TestSourceCode))]
    public void TestLinkedCougarMessage(string strCombinedSource)
    {
        using (var reader = new StringReader(strCombinedSource))
        {
            CougarMessageListener cougarListener = CougarMessageListener.RunGrammar(reader, null);
            var schema = cougarListener.Schema();

            Assert.That(schema, Is.Not.Null);
            Assert.That(schema.Messages.Count, Is.EqualTo(5));
            var message = schema.Messages[4];
            Assert.That(message.Name, Is.EqualTo("SUpdatedEGMGameMeters"));

            Assert.That(message.Members.Count, Is.EqualTo(6));

            var member = message.Members[5];
            Assert.That(member.Name, Is.EqualTo("m_sVariation"));
            Assert.That(member.MessageType, Is.Not.Null);
            Assert.That(member.MessageType?.Name, Is.EqualTo("SVariationInformation"));
        }
    }

}
