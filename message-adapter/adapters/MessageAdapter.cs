using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Threading;
using Cougar.Utils;
using CougarMessage.Metadata;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Adapter
{
    class MessageInstance
    {
        IMessage m_messageInstance;

        public MessageInstance(IMessage messageInstance)
        {
            Instance = messageInstance;
        }

        public IMessage Instance
        {
            get => m_messageInstance;
            set => m_messageInstance = value;
        }
    }

    public interface IDefineAdapter
    {
        string Name => ;
        int Value => ;
        string MemberName => ;
        string PostFix => ;
    }

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
                m_defineAdapt = DefineAdapterFactory.CreateDefineAdapter(messageAdapt.Define);
            }
            m_listDependentMessages = new List<MessageAdapter>();
        }

        public bool BuildMemberAdapters(List<MessageAdapter> listMessages)
        {
            m_listMembers = m_messageAdapt.Members.Select(member => MemberAdapterFactory.CreateMemberAdapter(member, listMessages)).ToList();
            return m_listMembers.Count > 0;
        }

        public string Name => m_messageAdapt.Name;

        public string UpperName => SnakeCaser.GetSnakeBellyCase(m_messageAdapt.Name).ToUpper();

        public string LowerSnakeName => SnakeCaser.GetSnakeCase(PlainName);

        public List<MemberAdapter> Members => m_listMembers;

        public int MembersCount => m_listMembers.Count;

        public List<MemberAdapter> ArrayMembers => m_listMembers.Where(member => member.IsArray).ToList();

        public List<MemberAdapter> NonArrayMembers => m_listMembers.Where(member => !member.IsArray).ToList();

        public bool HasArrayMember => m_listMembers.Any(member => member.IsArray);

        public bool HasMessageTypeMember => m_listMembers.Any(member => member.HasMessageType);

        public bool UpdateDependentMessages()
        {
            m_listMembers.Where(member => member.HasMessageType).ToList().ForEach(member => member.MessageType.AddDependentMessage(this));
            return true;
        }

        public List<MessageAdapter> DependentMessages => m_listDependentMessages;

        public void AddDependentMessage(MessageAdapter dependentMessage)
        {
            if (!m_listDependentMessages.Any(msg => msg.Name.CompareTo(dependentMessage.Name) == 0))
            {
                m_listDependentMessages.Add(dependentMessage);
            }
        }

        public bool HasDependentMessages => m_listDependentMessages.Count > 0;

        public bool GetDependsOnMessageType(MessageAdapter messageType)
        {
            if (HasMessageTypeMember)
            {
                return m_listMembers.Any(member => member.HasMessageType && member.MessageType.Name.CompareTo(messageType.Name) == 0);
            }
            return false;
        }

        public List<MessageAdapter> MessageTypeDependencies
        {
            get
            {
                if (HasMessageTypeMember)
                {
                    return m_listMembers.Where(member => member.HasMessageType)
                        .Select(member => member.MessageType).ToList();
                }

                return new List<MessageAdapter>();
            }
        }

        public bool HasEnumTypeMember => m_listMembers.Any(member => member.HasEnumType));

        public bool HasMembers =>m_listMembers.Count > 0;

        public bool HasFileTimeMembers => m_listMembers.Any(member => member.IsFiletime);

        public List<MemberAdapter> FileTimeMembers => m_listMembers.Where(member => member.IsFiletime).ToList();

        public string PlainName => m_messageAdapt.BaseName;

        public bool HasDefine =>m_defineAdapt != null;

        public DefineAdapter Define => m_defineAdapt;

        public bool HasNumericDefine => HasDefine  && Define.HasNumericValue;

        int JsonSize => Members.Sum(member => member.NameLength + member.ValueAsStringLength + 3);

        public int TotalJsonSize
        {
            get
            {
                int nReturn = 32;
                int nSize = JsonSize;
                while (nSize >= nReturn)
                {
                    nReturn *= 2;
                }

                return nReturn;
            }
        }

        public int TotalUpdateJsonSize
        {
            get
            {
                int nSize = JsonSize;
                nSize += 130 + MembersCount * 50;
                int nReturn = 32;
                while (nSize >= nReturn)
                {
                    nReturn *= 2;
                }

                return nReturn;
            }
        }

        public string PreprocessorDefineId
        {
            get
            {
                if (HasDefine)
                {
                    return Define.Name;
                }

                return "";
            }
        }

        public int DefineId
        {
            get
            {
                if (HasDefine)
                {
                    if (Define.HasNumericValue)
                    {
                        return Define.NumericValue;
                    }
                }

                return 0;
            }
        }

        public bool HasSiteIdMember => GetHasMember(SITE_ID, SITE_ID_MEMBER);

        public List<MemberAdapter> SiteIdMember => GetMemberByName(SITE_ID, SITE_ID_MEMBER);

        public bool HasCssSiteIdMember => GetHasMember(CSS_SITE_ID, CSS_SITE_ID_MEMBER);

        public List<MemberAdapter> CssSiteIdMember => GetMemberByName(CSS_SITE_ID, CSS_SITE_ID_MEMBER);

        public bool HasCardIdMember => GetHasMember(CARD_ID, CARD_ID_MEMBER);

        public List<MemberAdapter> CardIdMember => GetMemberByName(CARD_ID, CARD_ID_MEMBER);

        private bool GetHasMember(string strDescription, string strName)
        {
            Stack<IMember> stackMembers = new Stack<IMember>();
            return m_messageAdapt.FindTopMostMember(
                    member => member.ShortFieldDescription.Contains(strDescription) || member.Name.CompareTo(strName) == 0 || member.StrippedName.CompareTo(strName) == 0,
                    stackMembers
                );
        }

        private List<MemberAdapter> GetMemberByName(string strDescription, string strName)
        {
            Stack<MemberAdapter> stackMembers = new Stack<MemberAdapter>();
            if (FindTopMostMember(
                    member => member.ShortFieldDescription.Contains(strDescription) || member.Name.CompareTo(strName) == 0 || member.StrippedName.CompareTo(strName) == 0,
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
                else if (member.MessageType != null)
                {
                    if (member.MessageType.FindTopMostMember(memberTest, stackMembers))
                    {
                        return true;
                    }
                }
                stackMembers.Pop();
                return false;
            });
        }

        public bool HasEgmSerialNumberMember
        {
            get
            {
                Stack<IMember> stackMembers = new Stack<IMember>();
                return m_messageAdapt.FindTopMostMember(
                    member => member.Name.Contains("EGMSerialNumber"),
                    stackMembers
                );
            }
        }

        public List<MemberAdapter> EgmSerialNumberMember
        {
            get
            {
                Stack<IMember> stackMembers = new Stack<IMember>();
                m_messageAdapt.FindTopMostMember(
                    member => member.Name.Contains("EGMSerialNumber"),
                    stackMembers
                );
                return stackMembers.Select(member =>
                    m_listMembers.FirstOrDefault(memberAdapter =>
                        memberAdapter.Name.CompareTo(member.Name) == 0)).ToList();
            }
        }

        public bool HasDateMember => m_listMembers.Any(member => member.IsFiletime);

        public bool HasVariableLengthArrayMember => m_listMembers.Any(member => member.IsVariableLengthArray || (member.HasMessageType && member.MessageType.HasVariableLengthArrayMember));
        
        public bool HasVariableLengthArraySizeMember => m_listMembers.Any(member => (member.IsVariableLengthArray && ((VariableArrayMemberAdapter)member).HasSizeMember) || (member.HasMessageType && member.MessageType.HasVariableLengthArraySizeMember));

        public MemberAdapter VariableLengthArrayMember => m_listMembers.FirstOrDefault(member => member.IsVariableLengthArray);

        public MemberAdapter AnyVariableLengthArrayMember
        {
            get
            {
                MemberAdapter memberAdapter = null;
                m_listMembers.ForEach(member =>
                {
                    if (member.IsVariableLengthArray)
                    {
                        memberAdapter = member;
                    }
                    else
                    {
                        if (member.HasMessageType && member.MessageType.HasVariableLengthArrayMember)
                        {
                            memberAdapter = member.MessageType.AnyVariableLengthArrayMember;
                        }
                    }
                });
                return memberAdapter;
            }
        }

        public List<MemberAdapter> VariableLengthArrayMemberPath
        {
            get
            {
                if (HasVariableLengthArrayMember)
                {
                    List<MemberAdapter> listMembers = new List<MemberAdapter>();
                    m_listMembers.ForEach(member =>
                    {
                        if (member.IsVariableLengthArray)
                        {
                            listMembers.Add(member);
                        }
                        else
                        {
                            if (member.HasMessageType && member.MessageType.HasVariableLengthArrayMember)
                            {
                                listMembers.Add(member);
                                listMembers.AddRange(member.MessageType.VariableLengthArrayMemberPath);
                            }
                        }
                    });
                    return listMembers;
                }

                return new List<MemberAdapter>();

            }
        }

        public List<MemberAdapter> VariableLengthArraySizeMemberPath
        {
            get
            {
                if (HasVariableLengthArraySizeMember)
                {
                    List<MemberAdapter> listMembers = new List<MemberAdapter>();
                    m_listMembers.ForEach(member =>
                    {
                        if (member.IsVariableLengthArray && ((VariableArrayMemberAdapter)member).HasSizeMember)
                        {
                            listMembers.Add(((VariableArrayMemberAdapter)member).SizeMember);
                        }
                        else
                        {
                            if (member.HasMessageType && member.MessageType.HasVariableLengthArraySizeMember)
                            {
                                listMembers.Add(member);
                                listMembers.AddRange(member.MessageType.VariableLengthArraySizeMemberPath);
                            }
                        }
                    });
                    return listMembers;
                }

                return new List<MemberAdapter>();
            }
        }

        public bool HasMemberWithVariableLengthArrayMember => m_listMembers.Any(member => member.HasMessageType && member.MessageType.HasVariableLengthArrayMember);

        public MemberAdapter MemberWithVariableLengthArrayMember => m_listMembers.FirstOrDefault(member => member.HasMessageType && member.MessageType.HasVariableLengthArrayMember);

        public List<MessageAdapter> TypeDependencies => m_listMembers.Where(member => member.HasMessageType).Select(member => member.MessageType).DistinctBy(message => message.Name).ToList();

        public List<EnumAdapter> EnumDependencies => m_listMembers.Where(member => member.HasEnumType).Select(member => member.EnumType).DistinctBy(enumAdapter => enumAdapter.Name).ToList();

        public bool HasStrippedNameMemberClash =>  m_messageAdapt.HasStrippedNameMemberClash;

        public int MessageByteSize => m_messageAdapt.MessageByteSize;

        public string PointerType => Name + "Ptr_t";

        public string GenerateDate => DateTime.Now.ToString();

        public string WabFilter => m_messageAdapt.WabFilter;

        // Wab filter adapter behaviour

        public bool IsHOSTFilter => ((IWabFilterAttribute)m_messageAdapt.WabFilterAttribute).IsHost;

        public bool HasHOSTMemberFilter 
        {
            get
            {
                IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.WabFilterAttribute;
                if (wabFilterAttr.IsHost)
                {
                    return wabFilterAttr.HostTarget.MemberPath.Length > 0;
                }

                return false;
            }
        }

        public string HOSTMemberFilter
        {
            get
            {
                IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.WabFilterAttribute;
                if (wabFilterAttr.IsHost && wabFilterAttr.HostTarget.MemberPath.Length > 0)
                {
                    return string.Join(".", wabFilterAttr.HostTarget.MemberPath);
                }

                return "";
            }
        }

        public string DartClassFileName => LowerSnakeName + ".msg" + MessageSchemaAdapter.DART_EXT;
        
        public void SetMetaData(TypeMetaData typeData)
        {
            // Meta data may be targetting every member of a particular name so doesnt apply to the type...
            bool bForceMemberMetaData = false;
            if (typeData.Name.CompareTo("*") != 0)
            {
                m_typeData = typeData;
                bForceMemberMetaData = true;
            }
            foreach (MemberAdapter member in Members)
            {
                MemberMetadata metaData = typeData.MetaMembers.FirstOrDefault(meta => meta.Name.CompareTo(member.StrippedName) == 0 || meta.Name.CompareTo(member.Name) == 0);
                if (metaData == null)
                {
                    metaData = typeData.MetaMembers.FirstOrDefault(meta => meta.Name.CompareTo("*") == 0 && member.GetIsSameType(meta.TargetType));
                }

                if (metaData != null)
                {
                    member.SetMetaData(metaData, bForceMemberMetaData);
                }
            }
        }


        public int HOSTMemberOffset
        {
            get
            {
                IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.WabFilterAttribute;
                if (wabFilterAttr.IsHost && wabFilterAttr.HostTarget.MemberPath.Length > 0)
                {
                    return GetByteOffsetToTargetMember(wabFilterAttr.HostTarget);
                }

                return 0;
            }
        }

        public int HOSTMemberByteSize
        {
            get
            {
                IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.WabFilterAttribute;
                if (wabFilterAttr.IsHost && wabFilterAttr.HostTarget.MemberPath.Length > 0)
                {
                    return GetTargetMember(wabFilterAttr.HostTarget).OriginalByteSize;
                }

                return 0;
            }
        }

        public bool IsSITEFilter => ((IWabFilterAttribute)m_messageAdapt.WabFilterAttribute).IsSite();

        public bool HasSITEMemberFilter
        {
            get
            {
                IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.WabFilterAttribute;
                if (wabFilterAttr.IsSite)
                {
                    return wabFilterAttr.SiteTarget.MemberPath.Length > 0;
                }

                return false;
            }
        }

        public string SITEMemberFilter
        {
            get
            {
                IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.WabFilterAttribute;
                if (wabFilterAttr.IsSite && wabFilterAttr.SiteTarget.MemberPath.Length > 0)
                {
                    return string.Join(".", wabFilterAttr.SiteTarget.MemberPath);
                }

                return "";
            }
        }

        public int SITEMemberOffset
        {
            get
            {
                IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.WabFilterAttribute;
                if (wabFilterAttr.IsSite && wabFilterAttr.SiteTarget.MemberPath.Length > 0)
                {
                    return GetByteOffsetToTargetMember(wabFilterAttr.SiteTarget);
                }

                return 0;
            }
        }

        public int SITEMemberByteSize
        {
            get
            {
                IWabFilterAttribute wabFilterAttr = (IWabFilterAttribute)m_messageAdapt.WabFilterAttribute;
                if (wabFilterAttr.IsSite && wabFilterAttr.SiteTarget.MemberPath.Length > 0)
                {
                    return GetTargetMember(wabFilterAttr.SiteTarget).OriginalByteSize;
                }

                return 0;
            }
        }

        private IMember GetTargetMember(IWabFilterAttribute.IWabFilterTarget target)
        {
            var instance = new MessageInstance(m_messageAdapt);
            var listPath = target.MemberPath
                .Select(path =>
                {
                    var member = instance.Instance.Members
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
                    return instance.Instance.Members
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

        public string Description => m_messageAdapt.PrimaryDescription;

        public string ExtendedDescription => m_messageAdapt.ExtendedDescription;

        public string Category => m_messageAdapt.Category;

        public string Generator => m_messageAdapt.Generator;

        public string Consumer => m_messageAdapt.Consumer;

        public string AlertLevel => m_messageAdapt.AlertLevel;

        public string Reason => m_messageAdapt.Reason;

        public bool IsEmptyMessage => !m_messageAdapt.Members.Any();

        public bool IsNonMessage => m_messageAdapt.IsNonMessage;

        public string JavaClassFileName => JavaClassName + MessageSchemaAdapter.JAVA_EXT;

        public string JavaInterfaceFileName => JavaInterfaceName + MessageSchemaAdapter.JAVA_EXT;

        public string JavaClassName => PlainName + "_Msg";

        public string JavaInterfaceName => "I" + PlainName;

        public List<IDefineAdapter> MemberDefines => 
        {
            return Members => 
                .Where(member => member.HasArraySizeDefine)
                .Select(member => member.BuildArraySizeDefine(m_messageAdapt.Members()))
                .ToList();
        }

        public bool HasGeneratedMessages => 
        {
            return m_messageAdapt.GetGeneratedMessages().Any();
        }

        public List<MessageAdapter> GeneratedMessages => 
        {
            return m_messageAdapt.GetGeneratedMessages()
                .Select(MessageAdapterFactory.CreateMessageAdapter)
                .ToList();
        }

        public bool HasTraceMembers => 
        {
            return m_messageAdapt.TraceMembers().Any();
        }

        public interface ITraceMemberJsonAdapter
        {
            bool? HasExternalKey => ;
            string ExternalKey => ;
            string JsonPath => ;
        }

        public interface IMemberPathAdapter
        {
            MessageAdapter GeneratedMessage => ;
            bool IsSameMessageAsGenerator => ;
            List<ITraceMemberJsonAdapter> TraceMemberJsonPath => ;
        }

        public List<string> TraceMemberValuePaths => 
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
            bool HasExternalKey => ;
            string ExternalKey => ;
            string KeySnippet => ;
        }

        public bool HasExternalKey => 
        {
            return m_messageAdapt.ExternalKey() != null;
        }

        public IExternalKeyAdapter ExternalKey => 
        {
            return new IExternalKeyAdapter()
            {
                public bool HasExternalKey =>  => m_messageAdapt.ExternalKey() != null;

                public string ExternalKey =>  => m_messageAdapt.ExternalKey().Name;

                public string KeySnippet =>  => m_messageAdapt.ExternalKey().GetSnippet();
            };
        }

        public bool HasAdditionalAttribute => 
        {
            return m_typeData.GetAdditionalAttr() != null;
        }

        public string AdditionalAttribute => 
        {
            if (GetHasAdditionalAttribute())
            {
                return m_typeData.GetAdditionalAttr().Name + "(" + string.Join(", ", m_typeData.GetAdditionalAttr().GetParameters()) + ")";
            }
            return "*Error no attribute*";
        }

        public List<IMemberPathAdapter> TraceMemberPaths => 
        {
            if (GetHasGeneratedMessages() && HasTraceMembers => )
            {
                return GeneratedMessages => 
                    .Select(message =>
                    {
                        return new IMemberPathAdapter()
                        {
                            public MessageAdapter GeneratedMessage =>  => message;

                            public bool IsSameMessageAsGenerator =>  => message.m_messageAdapt.Name.CompareTo(m_messageAdapt.Name()) == 0;

                            public List<ITraceMemberJsonAdapter> TraceMemberJsonPath => 
                            {
                                var listAdapters = new List<ITraceMemberJsonAdapter>
                                {
                                    new ITraceMemberJsonAdapter()
                                    {
                                        public bool? HasExternalKey =>  => false;

                                        public string ExternalKey =>  => null;

                                        public string JsonPath => 
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
                                        var strExtKey = m_messageAdapt.ExternalKey() != null ? m_messageAdapt.ExternalKey().Name : "";
                                        return new ITraceMemberJsonAdapter()
                                        {
                                            public bool? HasExternalKey =>  => !string.IsNullOrEmpty(strExtKey);

                                            public string ExternalKey =>  => strExtKey;

                                            public string JsonPath => 
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

