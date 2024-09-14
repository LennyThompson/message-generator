using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Attribute = CougarMessage.Parser.MessageTypes.Attribute;

namespace test_model.model;

[TestFixture]
public class MessageTests
{
    [Test]
    public void TestSimpleMessage()
    {
        // #define JTP_NotifyJackpotHit								 95
        // 
        // struct SNotifyJackpotHit
        // {
        // 	//@description	SNotifyJackpotHit | Notify Jackpot Hit |
        // 	//				
        // 	//@category		JACKPOT
        // 	//@generator	JPC
        // 	//@consumer		JPD
        // 	//@alertlevel	ADVISORY
        // 	//@reason		sent by jpc to notify interested parties that a jackpot has been hit
        // 	//@wabfilter	SUBSITE LHOST HOST GHOST
        // 	//
        // 	USHORT		m_usJackpotSiteID;			//@fielddesc	m_usJackpotSiteID | Jackpot Site ID | Jackpot Site ID
        // 	DWORD		m_dwJackpotPoolNumber;		//@fielddesc	m_dwJackpotPoolNumber | Jackpot Pool Number | Translates to the pool_number
        // 	USHORT		m_usFloorControllerSiteID;	//@fielddesc	m_usFloorControllerSiteID | Floor Controller Site ID | The site that the floor controller is on
        // 	char		m_szFloorControllerName[JTP_NAME_SIZE];	//@fielddesc	m_szFloorControllerName | Floor Controller Name | The name of the floor controller
        // 	DWORD		m_dwEGMSerialNumber;
        // 	SEGMLocation m_location;				//@fielddesc	m_location | Location | Location of EGM
        // 	DWORD		m_dwJackpotHitValue;		//@fielddesc	m_dwJackpotHitValue | Jackpot Hit Value | JJackpot Hit Value
        // 	DWORD		m_dwCurrentJackpotValue;	//@fielddesc	m_dwCurrentJackpotValue | Current Jackpot Value | Current Jackpot Value
        // 	FILETIME	m_ftHitTime;				//@fielddesc	m_ftHitTime | Hit Time | Hit Time of the jackpot hit
        // 	char		m_szWinnerDescription[JTP_JACKPOTWINNERDESCRIPTION_SIZE];	//@fielddesc	m_szWinnerDescription | Winner Description | string with text describing the winner, etc for sign displays
        // };
        // 
        
        Define define = new Define();
        define.Name = "JTP_NAME_SIZE";
        NumericDefine numericDefine = new NumericDefine(define);
        numericDefine.NumericValue = 9;
        List<IDefine> listDefines = new List<IDefine>();
        listDefines.Add(numericDefine);
        define = new Define();
        define.Name = "JTP_JACKPOTWINNERDESCRIPTION_SIZE";
        numericDefine = new NumericDefine(define);
        numericDefine.NumericValue = 65;
        listDefines.Add(numericDefine);
        define = new Define();
        define.Name = "JTP_NotifyJackpotHit";
        numericDefine = new NumericDefine(define);
        numericDefine.NumericValue = 95;
        listDefines.Add(numericDefine);

        Message message = new Message(1);

        message.Name = "SNotifyJackpotHit";
        
        Attribute attribute = new Attribute();
        attribute.Name = "description";
        attribute.Type = IAttribute.AttributeType.Description;
        attribute.Values.Add(new List<string>());
        attribute.Values[0].Add("SNotifyJackpotHit");
        attribute.Values.Add(new List<string>());
        attribute.Values[1].Add("Notify");
        attribute.Values[1].Add("Jackpot");
        attribute.Values[1].Add("Hit");
        
        message.AddAttribute(attribute);
        
        attribute = new Attribute();
        attribute.Name = "category";
        attribute.Type = IAttribute.AttributeType.Category;
        attribute.Values.Add(new List<string>());
        attribute.Values[0].Add("JACKPOT");
        
        message.AddAttribute(attribute);

        attribute = new Attribute();
        attribute.Name = "generator";
        attribute.Type = IAttribute.AttributeType.Generator;
        attribute.Values.Add(new List<string>());
        attribute.Values[0].Add("JPC");

        message.AddAttribute(attribute);
        
        attribute = new Attribute();
        attribute.Name = "consumer";
        attribute.Type = IAttribute.AttributeType.Consumer;
        attribute.Values.Add(new List<string>());
        attribute.Values[0].Add("JPD");
        
        message.AddAttribute(attribute);

        attribute = new Attribute();
        attribute.Name = "alertlevel";
        attribute.Type = IAttribute.AttributeType.AlertLevel;
        attribute.Values.Add(new List<string>());
        attribute.Values[0].Add("ADVISORY");
        
        message.AddAttribute(attribute);

        attribute = new Attribute();
        attribute.Name = "reason";
        attribute.Type = IAttribute.AttributeType.Reason;
        attribute.Values.Add(new List<string>());
        attribute.Values[0].Add("sent");
        attribute.Values[0].Add("by");
        attribute.Values[0].Add("jpc");
        attribute.Values[0].Add("to");
        attribute.Values[0].Add("signal");
        attribute.Values[0].Add("jp");
        attribute.Values[0].Add("hit");

        message.AddAttribute(attribute);

        attribute = new Attribute();
        attribute.Name = "wabfilter";
        attribute.Type = IAttribute.AttributeType.WabFilter;
        attribute.Values.Add(new List<string>());
        attribute.Values[0].Add("SUBSITE");
        attribute.Values[0].Add("LHOST");
        attribute.Values[0].Add("HOST");
        attribute.Values[0].Add("GHOST");

        message.AddAttribute(attribute);

        Member member = new Member();
        
        member.Type = "USHORT";
        member.Name = "m_usJackpotSiteID";
        attribute = new();
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_usJackpotSiteID";
        attribute.AddValue("Jackpot Site ID");
        attribute.AddValue("Jackpot Site ID");
        member.AddAttribute(attribute);

        message.AddMember(member);
        
        member = new Member();
        
        member.Type = "DWORD";
        member.Name = "m_dwJackpotPoolNumber";
        attribute = new();
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_dwJackpotPoolNumber";
        attribute.AddValue("Jackpot Pool Number");
        attribute.AddValue("Translates to the pool_number");
        member.AddAttribute(attribute);

        message.AddMember(member);

        member = new Member();
        
        member.Type = "USHORT";
        member.Name = "m_usFloorControllerSiteID";
        attribute = new();
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_usFloorControllerSiteID";
        attribute.AddValue("Floor Controller Site ID");
        attribute.AddValue("The site that the floor controller is on");
        member.AddAttribute(attribute);

        message.AddMember(member);
        
        member = new Member();
        
        member.Type = "char";
        member.Name = "m_szFloorControllerName";
        member.ArraySize = "JTP_NAME_SIZE";
        member.ArraySizeDefine = listDefines.FirstOrDefault(def => def.Name == member.ArraySize);
        member.ArraySizeDefine?.Evaluate(defName => { return listDefines.FirstOrDefault(def => def.Name == defName);});
        attribute = new();
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_szFloorControllerName";
        attribute.AddValue("Floor Controller Name");
        attribute.AddValue("The name of the floor controller");
        member.AddAttribute(attribute);

        message.AddMember(member);

        member = new Member();
        
        member.Type = "DWORD";
        member.Name = "m_dwEGMSerialNumber";

        message.AddMember(member);

        member = new Member();
        
        member.Type = "SEGMLocation";
        member.Name = "m_location";
        attribute = new();
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_location";
        attribute.AddValue("Location");
        attribute.AddValue("SLocation of EGM");
        member.AddAttribute(attribute);

        message.AddMember(member);

        member = new Member();
        
        member.Type = "DWORD";
        member.Name = "m_dwJackpotHitValue";
        attribute = new();
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_dwJackpotHitValue";
        attribute.AddValue("Jackpot Hit Value");
        attribute.AddValue("Jackpot Hit Value");
        member.AddAttribute(attribute);

        message.AddMember(member);

        member = new Member();
        
        member.Type = "DWORD";
        member.Name = "m_dwCurrentJackpotValue";
        attribute = new();
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_dwCurrentJackpotValue";
        attribute.AddValue("Current Jackpot Value");
        attribute.AddValue("Current Jackpot Value");
        member.AddAttribute(attribute);

        message.AddMember(member);

        member = new Member();
        
        member.Type = "FILETIME";
        member.Name = "m_ftHitTime";
        attribute = new();
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_ftHitTime";
        attribute.AddValue("Hit Time");
        attribute.AddValue("Hit Time of the jackpot hit");
        member.AddAttribute(attribute);

        message.AddMember(member);

        member = new Member();
        
        member.Type = "char";
        member.Name = "m_szWinnerDescription";
        member.ArraySize = "JTP_JACKPOTWINNERDESCRIPTION_SIZE";
        member.ArraySizeDefine = listDefines.FirstOrDefault(def => def.Name == member.ArraySize);
        member.ArraySizeDefine?.Evaluate(defName => { return listDefines.FirstOrDefault(def => def.Name == defName);});
        attribute = new();
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_szWinnerDescription";
        attribute.AddValue("Winner Description");
        attribute.AddValue("string with text describing the winner, etc for sign displays");
        member.AddAttribute(attribute);

        message.AddMember(member);

        var defineMessage = listDefines
            .FirstOrDefault(define => define.BaseName != null && define.BaseName.CompareTo(message.BaseName) == 0);
        Assert.That(defineMessage, Is.Not.Null);
        message.Define = defineMessage;
        
        message.UpdateVariableLengthArray();
     
        Assert.That(message.Ordinal, Is.EqualTo(1));
        Assert.That(message.Attributes.Count, Is.EqualTo(7));
        Assert.That(message.Define, Is.Not.Null);
        Assert.That(message.BaseName, Is.EqualTo("NotifyJackpotHit"));
        Assert.That(message.Members.Count, Is.EqualTo(10));
        Assert.That(message.HasVariableLengthArrayMember, Is.False);
        Assert.That(message.HasStrippedNameMemberClash, Is.False);
        
        //Assert.That(message.HasValidAttribute(IAttribute.AttributeType.Description.ToString(), 0), Is.Not.Null);
        Assert.That(message.PrimaryDescription, Is.EqualTo("SNotifyJackpotHit"));
        Assert.That(message.ExtendedDescription, Is.EqualTo("Notify Jackpot Hit"));
    }
}