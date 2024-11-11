using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using CougarMessages.Parser;
using test_builders.asset_source;

namespace test_builders;

[TestFixture]
public class TestExtendedMessage
{
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
            ArrayMember arrayMember = (ArrayMember)member;
            Assert.That(arrayMember.IsVariableLengthArray, Is.False);
            Assert.That(arrayMember.ArraySize, Is.EqualTo("JTP_MAX_VARIATIONS"));

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
   
}