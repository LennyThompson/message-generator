using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace CougarMessage.Parser.MessageTypes
{
    public class Message : IMessage
    {
        private static readonly string[] VARIABLE_ARRAY_NAME = { "Length", "Size", "NumberOf", "Count", "Len" };
        public static readonly string ERROR_ARRAY_SIZE_MEMBER = "***Error no array size member***";
        private string m_strName;
        private int m_nOrdinal;
        private Dictionary<string, IAttribute> m_mapAttributes;
        private List<IMember> m_listMembers;
        private IDefine m_defineMessage;
        private List<IMessage> m_listGenerated;
        private List<CougarComponentModel.TraceAssociation> m_listTraceMembers;
        private CougarComponentModel.TimestampFilter m_timestampFilter;
        private CougarComponentModel.ExternalKeyGenerator m_externalKey;

        public Message(int nOrdinal)
        {
            m_mapAttributes = new Dictionary<string, IAttribute>();
            m_listMembers = new List<IMember>();
            m_nOrdinal = nOrdinal;
            m_defineMessage = new Define();
            m_listGenerated = new List<IMessage>();
            m_listTraceMembers = new List<CougarComponentModel.TraceAssociation>();
        }

        public void SetName(string strName)
        {
            m_strName = strName;
        }

        public void AddMember(IMember memberAdd)
        {
            IMember memberClash = m_listMembers.FirstOrDefault(member => member.StrippedName().CompareTo(memberAdd.StrippedName()) == 0);
            if (memberClash != null)
            {
                Member.UpdateStrippedNames(memberClash, memberAdd);
            }
            m_listMembers.Add(memberAdd);
        }

        public void AddAttribute(IAttribute attrAdd)
        {
            if (attrAdd.Name() != null && !string.IsNullOrEmpty(attrAdd.Name()))
            {
                m_mapAttributes[attrAdd.Name().ToUpper()] = attrAdd;
            }
        }

        public void SetDefine(IDefine defineMessage)
        {
            m_defineMessage = defineMessage;
        }

        public bool HasVariableLengthArrayMember()
        {
            return m_listMembers.Count > 1 && m_listMembers[m_listMembers.Count - 1].IsVariableLengthArray();
        }

        public void UpdateVariableLengthArray()
        {
            if (m_listMembers.Count > 0)
            {
                IMember memberLast = m_listMembers[m_listMembers.Count - 1];
                if (HasVariableLengthArrayMember())
                {
                    for (int index = m_listMembers.Count - 2; index >= 0; --index)
                    {
                        IMember member = m_listMembers[index];
                        foreach (string name in VARIABLE_ARRAY_NAME)
                        {
                            if (member.Name().Contains(name))
                            {
                                m_listMembers[m_listMembers.Count - 1] = new VariableArrayMember(memberLast, member);
                                return;
                            }
                        }
                    }
                    IMember memberError = new Member();
                    memberError.SetName(ERROR_ARRAY_SIZE_MEMBER);
                    memberError.SetType(ERROR_ARRAY_SIZE_MEMBER);
                    m_listMembers[m_listMembers.Count - 1] = new VariableArrayMember(memberLast, memberError);
                }
                else if (!memberLast.IsArray() && memberLast.Type().ToUpper() == "CHAR" && m_listMembers.Count > 1)
                {
                    IMember member = m_listMembers[m_listMembers.Count - 2];
                    foreach (string name in VARIABLE_ARRAY_NAME)
                    {
                        if (member.Name().Contains(name))
                        {
                            m_listMembers[m_listMembers.Count - 1] = new VariableArrayMember(memberLast, member);
                            return;
                        }
                    }
                }
            }
        }

        public string Name()
        {
            return m_strName;
        }

        public int Ordinal()
        {
            return m_nOrdinal;
        }

        public List<IAttribute> Attributes()
        {
            return m_mapAttributes.Values.ToList();
        }

        public List<IMember> Members()
        {
            return m_listMembers;
        }

        public IDefine Define()
        {
            return m_defineMessage;
        }

        public string BaseName()
        {
            if (Name().StartsWith("S"))
            {
                return Name().Substring(1);
            }
            return Name();
        }

        public bool FindTopMostMember(Predicate<IMember> memberTest, Stack<IMember> stackMembers)
        {
            return Members().Any(member =>
            {
                stackMembers.Push(member);
                if (memberTest(member))
                {
                    return true;
                }
                else if (member.MessageType() != null)
                {
                    if (member.MessageType().FindMember(memberTest, stackMembers))
                    {
                        return true;
                    }
                }
                stackMembers.Pop();
                return false;
            });
        }

        public bool FindMember(Predicate<IMember> memberTest, Stack<IMember> stackMembers)
        {
            if (Members().Any(member =>
            {
                stackMembers.Push(member);
                if (memberTest(member))
                {
                    return true;
                }
                stackMembers.Pop();
                return false;
            }))
            {
                return true;
            }

            return Members().Any(member =>
            {
                stackMembers.Push(member);
                if (member.MessageType() != null)
                {
                    if (member.MessageType().FindMember(memberTest, stackMembers))
                    {
                        return true;
                    }
                }
                stackMembers.Pop();
                return false;
            });
        }

        public bool FindAllMembers(Predicate<IMember> memberTest, List<Deque<IMember>> listMembers)
        {
            IMember memberLocal = Members().FirstOrDefault(member => memberTest(member));

            Members().Where(member => member.MessageType() != null).ToList().ForEach(member =>
            {
                List<Deque<IMember>> listLocalMembers = new List<Deque<IMember>>();
                if (member.MessageType().FindAllMembers(memberTest, listLocalMembers))
                {
                    listLocalMembers.ForEach(stack => stack.Push(member));
                    listMembers.AddRange(listLocalMembers);
                }
            });

            if (memberLocal != null)
            {
                Deque<IMember> memberStack = new Deque<IMember>();
                memberStack.Push(memberLocal);
                listMembers.Add(memberStack);
            }
            return listMembers.Count > 0;
        }

        public bool FindAllMessageMembers(Predicate<IMessage> messageMemberTest, List<Deque<IMember>> listMembers)
        {
            Members().Where(member => member.MessageType() != null).Where(member => messageMemberTest(member.MessageType())).ToList().ForEach(member =>
            {
                List<Deque<IMember>> listLocalMembers = new List<Deque<IMember>>();
                if (member.MessageType().FindAllMessageMembers(messageMemberTest, listLocalMembers))
                {
                    listLocalMembers.ForEach(stack => stack.Push(member));
                    listMembers.AddRange(listLocalMembers);
                }
            });
            return listMembers.Count > 0;
        }

        public static Predicate<T> DistinctByKey<T>(Func<T, object> keyExtractor)
        {
            ConcurrentDictionary<object, bool> seen = new ConcurrentDictionary<object, bool>();
            return t => seen.TryAdd(keyExtractor(t), true);
        }

        public bool HasStrippedNameMemberClash()
        {
            List<IMember> listDistinct = Members().Where(DistinctByKey(member => member.StrippedName())).ToList();
            return listDistinct.Count != Members().Count;
        }

        public string PrimaryDescription()
        {
            if (HasValidAttribute(IAttribute.AttributeType.DESCRIPTION.ToString(), 0))
            {
                return m_mapAttributes[IAttribute.AttributeType.DESCRIPTION.ToString()].Values()[1].Aggregate((a, b) => a + " " + b);
            }
            return "";
        }

        public string ExtendedDescription()
        {
            if (HasValidAttribute(IAttribute.AttributeType.DESCRIPTION.ToString(), 1))
            {
                return m_mapAttributes[IAttribute.AttributeType.DESCRIPTION.ToString()].Values()[2].Aggregate((a, b) => a + " " + b);
            }
            return "";
        }

        public string Category()
        {
            if (HasValidAttribute(IAttribute.AttributeType.CATEGORY.ToString(), 0))
            {
                return m_mapAttributes[IAttribute.AttributeType.CATEGORY.ToString()].Values()[0][0];
            }
            return "";
        }

        public string Generator()
        {
            if (HasValidAttribute(IAttribute.AttributeType.GENERATOR.ToString(), 0))
            {
                return m_mapAttributes[IAttribute.AttributeType.GENERATOR.ToString()].Values()[0][0];
            }
            return "";
        }

        public string[] Generators()
        {
            return Generator().Length > 0 ? Generator().Split(new char[] { ',', '/', '-' }) : new string[] { };
        }

        public string Consumer()
        {
            if (HasValidAttribute(IAttribute.AttributeType.CONSUMER.ToString(), 0))
            {
                return m_mapAttributes[IAttribute.AttributeType.CONSUMER.ToString()].Values()[0][0];
            }
            return "";
        }

        public string[] Consumers()
        {
            return Consumer().Length > 0 ? Consumer().Split(new char[] { ',', '/', '-' }) : new string[] { };
        }

        public string[] WabFilters()
        {
            if (m_mapAttributes.ContainsKey(IAttribute.AttributeType.WABFILTER.ToString()))
            {
                WabFilterAttribute wabFilter = (WabFilterAttribute)m_mapAttributes[IAttribute.AttributeType.WABFILTER.ToString()];
                List<string> listFilters = new List<string>();
                if (wabFilter.IsSite())
                {
                    listFilters.Add(wabFilter.SiteTarget().Name());
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
            if (m_defineMessage.Name() == null)
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
                        WabFilterAttribute wabFilter = m_mapAttributes.ContainsKey(IAttribute.AttributeType.WABFILTER.ToString()) ?
                            (WabFilterAttribute)m_mapAttributes[IAttribute.AttributeType.WABFILTER.ToString()] : new WabFilterAttribute();
                        wabFilter.SetHost(new string[] { });
                        m_mapAttributes[IAttribute.AttributeType.WABFILTER.ToString()] = wabFilter;
                    }
                    break;
                case "SITE_WabFilter":
                    {
                        WabFilterAttribute wabFilter = m_mapAttributes.ContainsKey(IAttribute.AttributeType.WABFILTER.ToString()) ?
                            (WabFilterAttribute)m_mapAttributes[IAttribute.AttributeType.WABFILTER.ToString()] : new WabFilterAttribute();
                        wabFilter.SetSite(new string[] { });
                        m_mapAttributes[IAttribute.AttributeType.WABFILTER.ToString()] = wabFilter;
                    }
                    break;
                default:
                    if (!Consumers().Contains(name))
                    {
                        if (!m_mapAttributes.ContainsKey(IAttribute.AttributeType.CONSUMER.ToString()))
                        {
                            m_mapAttributes[IAttribute.AttributeType.CONSUMER.ToString()] = new ComponentAttribute();
                        }
                        m_mapAttributes[IAttribute.AttributeType.CONSUMER.ToString()].AddValue(name);
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
                if (m_timestampFilter.UseRange() && !((Message)messageFor).FindTopMostMember(member => member.StrippedName().CompareTo(m_timestampFilter.SuppressField()) == 0, stackMembers))
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

