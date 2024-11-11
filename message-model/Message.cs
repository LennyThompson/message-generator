using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes
{
    public class Message(int nOrdinal) : IMessage
    {
        private static readonly string[] VARIABLE_ARRAY_NAME = { "Length", "Size", "NumberOf", "Count", "Len" };
        public static readonly string ERROR_ARRAY_SIZE_MEMBER = "***Error no array size member***";
        private string m_strName = "";
        private Dictionary<string, IAttribute>? m_mapAttributes;
        private List<IMember> m_listMembers = new();
        private IDefine? m_defineMessage;
        private List<IMessage>? m_listDependentMessages;
        private List<IMessage>? m_listGenerated;
        private List<TraceAssociation>? m_listTraceMembers;
        private TimestampFilter? m_timestampFilter;
        private ExternalKeyGenerator? m_externalKey;

        public void AddMember(IMember memberAdd)
        {
            IMember? memberClash = m_listMembers.FirstOrDefault(member => member.StrippedName.CompareTo(memberAdd.StrippedName) == 0);
            if (memberClash != null)
            {
                Member.UpdateStrippedNames(memberClash, memberAdd);
            }
            m_listMembers.Add(memberAdd);
        }

        public void AddAttribute(IAttribute attrAdd)
        {
            if (!string.IsNullOrEmpty(attrAdd.Name))
            {
                if (m_mapAttributes == null)
                {
                    m_mapAttributes = new();
                }
                m_mapAttributes[attrAdd.Name.ToUpper()] = attrAdd;
            }
        }

        public bool HasVariableLengthArrayMember
        {
            get => Members.LastOrDefault(member => member.IsArray && ((ArrayMember) member).IsVariableLengthArray) != null;
        }

        public void UpdateVariableLengthArray()
        {
            if (Members.Count > 0)
            {
                IMember memberLast = Members.Last();
                if (HasVariableLengthArrayMember)
                {
                    for (int index = m_listMembers.Count - 2; index >= 0; --index)
                    {
                        IMember member = m_listMembers[index];
                        foreach (string name in VARIABLE_ARRAY_NAME)
                        {
                            if (member.Name.Contains(name))
                            {
                                ((VariableArrayMember) m_listMembers[^1]).ArraySizeMember = member;
                                return;
                            }
                        }
                    }
                    IMember memberError = new Member();
                    ((Member) memberError).Name = ERROR_ARRAY_SIZE_MEMBER;
                    ((Member) memberError).Type = ERROR_ARRAY_SIZE_MEMBER;
                    ((VariableArrayMember) m_listMembers[^1]).ArraySizeMember = memberError;
                }
                else if (!memberLast.IsArray && memberLast.Type.ToUpper() == "CHAR" && m_listMembers.Count > 1)
                {
                    IMember member = m_listMembers[^2];
                    foreach (string name in VARIABLE_ARRAY_NAME)
                    {
                        if (member.Name.Contains(name))
                        {
                            ((VariableArrayMember) m_listMembers[^1]).ArraySizeMember = member;
                            return;
                        }
                    }
                }
            }
        }

        public string Name
        {
            get => m_strName;
            set => m_strName = value;
        }

        public int Ordinal
        {
            get => nOrdinal;
        }

        public List<IAttribute>? Attributes
        {
            get => m_mapAttributes?.Values.ToList();
            set
            {
                if (value != null)
                {
                    m_mapAttributes = value.ToDictionary(attr => attr.Name, attr => attr);
                }
            }
        }

        public List<IMember> Members
        {
            get => m_listMembers;
        }

        public IDefine? Define
        {
            get => m_defineMessage;
            set => m_defineMessage = value;
        }

        public string BaseName
        {
            get
            {
                if (Name.StartsWith("S"))
                {
                    return Name.Substring(1);
                }

                return Name;
            }
        }

        public bool FindTopMostMember(Predicate<IMember> memberTest, Stack<IMember> stackMembers)
        {
            return Members.Any
            (
                member =>
                {
                    if (memberTest(member))
                    {
                        stackMembers.Push(member);
                        return true;
                    }
                    else if (member.MessageType != null)
                    {
                        if (member.MessageType.FindTopMostMember(memberTest, stackMembers))
                        {
                            stackMembers.Push(member);
                            return true;
                        }
                    }
                    return false;
                }
            );
        }

        public bool FindMember(Predicate<IMember> memberTest, Stack<IMember> stackMembers)
        {
            if 
            (
                Members.Any
                (
                    member =>
                    {
                        stackMembers.Push(member);
                        if (memberTest(member))
                        {
                            return true;
                        }
                        stackMembers.Pop();
                        return false;
                    }
                )
            )
            {
                return true;
            }

            return Members.Any
            (
                member =>
                {
                    stackMembers.Push(member);
                    if (member.MessageType != null)
                    {
                        if (member.MessageType.FindMember(memberTest, stackMembers))
                        {
                            return true;
                        }
                    }
                    stackMembers.Pop();
                    return false;
                }
            );
        }

        public bool FindAllMembers(Predicate<IMember> memberTest, List<Stack<IMember>> listMembers)
        {

            Members
                .Where(member => member.MessageType != null)
                .ToList().ForEach(member =>
            {
                List<Stack<IMember>> listLocalMembers = new List<Stack<IMember>>();
                if (member.MessageType?.FindAllMembers(memberTest, listLocalMembers) ?? false)
                {
                    listLocalMembers.ForEach(stack => stack.Push(member));
                    listMembers.AddRange(listLocalMembers);
                }
            });

            IMember? memberLocal = Members.FirstOrDefault(member => memberTest(member));
            if (memberLocal != null)
            {
                Stack<IMember> memberStack = new Stack<IMember>();
                memberStack.Push(memberLocal);
                listMembers.Add(memberStack);
            }
            return listMembers.Count > 0;
        }

        public bool FindAllMessageMembers(Predicate<IMessage> messageMemberTest, List<Queue<IMember>> listMembers)
        {
            Members
                .Where(member => member.MessageType != null)
                .Where(member => messageMemberTest(member.MessageType!))
                .ToList().ForEach
                (
                    member =>
                    {
                        List<Queue<IMember>> listLocalMembers = new List<Queue<IMember>>();
                        if (member.MessageType!.FindAllMessageMembers(messageMemberTest, listLocalMembers))
                        {
                            listLocalMembers.ForEach(stack => stack.Enqueue(member));
                            listMembers.AddRange(listLocalMembers);
                        }
                    }
                );
            return listMembers.Count > 0;
        }

        public bool HasStrippedNameMemberClash
        {
            get
            {
                List<IMember> listDistinct = Members.DistinctBy(member => member.StrippedName).ToList();
                return listDistinct.Count != Members.Count;
            }
        }

        public string? Description
        {
            get
            {
                string strKey = IAttribute.AttributeType.Description.ToString().ToUpper();
                if (HasValidAttribute(strKey, 0))
                {
                    return m_mapAttributes![strKey].Values[0]
                        .Aggregate((a, b) => a + " " + b);
                }

                return null;
            }
        }

        public string? PrimaryDescription
        {
            get
            {
                string strKey = IAttribute.AttributeType.Description.ToString().ToUpper();
                if (HasValidAttribute(strKey, 1))
                {
                    return m_mapAttributes![strKey].Values[1]
                                .Aggregate((a, b) => a + " " + b);
                }

                return null;
            }
        }

        public string? ExtendedDescription
        {
            get
            {
                string strKey = IAttribute.AttributeType.Description.ToString().ToUpper();
                if (HasValidAttribute(strKey, 2))
                {
                    return m_mapAttributes![strKey].Values[2]
                                .Aggregate((a, b) => a + " " + b);
                }

                return null;
            }
        }

        public string? Category
        {
            get
            {
                string strKey = IAttribute.AttributeType.Category.ToString().ToUpper();
                if (HasValidAttribute(strKey, 0))
                {
                    return m_mapAttributes![strKey].Values[0][0];
                }

                return null;
            }
        }

        public string? Generator
        {
            get
            {
                string strKey = IAttribute.AttributeType.Generator.ToString().ToUpper();
                if (HasValidAttribute(strKey, 0))
                {
                    return m_mapAttributes![strKey].Values[0][0];
                }

                return null;
            }
        }

        public string[]? Generators
        {
            get => Generator?.Length > 0 ? Generator.Split(new char[] { ',', '/', '-' }) : new string[] { };
        }

        public string? Consumer
        {
            get
            {
                string strKey = IAttribute.AttributeType.Consumer.ToString().ToUpper();
                if (HasValidAttribute(strKey, 0))
                {
                    return m_mapAttributes![strKey].Values[0][0];
                }

                return null;
            }
        }

        public string[]? Consumers
        {
            get => Consumer?.Length > 0 ? Consumer.Split(new char[] { ',', '/', '-' }) : new string[] { };
        }

        public string[]? WabFilters
        {
            get
            {
                string strKey = IAttribute.AttributeType.WabFilter.ToString().ToUpper();
                if (m_mapAttributes?.ContainsKey(strKey) ?? false)
                {
                    WabFilterAttribute wabFilter =
                        (WabFilterAttribute)m_mapAttributes[strKey];
                    List<string> listFilters = new List<string>();
                    if (wabFilter.IsSite)
                    {
                        listFilters.Add(wabFilter.SiteTarget?.Name ?? "ERROR");
                    }

                    if (wabFilter.IsSubSite)
                    {
                        listFilters.Add(wabFilter.SubSiteTarget?.Name ?? "ERROR");
                    }

                    if (wabFilter.IsHost)
                    {
                        listFilters.Add(wabFilter.HostTarget?.Name ?? "ERROR");
                    }

                    if (wabFilter.IsGroupHost)
                    {
                        listFilters.Add(wabFilter.GroupHostTarget?.Name ?? "ERROR");
                    }

                    return listFilters.ToArray();
                }

                return null;
            }
        }

        public string? AlertLevel
        {
            get
            {
                string strKey = IAttribute.AttributeType.AlertLevel.ToString().ToUpper();
                if (HasValidAttribute(strKey, 0))
                {
                    return m_mapAttributes![strKey].Values[0][0];
                }

                return null;
            }
        }

        public string? WabFilter
        {
            get
            {
                string strKey = IAttribute.AttributeType.WabFilter.ToString().ToUpper();
                if (m_mapAttributes?.ContainsKey(strKey) ?? false)
                {
                    WabFilterAttribute wabFilter =
                        (WabFilterAttribute)m_mapAttributes[strKey];
                    return wabFilter.GetFilter();
                }

                return null;
            }
        }

        public IAttribute? WabFilterAttribute
        {
            get
            {
                string strKey = IAttribute.AttributeType.WabFilter.ToString().ToUpper();
                if (m_mapAttributes?.ContainsKey(strKey) ?? false)
                {
                    return m_mapAttributes[strKey];
                }

                return null;
            }
        }

        public string? Reason
        {
            get
            {
                string strKey = IAttribute.AttributeType.Reason.ToString().ToUpper();
                if (HasValidAttribute(strKey, 0))
                {
                    return m_mapAttributes![strKey].Values[0]
                                .Aggregate((a, b) => a + " " + b);
                }

                return null;
            }
        }

        public bool IsNonMessage
        {
            get
            {
                if (m_defineMessage == null || m_defineMessage?.Name == null)
                {
                    return true;
                }

                return false;
            }
        }

        public int MessageByteSize
        {
            get => Members.Sum(member => member.OriginalByteSize);
        }

        public void AddConsumer(string name)
        {
            if (m_mapAttributes == null)
            {
                m_mapAttributes = new Dictionary<string, IAttribute>();
            }
            switch (name)
            {
                case "HOST_WabFilter":
                    {
                        WabFilterAttribute wabFilter = m_mapAttributes.ContainsKey(IAttribute.AttributeType.WabFilter.ToString()) ?
                            (WabFilterAttribute)m_mapAttributes[IAttribute.AttributeType.WabFilter.ToString()] : new WabFilterAttribute();
                        wabFilter.SetHost(new string[] { });
                        m_mapAttributes[IAttribute.AttributeType.WabFilter.ToString()] = wabFilter;
                    }
                    break;
                case "SITE_WabFilter":
                    {
                        WabFilterAttribute wabFilter = m_mapAttributes.ContainsKey(IAttribute.AttributeType.WabFilter.ToString()) ?
                            (WabFilterAttribute)m_mapAttributes[IAttribute.AttributeType.WabFilter.ToString()] : new WabFilterAttribute();
                        wabFilter.SetSite(new string[] { });
                        m_mapAttributes[IAttribute.AttributeType.WabFilter.ToString()] = wabFilter;
                    }
                    break;
                default:
                    if (Consumers != null && !Consumers.Contains(name))
                    {
                        if (!m_mapAttributes.ContainsKey(IAttribute.AttributeType.Consumer.ToString()))
                        {
                            m_mapAttributes[IAttribute.AttributeType.Consumer.ToString()] = new ComponentAttribute();
                        }
                        m_mapAttributes[IAttribute.AttributeType.Consumer.ToString()].AddValue(name);
                    }
                    break;
            }
        }

        public void AddGeneratedMessages(List<IMessage> listMessages)
        {
            if (m_listGenerated == null)
            {
                m_listGenerated = new();
            }
            m_listGenerated.AddRange(listMessages);
            m_listGenerated = m_listGenerated.GroupBy(msg => msg.Name).Select(grp => grp.First()).ToList();
        }

        public List<IMessage>? GeneratedMessages
        {
            get => m_listGenerated;
        }

        public void AddTraceMember(TraceAssociation traceAssociation, ExternalKeyGenerator? externalKey)
        {
            m_externalKey = externalKey;
            if (m_listTraceMembers == null)
            {
                m_listTraceMembers = new();
            }
            m_listTraceMembers.Add(traceAssociation);
        }

        public List<TraceAssociation>? TraceMembers => m_listTraceMembers;

        public ExternalKeyGenerator? ExternalKey => m_externalKey;

        public TimestampFilter? TimestampFilter
        {
            get => m_timestampFilter;
            set => m_timestampFilter = value;
        }

        public bool GetUseTimestampRange(IMessage messageFor)
        {
            if (m_timestampFilter != null)
            {
                Stack<IMember> stackMembers = new Stack<IMember>();
                if (m_timestampFilter.UseRange && !((Message)messageFor).FindTopMostMember(member => member.StrippedName?.CompareTo(m_timestampFilter.SuppressIfFieldExists) == 0, stackMembers))
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasValidAttribute(string strAttrKey, int nIndex)
        {
            return (m_mapAttributes?.ContainsKey(strAttrKey) ?? false) && m_mapAttributes[strAttrKey].Values.Count > nIndex ;
        }

        public List<IMessage>? DependentMessages => m_listDependentMessages;
        public bool AddMessageDependency(IMessage messageDepends)
        {
            if (m_listDependentMessages == null)
            {
                m_listDependentMessages = new();
                m_listDependentMessages.Add(messageDepends);
            }
            else if (!m_listDependentMessages.Contains(messageDepends))
            {
                m_listDependentMessages.Add(messageDepends);
                return true;
            }

            return false;
        }

        public bool UpdateMessageDependencies()
        {
            m_listMembers.Where(member => member.MessageType != null).ToList().ForEach(member => member.MessageType!.AddMessageDependency(this));
            return true;

        }
    }
}

