using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Threading;

namespace CougarMessage.Adapter
{
    public class MessageAdapter
    {
        protected IMessage m_messageAdapt;
        protected DefineAdapter m_defineAdapt;

        protected TypeMetaData m_typeData;
        List<MemberAdapter> m_listMembers;

        List<MessageAdapter> m_listDependentMessages;

        private static string SITE_ID = "Site ID";
        private static string SITE_ID_MEMBER = "m_usSiteID";
        private static string CSS_SITE_ID = "CSS site ID";
        private static string CSS_SITE_ID_MEMBER = "m_usCssSiteID";
        private static string CARD_ID = "CID";
        private static string CARD_ID_MEMBER = "m_dwCardID";
        private static string ACCOUNT_ID = "Card Based Account ID";
        private static string ACCOUNT_ID_MEMBER = "m_llCardBasedAccountID";

        private static string EXTERNAL_KEY = "externalKey";

        public MessageAdapter(IMessage messageAdapt)
        {
            m_messageAdapt = messageAdapt;

            if (messageAdapt.Define.Name != null)
            {
                m_defineAdapt = DefineAdapterFactory.CreateDefineAdapter(messageAdapt.Define());
            }
            m_listDependentMessages = new List<MessageAdapter>();
        }

        public bool BuildMemberAdapters(List<MessageAdapter> listMessages)
        {
            m_listMembers = m_messageAdapt.Members().Select(member => MemberAdapterFactory.CreateMemberAdapter(member, listMessages)).ToList();
            return m_listMembers.Count > 0;
        }

        public string GetName()
        {
            return m_messageAdapt.Name;
        }

        public string GetUpperName()
        {
            return SnakeCaser.GetSnakeBellyCase(m_messageAdapt.Name()).ToUpper();
        }

        public string GetLowerSnakeName()
        {
            return SnakeCaser.GetSnakeCase(GetPlainName());
        }

        public List<MemberAdapter> GetMembers()
        {
            return m_listMembers;
        }

        public int GetMembersCount()
        {
            return m_listMembers.Count;
        }

        public List<MemberAdapter> GetArrayMembers()
        {
            return m_listMembers.Where(member => member.GetIsArray()).ToList();
        }

        public List<MemberAdapter> GetNonArrayMembers()
        {
            return m_listMembers.Where(member => !member.GetIsArray()).ToList();
        }

        public bool GetHasArrayMember()
        {
            return m_listMembers.Any(member => member.GetIsArray());
        }

        public bool GetHasMessageTypeMember()
        {
            return m_listMembers.Any(member => member.GetHasMessageType());
        }

        public bool UpdateDependentMessages()
        {
            m_listMembers.Where(member => member.GetHasMessageType()).ToList().ForEach(member => member.GetMessageType().AddDependentMessage(this));
            return true;
        }

        public List<MessageAdapter> GetDependentMessages()
        {
            return m_listDependentMessages;
        }

        public void AddDependentMessage(MessageAdapter dependentMessage)
        {
            if (!m_listDependentMessages.Any(msg => msg.GetName().CompareTo(dependentMessage.GetName()) == 0))
            {
                m_listDependentMessages.Add(dependentMessage);
            }
        }

        public bool GetHasDependentMessages()
        {
            return m_listDependentMessages.Count > 0;
        }

        public bool GetDependsOnMessageType(MessageAdapter messageType)
        {
            if (GetHasMessageTypeMember())
            {
                return m_listMembers.Any(member => member.GetHasMessageType() && member.GetMessageType().GetName().CompareTo(messageType.GetName()) == 0);
            }
            return false;
        }

        public List<MessageAdapter> GetMessageTypeDependencies()
        {
            if (GetHasMessageTypeMember())
            {
                return m_listMembers.Where(member => member.GetHasMessageType()).Select(member => member.GetMessageType()).ToList();
            }
            return new List<MessageAdapter>();
        }

        public bool GetHasEnumTypeMember()
        {
            return m_listMembers.Any(member => member.GetHasEnumType());
        }

        public bool GetHasMembers()
        {
            return m_listMembers.Count > 0;
        }

        public bool GetHasFileTimeMembers()
        {
            return m_listMembers.Any(member => member.GetIsFiletime());
        }

        public List<MemberAdapter> GetFileTimeMembers()
        {
            return m_listMembers.Where(member => member.GetIsFiletime()).ToList();
        }

        public string GetPlainName()
        {
            return m_messageAdapt.BaseName();
        }

        public bool GetHasDefine()
        {
            return m_defineAdapt != null;
        }

        public DefineAdapter GetDefine()
        {
            return m_defineAdapt;
        }

        public bool GetHasNumericDefine()
        {
            return GetHasDefine() && GetDefine().GetHasNumericValue();
        }

        int JsonSize()
        {
            return GetMembers().Sum(member => member.GetNameLength() + member.GetValueAsStringLength() + 3);
        }

        public int GetTotalJsonSize()
        {
            int nReturn = 32;
            int nSize = JsonSize();
            while (nSize >= nReturn)
            {
                nReturn *= 2;
            }
            return nReturn;
        }

        public int GetTotalUpdateJsonSize()
        {
            int nSize = JsonSize();
            nSize += 130 + GetMembersCount() * 50;
            int nReturn = 32;
            while (nSize >= nReturn)
            {
                nReturn *= 2;
            }
            return nReturn;
        }

        public string GetPreprocessorDefineId()
        {
            if (GetHasDefine())
            {
                return GetDefine().GetName();
            }
            return "";
        }

        public int GetDefineId()
        {
            if (GetHasDefine())
            {
                if (GetDefine().GetHasNumericValue())
                {
                    return GetDefine().GetNumericValue();
                }
            }
            return 0;
        }

        public bool GetHasSiteIdMember()
        {
            return GetHasMember(SITE_ID, SITE_ID_MEMBER);
        }

        public List<MemberAdapter> GetSiteIdMember()
        {
            return GetMemberByName(SITE_ID, SITE_ID_MEMBER);
        }

        public bool GetHasCssSiteIdMember()
        {
            return GetHasMember(CSS_SITE_ID, CSS_SITE_ID_MEMBER);
        }

        public List<MemberAdapter> GetCssSiteIdMember()
        {
            return GetMemberByName(CSS_SITE_ID, CSS_SITE_ID_MEMBER);
        }

        public bool GetHasCardIdMember()
        {
            return GetHasMember(CARD_ID, CARD_ID_MEMBER);
        }

        public List<MemberAdapter> GetCardIdMember()
        {
            return GetMemberByName(CARD_ID, CARD_ID_MEMBER);
        }

        private bool GetHasMember(string strDescription, string strName)
        {
            Stack<IMember> stackMembers = new Stack<IMember>();
            return m_messageAdapt.FindTopMostMember(
                    member => member.ShortFieldDescription().Contains(strDescription) || member.Name.CompareTo(strName) == 0 || member.StrippedName().CompareTo(strName) == 0,
                    stackMembers
                );
        }

        private List<MemberAdapter> GetMemberByName(string strDescription, string strName)
        {
            Stack<MemberAdapter> stackMembers = new Stack<MemberAdapter>();
            if (FindTopMostMember(
                    member => member.m_memberAdapt.ShortFieldDescription().Contains(strDescription) || member.m_memberAdapt.Name.CompareTo(strName) == 0 || member.m_memberAdapt.StrippedName().CompareTo(strName) == 0,
                    stackMembers
                ))
            {
                return new List<MemberAdapter>(stackMembers);
            }
            return new List<MemberAdapter>();
        }

        public bool FindTopMostMember(Predicate<MemberAdapter> memberTest, Stack<MemberAdapter> stackMembers)
        {
            return m_listMembers.Any(member =>
            {
                stackMembers.Push(member);
                if (memberTest(member))
                {
                    return true;
                }
                else if (member.GetMessageType() != null)
                {
                    if (member.GetMessageType().FindTopMostMember(memberTest, stackMembers))
                    {
                        return true;
                    }
                }
                stackMembers.Pop();
                return false;
            });
        }

        public bool GetHasEgmSerialNumberMember()
        {
            Stack<IMember> stackMembers = new Stack<IMember>();
            return m_messageAdapt.FindTopMostMember(
                    member => member.Name.Contains("EGMSerialNumber"),
                    stackMembers
                );
        }

        public List<MemberAdapter> GetEgmSerialNumberMember()
        {
            Stack<IMember> stackMembers = new Stack<IMember>();
            m_messageAdapt.FindTopMostMember(
                    member => member.Name.Contains("EGMSerialNumber"),
                    stackMembers
                );
            return stackMembers.Select(member => m_listMembers.FirstOrDefault(memberAdapter => memberAdapter.GetName().CompareTo(member.Name()) == 0)).ToList();
        }

        public bool GetHasDateMember()
        {
            return m_listMembers.Any(member => member.GetIsFiletime());
        }

        public bool GetHasVariableLengthArrayMember()
        {
            return m_listMembers.Any(member => member.GetIsVariableLengthArray() || (member.GetHasMessageType() && member.GetMessageType().GetHasVariableLengthArrayMember()));
        }

        public bool GetHasVariableLengthArraySizeMember()
        {
            return m_listMembers.Any(member => (member.GetIsVariableLengthArray() && ((VariableArrayMemberAdapter)member).GetHasSizeMember()) || (member.GetHasMessageType() && member.GetMessageType().GetHasVariableLengthArraySizeMember()));
        }

        public MemberAdapter GetVariableLengthArrayMember()
        {
            return m_listMembers.FirstOrDefault(member => member.GetIsVariableLengthArray());
        }

        public MemberAdapter GetAnyVariableLengthArrayMember()
        {
            MemberAdapter memberAdapter = null;
            m_listMembers.ForEach(member =>
            {
                if (member.GetIsVariableLengthArray())
                {
                    memberAdapter = member;
                }
                else
                {
                    if (member.GetHasMessageType() && member.GetMessageType().GetHasVariableLengthArrayMember())
                    {
                        memberAdapter = member.GetMessageType().GetAnyVariableLengthArrayMember();
                    }
                }
            });
            return memberAdapter;
        }

        public List<MemberAdapter> GetVariableLengthArrayMemberPath()
        {
            if (GetHasVariableLengthArrayMember())
            {
                List<MemberAdapter> listMembers = new List<MemberAdapter>();
                m_listMembers.ForEach(member =>
                {
                    if (member.GetIsVariableLengthArray())
                    {
                        listMembers.Add(member);
                    }
                    else
                    {
                        if (member.GetHasMessageType() && member.GetMessageType().GetHasVariableLengthArrayMember())
                        {
                            listMembers.Add(member);
                            listMembers.AddRange(member.GetMessageType().GetVariableLengthArrayMemberPath());
                        }
                    }
                });
                return listMembers;
            }
            return new List<MemberAdapter>();
        }

        public List<MemberAdapter> GetVariableLengthArraySizeMemberPath()
        {
            if (GetHasVariableLengthArraySizeMember())
            {
                List<MemberAdapter> listMembers = new List<MemberAdapter>();
                m_listMembers.ForEach(member =>
                {
                    if (member.GetIsVariableLengthArray() && ((VariableArrayMemberAdapter)member).GetHasSizeMember())
                    {
                        listMembers.Add(((VariableArrayMemberAdapter)member).GetSizeMember());
                    }
                    else
                    {
                        if (member.GetHasMessageType() && member.GetMessageType().GetHasVariableLengthArraySizeMember())
                        {
                            listMembers.Add(member);
                            listMembers.AddRange(member.GetMessageType().GetVariableLengthArraySizeMemberPath());
                        }
                    }
                });
                return listMembers;
            }
            return new List<MemberAdapter>();
        }

        public bool GetHasMemberWithVariableLengthArrayMember()
        {
            return m_listMembers.Any(member => member.GetHasMessageType() && member.GetMessageType().GetHasVariableLengthArrayMember());
        }

        public MemberAdapter GetMemberWithVariableLengthArrayMember()
        {
            return m_listMembers.FirstOrDefault(member => member.GetHasMessageType() && member.GetMessageType().GetHasVariableLengthArrayMember());
        }

        public List<MessageAdapter> GetTypeDependencies()
        {
            return m_listMembers.Where(member => member.GetHasMessageType()).Select(member => member.GetMessageType()).DistinctBy(message => message.GetName()).ToList();
        }

        public List<EnumAdapter> GetEnumDependencies()
        {
            return m_listMembers.Where(member => member.GetHasEnumType()).Select(member => member.GetEnumType()).DistinctBy(enumAdapter => enumAdapter.GetName()).ToList();
        }

        public static Predicate<T> DistinctByKey<T>(Func<T, object> keyExtractor)
        {
            Set<object> seen = ConcurrentDictionary<object, byte>.Keys;
            return t => seen.Add(keyExtractor(t));
        }

        public bool GetHasStrippedNameMemberClash()
        {
            return m_messageAdapt.HasStrippedNameMemberClash();
        }

        public int GetMessageByteSize()
        {
            return m_messageAdapt.MessageByteSize();
        }

        public string GetPointerType()
        {
            return GetName() + "Ptr_t";
        }

        public string GetGenerateDate()
        {
            return DateTime.Now.ToString();
        }

        public string GetWabFilter()
        {
            return m_messageAdapt.Wabfilter();
        }

        // Wab filter adapter behaviour

        public bool GetIsHOSTFilter()
        {
            return ((IWabFilterAttribute)m_messageAdapt.GetWabFilterAttribute()).IsHost();
        }

        public bool GetHasHOSTMemberFilter()
        {
            IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.GetWabFilterAttribute();
            if (wabFilterAttr.IsHost())
            {
                return wabFilterAttr.HostTarget().MemberPath().Length > 0;
            }
            return false;
        }

        public string GetHOSTMemberFilter()
        {
            IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.GetWabFilterAttribute();
            if (wabFilterAttr.IsHost() && wabFilterAttr.HostTarget().MemberPath().Length > 0)
            {
                return string.Join(".", wabFilterAttr.HostTarget().MemberPath());
            }
            return "";
        }

        public string GetDartClassFileName()
        {
            return GetLowerSnakeName() + ".msg" + MessageSchemaAdapter.DART_EXT;
        }

        public void SetMetaData(TypeMetaData typeData)
        {
            // Meta data may be targetting every member of a particular name so doesnt apply to the type...
            bool bForceMemberMetaData = false;
            if (typeData.GetName().CompareTo("*") != 0)
            {
                m_typeData = typeData;
                bForceMemberMetaData = true;
            }
            foreach (MemberAdapter member in GetMembers())
            {
                MemberMetadata metaData = typeData.GetMetaMembers().FirstOrDefault(meta => meta.GetName().CompareTo(member.GetStrippedName()) == 0 || meta.GetName().CompareTo(member.GetName()) == 0);
                if (metaData == null)
                {
                    metaData = typeData.GetMetaMembers().FirstOrDefault(meta => meta.GetName().CompareTo("*") == 0 && member.GetIsSameType(meta.GetTargetType()));
                }

                if (metaData != null)
                {
                    member.SetMetaData(metaData, bForceMemberMetaData);
                }
            }
        }

        class MessageInstance
        {
            IMessage m_messageInstance;

            public MessageInstance(IMessage messageInstance)
            {
                SetInstance(messageInstance);
            }

            public IMessage Instance()
            {
                return m_messageInstance;
            }

            public void SetInstance(IMessage messageInstance)
            {
                m_messageInstance = messageInstance;
            }
        }

        public int GetHOSTMemberOffset()
        {
            IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.GetWabFilterAttribute();
            if (wabFilterAttr.IsHost() && wabFilterAttr.HostTarget().MemberPath().Length > 0)
            {
                return GetByteOffsetToTargetMember(wabFilterAttr.HostTarget());
            }
            return 0;
        }

        public int GetHOSTMemberByteSize()
        {
            IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.GetWabFilterAttribute();
            if (wabFilterAttr.IsHost() && wabFilterAttr.HostTarget().MemberPath().Length > 0)
            {
                return GetTargetMember(wabFilterAttr.HostTarget()).OriginalByteSize();
            }
            return 0;
        }

        public bool GetIsSITEFilter()
        {
            return ((IWabFilterAttribute)m_messageAdapt.GetWabFilterAttribute()).IsSite();
        }

        public bool GetHasSITEMemberFilter()
        {
            IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.GetWabFilterAttribute();
            if (wabFilterAttr.IsSite())
            {
                return wabFilterAttr.SiteTarget().MemberPath().Length > 0;
            }
            return false;
        }

        public string GetSITEMemberFilter()
        {
            IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.GetWabFilterAttribute();
            if (wabFilterAttr.IsSite() && wabFilterAttr.SiteTarget().MemberPath.Length > 0)
            {
                return string.Join(".", wabFilterAttr.SiteTarget().MemberPath);
            }
            return "";
        }

        public int GetSITEMemberOffset()
        {
            IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.GetWabFilterAttribute();
            if (wabFilterAttr.IsSite() && wabFilterAttr.SiteTarget().MemberPath.Length > 0)
            {
                return GetByteOffsetToTargetMember(wabFilterAttr.SiteTarget());
            }
            return 0;
        }

        public int GetSITEMemberByteSize()
        {
            IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.GetWabFilterAttribute();
            if (wabFilterAttr.IsSite() && wabFilterAttr.SiteTarget().MemberPath.Length > 0)
            {
                return GetTargetMember(wabFilterAttr.SiteTarget()).OriginalByteSize();
            }
            return 0;
        }

        private IMember GetTargetMember(IWabFilterAttribute.IWabFilterTarget target)
        {
            var instance = new MessageInstance(m_messageAdapt);
            var listPath = target.MemberPath
                .Select(path =>
                {
                    var member = instance.Instance().Members()
                        .FirstOrDefault(member => 
                        {
                            if (member.Name.CompareTo(path) == 0)
                            {
                                instance.SetInstance(member.MessageType());
                                return true;
                            }
                            return false;
                        });
                    return member;
                })
                .ToList();

            return listPath.Last();
        }

        private int GetByteOffsetToTargetMember(IWabFilterAttribute.IWabFilterTarget target)
        {
            var instance = new MessageInstance(m_messageAdapt);
            return target.MemberPath
                .Select(path =>
                {
                    return instance.Instance().Members()
                        .TakeWhile(member =>
                        {
                            if (member.Name.CompareTo(path) != 0)
                            {
                                return true;
                            }
                            instance.SetInstance(member.MessageType());
                            return false;
                        })
                        .Sum(member => member.OriginalByteSize());
                })
                .Sum();
        }

        public string GetDescription()
        {
            return m_messageAdapt.PrimaryDescription();
        }

        public string GetExtendedDescription()
        {
            return m_messageAdapt.ExtendedDescription();
        }

        public string GetCategory()
        {
            return m_messageAdapt.Category();
        }

        public string GetGenerator()
        {
            return m_messageAdapt.Generator();
        }

        public string GetConsumer()
        {
            return m_messageAdapt.Consumer();
        }

        public string GetAlertLevel()
        {
            return m_messageAdapt.AlertLevel();
        }

        public string GetReason()
        {
            return m_messageAdapt.Reason();
        }

        public bool GetIsEmptyMessage()
        {
            return !m_messageAdapt.Members().Any();
        }

        public bool GetIsNonMessage()
        {
            return m_messageAdapt.IsNonMessage();
        }

        public string GetJavaClassFileName()
        {
            return GetJavaClassName() + MessageSchemaAdapter.JAVA_EXT;
        }

        public string GetJavaInterfaceFileName()
        {
            return GetJavaInterfaceName() + MessageSchemaAdapter.JAVA_EXT;
        }

        public string GetJavaClassName()
        {
            return GetPlainName() + "_Msg";
        }

        public string GetJavaInterfaceName()
        {
            return "I" + GetPlainName();
        }

        public interface IDefineAdapter
        {
            string GetName();
            int GetValue();
            string GetMemberName();
            string GetPostFix();
        }

        public List<IDefineAdapter> GetMemberDefines()
        {
            return GetMembers()
                .Where(member => member.GetHasArraySizeDefine())
                .Select(member => member.BuildArraySizeDefine(m_messageAdapt.Members()))
                .ToList();
        }

        public bool GetHasGeneratedMessages()
        {
            return m_messageAdapt.GetGeneratedMessages().Any();
        }

        public List<MessageAdapter> GetGeneratedMessages()
        {
            return m_messageAdapt.GetGeneratedMessages()
                .Select(MessageAdapterFactory.CreateMessageAdapter)
                .ToList();
        }

        public bool GetHasTraceMembers()
        {
            return m_messageAdapt.TraceMembers().Any();
        }

        public interface ITraceMemberJsonAdapter
        {
            bool? GetHasExternalKey();
            string GetExternalKey();
            string GetJsonPath();
        }

        public interface IMemberPathAdapter
        {
            MessageAdapter GetGeneratedMessage();
            bool GetIsSameMessageAsGenerator();
            List<ITraceMemberJsonAdapter> GetTraceMemberJsonPath();
        }

        public List<string> GetTraceMemberValuePaths()
        {
            return m_messageAdapt.TraceMembers()
                .Select(traceAssoc =>
                {
                    if (traceAssoc.GetSource().CompareTo(EXTERNAL_KEY) == 0)
                    {
                        return EXTERNAL_KEY;
                    }
                    else
                    {
                        if (GetHasMember(traceAssoc.GetSource(), traceAssoc.GetSource()))
                        {
                            return "value." + string.Join(".", GetMemberByName(traceAssoc.GetSource(), traceAssoc.GetSource()).Select(MemberAdapter.GetStrippedName));
                        }
                    }
                    return "";
                })
                .Where(path => !string.IsNullOrEmpty(path))
                .ToList();
        }

        public interface IExternalKeyAdapter
        {
            bool GetHasExternalKey();
            string GetExternalKey();
            string GetKeySnippet();
        }

        public bool GetHasExternalKey()
        {
            return m_messageAdapt.ExternalKey() != null;
        }

        public IExternalKeyAdapter GetExternalKey()
        {
            return new IExternalKeyAdapter()
            {
                public bool GetHasExternalKey() => m_messageAdapt.ExternalKey() != null;

                public string GetExternalKey() => m_messageAdapt.ExternalKey().GetName();

                public string GetKeySnippet() => m_messageAdapt.ExternalKey().GetSnippet();
            };
        }

        public bool GetHasAdditionalAttribute()
        {
            return m_typeData.GetAdditionalAttr() != null;
        }

        public string GetAdditionalAttribute()
        {
            if (GetHasAdditionalAttribute())
            {
                return m_typeData.GetAdditionalAttr().GetName() + "(" + string.Join(", ", m_typeData.GetAdditionalAttr().GetParameters()) + ")";
            }
            return "*Error no attribute*";
        }

        public List<IMemberPathAdapter> GetTraceMemberPaths()
        {
            if (GetHasGeneratedMessages() && GetHasTraceMembers())
            {
                return GetGeneratedMessages()
                    .Select(message =>
                    {
                        return new IMemberPathAdapter()
                        {
                            public MessageAdapter GetGeneratedMessage() => message;

                            public bool GetIsSameMessageAsGenerator() => message.m_messageAdapt.Name.CompareTo(m_messageAdapt.Name()) == 0;

                            public List<ITraceMemberJsonAdapter> GetTraceMemberJsonPath()
                            {
                                var listAdapters = new List<ITraceMemberJsonAdapter>
                                {
                                    new ITraceMemberJsonAdapter()
                                    {
                                        public bool? GetHasExternalKey() => false;

                                        public string GetExternalKey() => null;

                                        public string GetJsonPath()
                                        {
                                            if (m_messageAdapt.GetUseTimestampRange(message.m_messageAdapt))
                                            {
                                                return "\"_timestamp\" : { $gte : timestampFrom, $lt : timestampTo }";
                                            }
                                            else
                                            {
                                                return "\"_timestamp\" : { $gte : timestampFrom }";
                                            }
                                        }
                                    }
                                };

                                listAdapters.AddRange(
                                    m_messageAdapt.TraceMembers()
                                    .Select(memberAssoc =>
                                    {
                                        var strExtKey = m_messageAdapt.ExternalKey() != null ? m_messageAdapt.ExternalKey().GetName() : "";
                                        return new ITraceMemberJsonAdapter()
                                        {
                                            public bool? GetHasExternalKey() => !string.IsNullOrEmpty(strExtKey);

                                            public string GetExternalKey() => strExtKey;

                                            public string GetJsonPath()
                                            {
                                                string strThisPath = "****error****";
                                                if (memberAssoc.GetSource().CompareTo(EXTERNAL_KEY) == 0)
                                                {
                                                    strThisPath = strExtKey;
                                                }
                                                else if (GetHasMember(memberAssoc.GetSource(), memberAssoc.GetSource()))
                                                {
                                                    strThisPath = "value." + string.Join(".", GetMemberByName(memberAssoc.GetSource(), memberAssoc.GetSource()).Select(MemberAdapter.GetStrippedName));
                                                }
                                                string strJsonPath = "";
                                                var strMemberName = memberAssoc.GetDestiniations()
                                                    .FirstOrDefault(name => message.GetHasMember(name, name));
                                                if (!string.IsNullOrEmpty(strMemberName))
                                                {
                                                    strJsonPath = "\"" + string.Join(".", message.GetMemberByName(strMemberName, strMemberName).Select(MemberAdapter.GetStrippedName)) + "\" : " + strThisPath;
                                                }
                                                return strJsonPath;
                                            }
                                        };
                                    })
                                    .Where(path => !string.IsNullOrEmpty(path.GetJsonPath()))
                                    .ToList()
                                );

                                return listAdapters;
                            }
                        };
                    })
                    .ToList();
            }
            else
            {
                return new List<IMemberPathAdapter>();
            }
        }
    }
}

}

