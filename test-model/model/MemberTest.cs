using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Attribute = CougarMessage.Parser.MessageTypes.Attribute;

namespace test_model.model;

[TestFixture]
public class MemberTest
{
    [Test]
    public void TestSimpleMember()
    {
        // 	USHORT		m_usSiteID;								//@fielddesc	m_usSiteID | Site ID | Site ID of where the machine is
        Member member = new Member();
        
        member.Type = "USHORT";
        member.Name = "m_usSiteID";
        Attribute attribute = new();
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_usSiteID";
        attribute.AddValue("Site ID");
        attribute.AddValue("Site ID of where the machine is");
        member.AddAttribute(attribute);
        
        Assert.That(member.Type, Is.EqualTo("USHORT"));
        Assert.That(member.Name, Is.EqualTo("m_usSiteID"));
        Assert.That(member.ShortName, Is.EqualTo("usSiteID"));
        Assert.That(member.StrippedName, Is.EqualTo("SiteID"));
        Assert.That(member.Attributes, Is.Not.Null);
        Assert.That(member.Attributes!.Count, Is.EqualTo(1));
        Assert.That(member.Attributes![0].Type, Is.EqualTo(IAttribute.AttributeType.FieldDesc));
        Assert.That(member.Attributes![0].Name, Is.EqualTo("m_usSiteID"));
        Assert.That(member.Attributes![0].Values.Count(), Is.EqualTo(2));
    }
    
    [Test]
    public void TestArrayStringMember()
    {
        // #define JTP_NAME_SIZE						9
        // 	char		m_szComponent[JTP_NAME_SIZE];	//@fielddesc	m_szComponent | Component name | source component
        Define define = new Define();
        define.Name = "JTP_NAME_SIZE";
        NumericDefine numericDefine = new NumericDefine(define);
        numericDefine.NumericValue = 9;
        List<IDefine> listDefines = new List<IDefine>();
        listDefines.Add(numericDefine);

        Member member = new Member();
        member.Type = "char";
        member.Name = "m_szComponent";
        Attribute attribute = new();
        member.ArraySize = "JTP_NAME_SIZE";
        member.ArraySizeDefine = listDefines.FirstOrDefault(def => def.Name == member.ArraySize);
        member.ArraySizeDefine?.Evaluate(defName => { return listDefines.FirstOrDefault(def => def.Name == defName);});
        
        attribute.Type = IAttribute.AttributeType.FieldDesc;
        attribute.Name = "m_szComponent";
        attribute.AddValue("Somponent name");
        attribute.AddValue("source component");
        member.AddAttribute(attribute);

        Assert.That(member.Name, Is.EqualTo("m_szComponent"));
        Assert.That(member.ShortName, Is.EqualTo("szComponent"));
        Assert.That(member.StrippedName, Is.EqualTo("Component"));
        Assert.That(member.IsArray, Is.True);
        Assert.That(member.NumericArraySize, Is.EqualTo(9));
    }

    [Test]
    public void TestVariableArrayMember()
    {
        //	BYTE m_bLen;
        // BYTE m_strMessage[1];
        
        Member memberSize = new Member();
        memberSize.Type = "BYTE";
        memberSize.Name = "m_bLen";
        
        Member member = new Member();
        member.Type = "BYTE";
        member.Name = "m_strMessage";
        member.ArraySize = "1";
        
        List<IMember> listMembers = new List<IMember>(){ memberSize, member};

        VariableArrayMember variableMember = new VariableArrayMember(member, memberSize);
        
        Assert.That(variableMember.Type, Is.EqualTo("BYTE"));
        Assert.That(variableMember.Name, Is.EqualTo("m_strMessage"));
        Assert.That(variableMember.NumericArraySize, Is.EqualTo(1));
        Assert.That(variableMember.ShortName, Is.EqualTo("strMessage"));
        Assert.That(variableMember.StrippedName, Is.EqualTo("Message"));
        Assert.That(variableMember.IsArray, Is.True);
        Assert.That(variableMember.IsVariableLengthArray, Is.True);
        Assert.That(variableMember.ArraySizeMember, Is.Not.Null);
        Assert.That(variableMember.ArraySizeMember.Name, Is.EqualTo("m_bLen"));
        Assert.That(variableMember.ArraySizeMember.Type, Is.EqualTo("BYTE"));
    }
    
    [Test]
    public void TestVariableArrayMemberExplicit()
    {
        // #define JTP_VARIABLE_SIZE_ARRAY 1
        //	BYTE m_bLen;
        // BYTE m_strMessage[1];
        
        Define define = new Define();
        define.Name = "JTP_VARIABLE_SIZE_ARRAY";
        NumericDefine numericDefine = new NumericDefine(define);
        numericDefine.NumericValue = 1;
        List<IDefine> listDefines = new List<IDefine>();
        listDefines.Add(numericDefine);

        Member memberSize = new Member();
        memberSize.Type = "BYTE";
        memberSize.Name = "m_bLen";
        
        Member member = new Member();
        member.Type = "BYTE";
        member.Name = "m_strMessage";
        member.ArraySize = "JTP_VARIABLE_SIZE_ARRAY";
        member.ArraySizeDefine = listDefines.FirstOrDefault(def => def.Name == member.ArraySize);
        member.ArraySizeDefine?.Evaluate(defName => { return listDefines.FirstOrDefault(def => def.Name == defName);});
        
        List<IMember> listMembers = new List<IMember>(){ memberSize, member};

        VariableArrayMember variableMember = new VariableArrayMember(member, memberSize);
        
        Assert.That(variableMember.Type, Is.EqualTo("BYTE"));
        Assert.That(variableMember.Name, Is.EqualTo("m_strMessage"));
        Assert.That(variableMember.NumericArraySize, Is.EqualTo(1));
        Assert.That(variableMember.ShortName, Is.EqualTo("strMessage"));
        Assert.That(variableMember.StrippedName, Is.EqualTo("Message"));
        Assert.That(variableMember.IsArray, Is.True);
        Assert.That(variableMember.IsVariableLengthArray, Is.True);
        Assert.That(variableMember.ArraySizeMember, Is.Not.Null);
        Assert.That(variableMember.ArraySizeMember.Name, Is.EqualTo("m_bLen"));
        Assert.That(variableMember.ArraySizeMember.Type, Is.EqualTo("BYTE"));
    }

    [Test]
    public void TestVariableArrayMemberDodgy()
    {
        //	DWORD	m_dwLength;					//@fielddesc	m_dwLength | length of sql string
        // char	m_szSQLstring;				//@fielddesc	m_szSQLstring | SQL string |
        
        Member memberSize = new Member();
        memberSize.Type = "DWORD";
        memberSize.Name = "m_dwLength";
        
        Member member = new Member();
        member.Type = "char";
        member.Name = "m_szSQLstring";
        
        List<IMember> listMembers = new List<IMember>(){ memberSize, member};

        VariableArrayMember variableMember = new VariableArrayMember(member, memberSize);
        
        Assert.That(variableMember.Type, Is.EqualTo("char"));
        Assert.That(variableMember.Name, Is.EqualTo("m_szSQLstring"));
        Assert.That(variableMember.ArraySize, Is.Null);
        Assert.That(variableMember.ShortName, Is.EqualTo("szSQLstring"));
        Assert.That(variableMember.StrippedName, Is.EqualTo("SQLstring"));
        Assert.That(variableMember.IsArray, Is.True);
        Assert.That(variableMember.IsVariableLengthArray, Is.True);
        Assert.That(variableMember.ArraySizeMember, Is.Not.Null);
        Assert.That(variableMember.ArraySizeMember.Name, Is.EqualTo("m_dwLength"));
        Assert.That(variableMember.ArraySizeMember.Type, Is.EqualTo("DWORD"));
    }

    [Test]
    public void TestPrefixExtraction()
    {
        Member member = new Member();
        member.Type = "EPatronMembershipStatus";
        member.Name = "m_memberStatus";
        Assert.That(member.Prefix, Is.EqualTo("member"));
        
    }
}