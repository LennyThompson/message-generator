using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes
{
    public class Message : IMessage, IMessage
    {
        private static readonly string[] VARIABLE_ARRAY_NAME = { "Length", "Size", "NumberOf", "Count", "Len" };
        public static readonly string ERROR_ARRAY_SIZE_MEMBER = "***Error no array size member***";
        private string m_strName;
        private int m_nOrdinal;
        private Dictionary<string, IAttribute> m_mapAttributes;
        private List<IMember> m_listMembers;
        private IDefine? m_defineMessage;
        private List<IMessage>? m_listGenerated;
        private List<TraceAssociation>? m_listTraceMembers;
        private TimestampFilter? m_timestampFilter;
        private ExternalKeyGenerator? m_externalKey;

        public Message(int nOrdinal)
        {
            m_mapAttributes = new Dictionary<string, IAttribute>();
            m_listMembers = new List<IMember>();
            m_nOrdinal = nOrdinal;
        }

        public void SetName(string strName)
        {
            m_strName = strName;
        }

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
                m_mapAttributes[attrAdd.Name.ToUpper()] = attrAdd;
            }
        }

        public void SetDefine(IDefine defineMessage)
        {
            m_defineMessage = defineMessage;
        }

        public bool HasVariableLengthArrayMember()
        {
            return Members.LastOrDefault()?.IsVariableLengthArray ?? false;
        }

        public void UpdateVariableLengthArray()
        {
            if (Members.Count > 0)
            {
                IMember memberLast = Members.Last();
                if (HasVariableLengthArrayMember())
                {
                    for (int index = m_listMembers.Count - 2; index >= 0; --index)
                    {
                        IMember member = m_listMembers[index];
                        foreach (string name in VARIABLE_ARRAY_NAME)
                        {
                            if (member.Name.Contains(name))
                            {
                                m_listMembers[m_listMembers.Count - 1] = new VariableArrayMember(memberLast, member);
                                return;
                            }
                        }
                    }
                    IMember memberError = new Member();
                    ((Member) memberError).SetName(ERROR_ARRAY_SIZE_MEMBER);
                    ((Member) memberError).SetType(ERROR_ARRAY_SIZE_MEMBER);
                    m_listMembers[m_listMembers.Count - 1] = new VariableArrayMember(memberLast, memberError);
                }
                else if (!memberLast.IsArray && memberLast.Type().ToUpper() == "CHAR" && m_listMembers.Count > 1)
                {
                    IMember member = m_listMembers[m_listMembers.Count - 2];
                    foreach (string name in VARIABLE_ARRAY_NAME)
                    {
                        if (member.Name.Contains(name))
                        {
                            m_listMembers[m_listMembers.Count - 1] = new VariableArrayMember(memberLast, member);
                            return;
                        }
                    }
                }
            }
        }

        public string Name
        {
            get => m_strName;
        }

        public int Ordinal
        {
            get => m_nOrdinal;
        }

        public List<IAttribute> Attributes
        {
            get => m_mapAttributes.Values.ToList();
        }

        public List<IMember> Members
        {
            get => m_listMembers;
        }

        public IDefine Define
        {
            get => m_defineMessage;
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
                    stackMembers.Push(member);
                    if (memberTest(member))
                    {
                        return true;
                    }
                    else if (member.MessageType != null)
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

        public bool FindAllMembers(Predicate<IMember> memberTest, List<Queue<IMember>> listMembers)
        {
            IMember memberLocal = Members.FirstOrDefault(member => memberTest(member));

            Members
                .Where(member => member.MessageType != null)
                .ToList().ForEach(member =>
            {
                List<Queue<IMember>> listLocalMembers = new List<Queue<IMember>>();
                if (member.MessageType.FindAllMembers(memberTest, listLocalMembers))
                {
                    listLocalMembers.ForEach(stack => stack.Enqueue(member));
                    listMembers.AddRange(listLocalMembers);
                }
            });

            if (memberLocal != null)
            {
                Queue<IMember> memberStack = new Queue<IMember>();
                memberStack.Enqueue(memberLocal);
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

        public static Predicate<T> DistinctByKey<T>(Func<T, object> keyExtractor)
        {
            ConcurrentDictionary<object, bool> seen = new ConcurrentDictionary<object, bool>();
            return t => seen.TryAdd(keyExtractor(t), true);
        }

        public bool HasStrippedNameMemberClash()
        {
            List<IMember> listDistinct = Members.DistinctBy(member => member.StrippedName).ToList();
            return listDistinct.Count != Members.Count;
        }

        public string PrimaryDescription()
        {
            if (HasValidAttribute(IAttribute.AttributeType.Description.ToString(), 0))
            {
                return m_mapAttributes[IAttribute.AttributeType.Description.ToString()].Values()[1].Aggregate((a, b) => a + " " + b);
            }
            return "";
        }

        public string ExtendedDescription()
        {
            if (HasValidAttribute(IAttribute.AttributeType.Description.ToString(), 1))
            {
                return m_mapAttributes[IAttribute.AttributeType.Description.ToString()].Values()[2].Aggregate((a, b) => a + " " + b);
            }
            return "";
        }

        public string Category()
        {
            if (HasValidAttribute(IAttribute.AttributeType.Category.ToString(), 0))
            {
                return m_mapAttributes[IAttribute.AttributeType.Category.ToString()].Values()[0][0];
            }
            return "";
        }

        public string Generator()
        {
            if (HasValidAttribute(IAttribute.AttributeType.Generator.ToString(), 0))
            {
                return m_mapAttributes[IAttribute.AttributeType.Generator.ToString()].Values()[0][0];
            }
            return "";
        }

        public string[] Generators()
        {
            return Generator().Length > 0 ? Generator().Split(new char[] { ',', '/', '-' }) : new string[] { };
        }

        public string Consumer()
        {
            if (HasValidAttribute(IAttribute.AttributeType.Consumer.ToString(), 0))
            {
                return m_mapAttributes[IAttribute.AttributeType.Consumer.ToString()].Values()[0][0];
            }
            return "";
        }

        public string[] Consumers()
        {
            return Consumer().Length > 0 ? Consumer().Split(new char[] { ',', '/', '-' }) : new string[] { };
        }

        public string[] WabFilters()
        {
            if (m_mapAttributes.ContainsKey(IAttribute.AttributeType.WabFilter.ToString()))
            {
                WabFilterAttribute wabFilter = (WabFilterAttribute)m_mapAttributes[IAttribute.AttributeType.WabFilter.ToString()];
                List<string> listFilters = new List<string>();
                if (wabFilter.IsSite())
                {
                    listFilters.Add(wabFilter.SiteTarget.Name);
                }
                if (wabFilter.IsSubSite())
                {
                    listFilters.Add(wabFilter.SubSiteTarget().Name());
                }
                if (wabFilter.IsHost())
                {
                    listFilters.Add(wabFilter.HostTarget().Name());
                }
                if (wabFilter.IsGroupHost())
                {
                    listFilters.Add(wabFilter.GroupHostTarget().Name());
                }
                return listFilters.ToArray();
            }
            return new string[] { };
        }

        public string Alertlevel()
        {
            if (HasValidAttribute(IAttribute.AttributeType.ALERTLEVEL.ToString(), 0))
            {
                return m_mapAttributes[IAttribute.AttributeType.ALERTLEVEL.ToString()].Values()[0][0];
            }
            return "";
        }

        public string Wabfilter()
        {
            if (m_mapAttributes.ContainsKey(IAttribute.AttributeType.WABFILTER.ToString()))
            {
                WabFilterAttribute wabFilter = (WabFilterAttribute)m_mapAttributes[IAttribute.AttributeType.WABFILTER.ToString()];
                return wabFilter.GetFilter();
            }
            return "";
        }

        public IAttribute GetWabFilterAttribute()
        {
            if (m_mapAttributes.ContainsKey(IAttribute.AttributeType.WABFILTER.ToString()))
            {
                return m_mapAttributes[IAttribute.AttributeType.WABFILTER.ToString()];
            }
            return new WabFilterAttribute();
        }

        public string Reason()
        {
            if (HasValidAttribute(IAttribute.AttributeType.REASON.ToString(), 0))
            {
                return m_mapAttributes[IAttribute.AttributeType.REASON.ToString()].Values()[0].Aggregate((a, b) => a + " " + b);
            }
            return "";
        }

        public bool IsNonMessage()
        {
            if (m_defineMessage.Name == null)
            {
                return true;
            }
            return false;
        }

        public int MessageByteSize()
        {
            return m_listMembers.Sum(member => member.OriginalByteSize());
        }

        public void AddConsumer(string name)
        {
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
                    if (!Consumers().Contains(name))
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
            m_listGenerated.AddRange(listMessages);
            m_listGenerated = m_listGenerated.GroupBy(msg => msg.Name()).Select(grp => grp.First()).ToList();
        }

        public List<IMessage> GetGeneratedMessages()
        {
            return m_listGenerated;
        }

        public void AddTraceMember(CougarComponentModel.TraceAssociation traceAssociation, CougarComponentModel.ExternalKeyGenerator externalKey)
        {
            m_externalKey = externalKey;
            m_listTraceMembers.Add(traceAssociation);
        }

        public List<CougarComponentModel.TraceAssociation> TraceMembers()
        {
            return m_listTraceMembers;
        }

        public CougarComponentModel.ExternalKeyGenerator ExternalKey()
        {
            return m_externalKey;
        }

        public void SetTimestampFilter(CougarComponentModel.TimestampFilter timestampFilter)
        {
            m_timestampFilter = timestampFilter;
        }

        public bool GetUseTimestampRange(IMessage messageFor)
        {
            if (m_timestampFilter != null)
            {
                Stack<IMember> stackMembers = new Stack<IMember>();
                if (m_timestampFilter.UseRange && !((Message)messageFor).FindTopMostMember(member => member.StrippedName.CompareTo(m_timestampFilter.SuppressField) == 0, stackMembers))
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasValidAttribute(string strAttrKey, int nIndex)
        {
            return m_mapAttributes.ContainsKey(strAttrKey) && m_mapAttributes[strAttrKey].Values().Count > nIndex;
        }
    }
}

