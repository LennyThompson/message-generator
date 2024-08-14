using System;
using System.Collections.Generic;
using System.Linq;

namespace CougarMessage.Adapter
{
    public class MemberAdapter
    {
        protected IMember m_memberAdapt;
        private MessageAdapter m_messageTypeAdapter;
        private EnumAdapter m_enumTypeAdapter;
        private MemberMetadata m_metaData;

        public void SetMetaData(MemberMetadata metaData, bool bForce)
        {
            if (m_metaData == null && !bForce)
            {
                if (m_memberAdapt.Type().Equals(metaData.GetTargetType(), StringComparison.OrdinalIgnoreCase))
                {
                    m_metaData = metaData;
                }
            }
            else if (bForce)
            {
                m_metaData = metaData;
            }
        }

        public interface IFromJSONAdapter
        {
            string GetName();
            string GetMemberShortName();
            string GetType();
            string GetInitialiser();
            bool GetHasJsonGetterCast();
            string GetJsonGetterCast();
            bool GetIsMultiLineDeclaration();
            string GetAppend();
            string GetScanfArg();
            string GetPtreeDefault();
            string GetRapidJSONGetter();
            string GetUpdaterFunction();
        }

        public MemberAdapter(IMember memberAdapt, List<MessageAdapter> listMessages)
        {
            m_memberAdapt = memberAdapt;
            if (GetHasMessageType())
            {
                m_messageTypeAdapter = listMessages.FirstOrDefault(message => message.GetName().Equals(m_memberAdapt.Type(), StringComparison.OrdinalIgnoreCase));
            }
        }

        public string GetName()
        {
            return m_memberAdapt.Name;
        }

        public string GetType()
        {
            return m_memberAdapt.Type();
        }

        public bool GetHasMessageType()
        {
            return m_memberAdapt.MessageType() != null;
        }

        public bool GetHasEnumType()
        {
            return m_memberAdapt.EnumType() != null;
        }

        public MessageAdapter GetMessageType()
        {
            if (GetHasMessageType() && m_messageTypeAdapter == null)
            {
                m_messageTypeAdapter = MessageAdapterFactory.CreateMessageAdapter(m_memberAdapt.MessageType());
            }
            return m_messageTypeAdapter;
        }

        public EnumAdapter GetEnumType()
        {
            if (GetHasEnumType() && m_enumTypeAdapter == null)
            {
                m_enumTypeAdapter = new EnumAdapter(m_memberAdapt.EnumType());
            }
            return m_enumTypeAdapter;
        }

        public bool GetHasFieldDescriptionAttribute()
        {
            return m_memberAdapt.Attributes().Count > 0 && m_memberAdapt.Attributes()[0] is IFielddescAttribute;
        }

        public FieldDescriptionAttributeAdapter GetFieldDescriptionAttribute()
        {
            if (GetHasFieldDescriptionAttribute())
            {
                return new FieldDescriptionAttributeAdapter(m_memberAdapt.Attributes()[0]);
            }
            return null;
        }

        public int GetNumericArraySize()
        {
            return m_memberAdapt.NumericArraySize();
        }

        public bool GetHasNumericArraySize()
        {
            if (m_memberAdapt.IsArray())
            {
                return GetNumericArraySize() > 0;
            }
            return false;
        }

        public bool GetIsArray()
        {
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "CHAR":
                    return false;
                default:
                    break;
            }
            return m_memberAdapt.IsArray();
        }

        public bool GetIsDeclaredArray()
        {
            return m_memberAdapt.IsArray();
        }

        public bool GetIsArrayPointer()
        {
            return m_memberAdapt.IsArrayPointer();
        }

        public bool GetIsVariableLengthArray()
        {
            return false;
        }

        public bool GetHasArraySizeDefine()
        {
            return m_memberAdapt.IsArray() && !m_memberAdapt.IsVariableLengthArray() && !string.IsNullOrEmpty(m_memberAdapt.ArraySize());
        }

        MessageAdapter.IDefineAdapter m_defineAdapter = null;

        public MessageAdapter.IDefineAdapter GetArraySizeDefine()
        {
            return m_defineAdapter;
        }

        public MessageAdapter.IDefineAdapter BuildArraySizeDefine(List<IMember> otherMembers)
        {
            if (m_defineAdapter == null && GetHasArraySizeDefine())
            {
                m_defineAdapter = new MessageAdapter.IDefineAdapter()
                {
                    GetName = () => m_memberAdapt.ArraySize(),
                    GetValue = () => m_memberAdapt.NumericArraySize(),
                    GetMemberName = () => m_memberAdapt.StrippedName(),
                    GetPostFix = () => otherMembers.Any(member => member.StrippedName().Equals(m_memberAdapt.StrippedName() + "Length", StringComparison.OrdinalIgnoreCase)) ? "Max" : ""
                };
            }
            return GetArraySizeDefine();
        }

        public string GetCppArrayType()
        {
            if (GetIsVariableLengthArray())
            {
                return "std::vector";
            }
            else if (GetIsArray())
            {
                return "std::array";
            }
            return "";
        }

        public bool GetGenerateArrayMember()
        {
            if (GetIsArray())
            {
                if (!GetHasMessageType() || !GetMessageType().GetIsNonMessage())
                {
                    return true;
                }
            }
            return false;
        }

        public string GetArraySize()
        {
            if (m_memberAdapt.IsArray())
            {
                return m_memberAdapt.ArraySize();
            }
            return "";
        }

        public string GetShortName()
        {
            return m_memberAdapt.ShortName();
        }

        public string GetStrippedName()
        {
            return m_memberAdapt.StrippedName();
        }

        public bool GetIsString()
        {
            if (GetHasEnumType())
            {
                return true;
            }

            switch (m_memberAdapt.Type().ToUpper())
            {
                case "CHAR":
                    return m_memberAdapt.IsArray();
                case "FILETIME":
                    return true;
                default:
                    break;
            }
            return false;
        }

        private bool TreatAsCppString()
        {
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "CHAR":
                    return m_memberAdapt.IsArray();
                default:
                    break;
            }
            return false;
        }

        public bool GetIsParameterString()
        {
            return TreatAsCppString();
        }

        public bool GetIsHashString()
        {
            return TreatAsCppString();
        }

        public bool GetIsFiletime()
        {
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "FILETIME":
                    return true;
                default:
                    break;
            }
            return false;
        }

        public bool GetIsBoolean()
        {
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "BOOL":
                    return true;
                default:
                    break;
            }
            return false;
        }

        public bool GetIsSameType(string strType)
        {
            return m_memberAdapt.Type().ToUpper().Equals(strType, StringComparison.OrdinalIgnoreCase);
        }

        public string GetSingleValueCppType()
        {
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "FLOAT":
                    return "float";
                case "DOUBLE":
                    return "double";
                case "INT":
                    return "int32_t";
                case "LONG":
                    return "long";
                case "ULONG":
                    return "unsigned long";
                case "UINT":
                    return "uint32_t";
                case "USHORT":
                case "WORD":
                case "WCHAR":
                    return "uint16_t";
                case "DWORD":
                    return "uint32_t";
                case "SHORT":
                    return "int16_t";
                case "BOOL":
                    return "bool";
                case "LONGLONG":
                case "__INT64":
                    return "int64_t";
                case "BYTE":
                    return "uint8_t";
                case "CHAR":
                    return "char";
                case "FILETIME":
                    return "std::string";
                case "SGENERALINFO":
                    return "SGeneralInfo";
                default:
                    if (GetHasMessageType())
                    {
                        return GetMessageType().GetName();
                    }
                    else if (GetHasEnumType())
                    {
                        return GetEnumType().GetName();
                    }
                    return "****UNKNOWN";
            }
        }

        public string GetSingleValueDartType()
        {
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "FLOAT":
                case "DOUBLE":
                    return "double";
                case "INT":
                case "LONG":
                case "ULONG":
                case "UINT":
                case "USHORT":
                case "WORD":
                    return "int";
                case "WCHAR":
                    return "int";
                case "DWORD":
                    return "int";
                case "SHORT":
                    return "int";
                case "BOOL":
                    return "bool";
                case "LONGLONG":
                case "__INT64":
                    return "int";
                case "BYTE":
                    return "int";
                case "CHAR":
                    return "int";
                case "FILETIME":
                    return "DateTime";
                case "SGENERALINFO":
                    return "SGeneralInfo";
                default:
                    if (GetHasMessageType())
                    {
                        return GetMessageType().GetName();
                    }
                    else if (GetHasEnumType())
                    {
                        return GetEnumType().GetName();
                    }
                    return "****UNKNOWN";
            }
        }

        public string GetCSharpBinaryConversion()
        {
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "FLOAT":
                    return "ToFloat";
                case "DOUBLE":
                    return "ToDouble";
                case "INT":
                    return "ToInt32";
                case "LONG":
                    return "ToInt32";
                case "ULONG":
                    return "ToUInt32";
                case "UINT":
                    return "ToUInt32";
                case "USHORT":
                case "WORD":
                case "WCHAR":
                    return "ToUInt16";
                case "DWORD":
                    return "ToUInt32";
                case "SHORT":
                    return "ToInt16";
                case "BOOL":
                    return "ToBool";
                case "LONGLONG":
                case "__INT64":
                    return "ToInt64";
                case "BYTE":
                    return "ToByte";
                case "CHAR":
                    return GetIsString() ? "ToString" : "ToChar";
                case "FILETIME":
                    return "ToDateTime";
                default:
                    if (GetHasMessageType())
                    {
                        return GetMessageType().GetPlainName();
                    }
                    else if (GetHasEnumType())
                    {
                        return GetEnumType().GetShortName();
                    }
                    return "****UNKNOWN";
            }
        }

        private string GetCSharpOverrideType()
        {
            if (m_metaData != null && m_metaData.GetIsCSharp() && m_metaData.GetHasTypeUpdate())
            {
                return m_metaData.GetTypeUpdate();
            }
            return "";
        }

        public string GetSingleValueCSharpType()
        {
            string strType = GetCSharpOverrideType();
            if (!string.IsNullOrEmpty(strType))
            {
                return strType;
            }
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "FLOAT":
                    return "float";
                case "DOUBLE":
                    return "double";
                case "INT":
                    return "int";
                case "LONG":
                    return "int";
                case "ULONG":
                    return "uint";
                case "UINT":
                    return "uint";
                case "USHORT":
                case "WORD":
                case "WCHAR":
                    return "ushort";
                case "DWORD":
                    return "uint";
                case "SHORT":
                    return "short";
                case "BOOL":
                    return "bool";
                case "LONGLONG":
                case "__INT64":
                    return "long";
                case "BYTE":
                    return "byte";
                case "CHAR":
                    return "char";
                case "FILETIME":
                    return "DateTime";
                default:
                    if (GetHasMessageType())
                    {
                        return GetMessageType().GetPlainName();
                    }
                    else if (GetHasEnumType())
                    {
                        return GetEnumType().GetName();
                    }
                    return "****UNKNOWN";
            }
        }

        public bool GetHasValueConversion()
        {
            return m_metaData != null && m_metaData.GetHasConversion();
        }

        public string GetValueConversion()
        {
            if (GetHasValueConversion())
            {
                if (m_metaData.GetConversion().Contains("*"))
                {
                    return m_metaData.GetConversion().Replace("*", GetStrippedName());
                }
                return m_metaData.GetConversion();
            }
            return "";
        }

        public string GetBsonTransformFn()
        {
            if (GetIsString())
            {
                return "";
            }

            switch (m_memberAdapt.Type().ToUpper())
            {
                case "FLOAT":
                case "DOUBLE":
                    return "bsoncxx::types::b_double";
                case "INT":
                case "LONG":
                case "ULONG":
                case "UINT":
                case "USHORT":
                case "WORD":
                case "WCHAR":
                case "SHORT":
                case "DWORD":
                case "BYTE":
                case "CHAR":
                    return "bsoncxx::types::b_int32";
                case "BOOL":
                    return "bsoncxx::types::b_bool";
                case "LONGLONG":
                case "__INT64":
                    return "bsoncxx::types::b_int64";
                case "FILETIME":
                    return "bsoncxx::types::b_timestamp";
                default:
                    return "****UNKNOWN";
            }
        }

        public string GetBsonGetter()
        {
            if (GetIsString())
            {
                return "get_utf8().value.to_string()";
            }

            switch (m_memberAdapt.Type().ToUpper())
            {
                case "FLOAT":
                case "DOUBLE":
                    return "get_double()";
                case "INT":
                case "LONG":
                case "ULONG":
                case "UINT":
                case "USHORT":
                case "WORD":
                case "WCHAR":
                case "SHORT":
                case "DWORD":
                case "BYTE":
                case "CHAR":
                    return "get_int32()";
                case "BOOL":
                    return "get_bool()";
                case "LONGLONG":
                case "__INT64":
                    return "get_int64()";
                case "FILETIME":
                    return "get_date()";
                default:
                    return "****UNKNOWN";
            }
        }

        public bool GetHasBsonCast()
        {
            if (GetIsString())
            {
                return false;
            }

            switch (m_memberAdapt.Type().ToUpper())
            {
                case "DOUBLE":
                case "INT":
                    return false;
                case "LONG":
                case "ULONG":
                case "UINT":
                case "USHORT":
                case "WORD":
                case "WCHAR":
                case "SHORT":
                case "DWORD":
                case "BYTE":
                case "CHAR":
                    return true;
                case "BOOL":
                    return false;
                case "LONGLONG":
                case "__INT64":
                    return false;
                case "FILETIME":
                    return false;
                default:
                    return false;
            }
        }

        public string GetBsonCast()
        {
            if (GetIsString())
            {
                return "";
            }

            switch (m_memberAdapt.Type().ToUpper())
            {
                case "DOUBLE":
                case "INT":
                    return "";
                case "LONG":
                case "ULONG":
                case "UINT":
                case "USHORT":
                case "WORD":
                case "WCHAR":
                case "SHORT":
                case "DWORD":
                case "BYTE":
                case "CHAR":
                    return "static_cast<int32_t>";
                case "BOOL":
                    return "";
                case "LONGLONG":
                case "__INT64":
                    return "";
                case "FILETIME":
                    return "";
                default:
                    return "";
            }
        }

        public string GetJSONCast()
        {
            if (GetIsString())
            {
                return "";
            }

            switch (m_memberAdapt.Type().ToUpper())
            {
                case "DOUBLE":
                case "INT":
                case "LONG":
                case "ULONG":
                case "UINT":
                case "USHORT":
                case "WORD":
                case "WCHAR":
                case "SHORT":
                case "DWORD":
                case "BOOL":
                case "LONGLONG":
                case "__INT64":
                    return "";
                case "BYTE":
                case "CHAR":
                    return "(uint32_t) ";
                case "FILETIME":
                    return "";
                default:
                    return "";
            }
        }

        public string GetCppType()
        {
            string strReturn = GetSingleValueCppType();
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "CHAR":
                    strReturn = m_memberAdapt.IsArray() ? "std::string" : strReturn;
                default:
                    break;
            }
            return strReturn;
        }

        public string GetDartType()
        {
            string strReturn = GetSingleValueDartType();
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "CHAR":
                    strReturn = m_memberAdapt.IsArray() ? "String" : strReturn;
                default:
                    break;
            }
            return strReturn;
        }
        
        public IFromJSONAdapter GetFromJSONAdapter()
        {
            return new IFromJSONAdapter
            {
                GetName = () =>
                {
                    switch (m_memberAdapt.Type().ToUpper())
                    {
                        case "FILETIME":
                            return "systime" + GetStrippedName();
                        default:
                            return GetStrippedName();
                    }
                },
                GetMemberShortName = () => GetShortName(),
                GetType = () =>
                {
                    string strReturn = GetSingleValueCppType();
                    if (GetIsArray())
                    {
                        strReturn = GetCppType();
                    }
                    else if (GetHasEnumType())
                    {
                        strReturn = "std::string";
                    }
                    else
                    {
                        if (!GetIsVariableLengthArray())
                        {
                            switch (m_memberAdapt.Type().ToUpper())
                            {
                                case "CHAR":
                                    strReturn = GetIsString() ? "std::string" : "char";
                                    break;
                                case "FILETIME":
                                    return "SYSTEMTIME";
                                default:
                                    break;
                            }
                        }
                    }
                    return strReturn;
                },
                GetInitialiser = () =>
                {
                    string strReturn = "";
                    switch (m_memberAdapt.Type().ToUpper())
                    {
                        case "FILETIME":
                            return "std::memset(&" + GetName() + ", 0, sizeof(" + GetName() + "))";
                        default:
                            break;
                    }
                    return strReturn;
                },
                GetHasJsonGetterCast = () =>
                {
                    switch (m_memberAdapt.Type().ToUpper())
                    {
                        case "SHORT":
                            return true;
                        default:
                            return false;
                    }
                },
                GetJsonGetterCast = () =>
                {
                    switch (m_memberAdapt.Type().ToUpper())
                    {
                        case "SHORT":
                            return "int16_t";
                        default:
                            return "";
                    }
                },
                GetIsMultiLineDeclaration = () =>
                {
                    switch (m_memberAdapt.Type().ToUpper())
                    {
                        case "FILETIME":
                            return true;
                        default:
                            return false;
                    }
                },
                GetAppend = () =>
                {
                    switch (m_memberAdapt.Type().ToUpper())
                    {
                        case "DOUBLE":
                            return "(0.0)";
                        case "INT":
                        case "LONG":
                        case "ULONG":
                        case "UINT":
                        case "USHORT":
                        case "WORD":
                        case "WCHAR":
                        case "SHORT":
                        case "DWORD":
                        case "BYTE":
                        case "BOOL":
                        case "LONGLONG":
                        case "__INT64":
                        case "CHAR":
                            return GetIsString() ? "" : "(0)";
                        case "FILETIME":
                            return "";
                        default:
                            return "";
                    }
                },
                GetScanfArg = () =>
                {
                    if (m_memberAdapt.IsArray())
                    {
                        return GetName();
                    }
                    else if (GetHasMessageType())
                    {
                        return GetName();
                    }
                    else if (GetHasEnumType())
                    {
                        return GetName();
                    }
                    switch (m_memberAdapt.Type().ToUpper())
                    {
                        case "FILETIME":
                            return "&" + GetName() + ".wYear, &" + GetName() + ".wMonth, &" + GetName() + ".wDay, &" + GetName() + ".wHour, &" + GetName() + ".wMinute, &" + GetName() + ".wSecond, &" + GetName() + ".wMilliseconds";
                        default:
                            return "&" + GetName();
                    }
                },
                GetPtreeDefault = () =>
                {
                    if (GetIsString())
                    {
                        return "\"\"";
                    }

                    switch (m_memberAdapt.Type().ToUpper())
                    {
                        case "DOUBLE":
                            return "0.0";
                        case "INT":
                        case "LONG":
                        case "ULONG":
                        case "UINT":
                        case "USHORT":
                        case "WORD":
                        case "WCHAR":
                        case "SHORT":
                        case "DWORD":
                        case "BYTE":
                        case "LONGLONG":
                        case "__INT64":
                        case "CHAR":
                            return "0";
                        case "BOOL":
                        case "FILETIME":
                            return "\"\"";
                        default:
                            return "";
                    }
                },
                GetRapidJSONGetter = () =>
                {
                    if (GetIsString())
                    {
                        return "GetString";
                    }

                    switch (m_memberAdapt.Type().ToUpper())
                    {
                        case "FLOAT":
                            return "GetFloat";
                        case "DOUBLE":
                            return "GetDouble";
                        case "INT":
                        case "LONG":
                        case "ULONG":
                        case "UINT":
                        case "USHORT":
                        case "WORD":
                        case "WCHAR":
                        case "SHORT":
                        case "DWORD":
                        case "BYTE":
                        case "LONGLONG":
                        case "__INT64":
                        case "CHAR":
                            return "GetInt";
                        case "BOOL":
                            return "GetBool";
                        case "FILETIME":
                            return "GetString";
                        default:
                            return "***Unknown***";
                    }
                },
                GetUpdaterFunction = () =>
                {
                    if (!GetIsFiletime() && GetIsString())
                    {
                        return "valueUpdaterString";
                    }

                    switch (m_memberAdapt.Type().ToUpper())
                    {
                        case "FLOAT":
                            return "valueUpdaterFloat";
                        case "DOUBLE":
                            return "valueUpdaterDouble";
                        case "INT":
                            return "valueUpdaterInteger";
                        case "LONG":
                            return "valueUpdaterLong";
                        case "DWORD":
                        case "ULONG":
                            return "valueUpdaterUnsignedLong";
                        case "UINT":
                            return "valueUpdaterUnsignedInteger";
                        case "USHORT":
                        case "WORD":
                        case "WCHAR":
                            return "valueUpdaterUnsignedShortInteger";
                        case "SHORT":
                            return "valueUpdaterShortInteger";
                        case "LONGLONG":
                        case "__INT64":
                            return "valueUpdaterLongInteger";
                        case "BYTE":
                            return "valueUpdaterByte";
                        case "CHAR":
                            return "valueUpdaterChar";
                        case "BOOL":
                            return "valueUpdaterBoolean";
                        case "FILETIME":
                            return "valueUpdaterFiletime";
                        default:
                            return "***Unknown***";
                    }
                }
            };
        }

        public string GetBaseJavaType()
        {
            string strReturn = "**** UNKNOWN TYPE ****";
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "FLOAT":
                    strReturn = "float";
                    break;
                case "DOUBLE":
                    strReturn = "double";
                    break;
                case "INT":
                case "LONG":
                case "ULONG":
                case "UINT":
                case "DWORD":
                    strReturn = "int";
                    break;
                case "SHORT":
                case "USHORT":
                case "WORD":
                case "WCHAR":
                    strReturn = "int";      // Due to restriction in bson deserialisers
                    break;
                case "BOOL":
                    strReturn = "boolean";
                    break;
                case "LONGLONG":
                case "__INT64":
                    strReturn = "long";
                    break;
                case "FILETIME":
                    strReturn = "Date";
                    break;
                case "BYTE":
                    strReturn = "byte";
                    break;
                case "CHAR":
                    strReturn = m_memberAdapt.IsArray() ? "String" : "char";
                    break;
                default:
                    if (GetHasMessageType())
                    {
                        strReturn = GetMessageType().GetJavaClassName();
                    }
                    else if (GetHasEnumType())
                    {
                        strReturn = GetEnumType().Name;
                    }
                    break;
            }
            return strReturn;
        }

        public string GetJavaType()
        {
            string strReturn = GetBaseJavaType();
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "CHAR":
                    return strReturn;
                default:
                    break;
            }

            if (m_memberAdapt.IsArray())
            {
                strReturn += "[]";
            }
            return strReturn;
        }

        public string GetCSharpType()
        {
            string strReturn = GetCSharpOverrideType();
            if (!string.IsNullOrEmpty(strReturn))
            {
                return strReturn;
            }
            strReturn = GetSingleValueCSharpType();
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "CHAR":
                    return (m_memberAdapt.IsArray() || m_memberAdapt.IsVariableLengthArray()) ? "string" : strReturn;
                default:
                    break;
            }

            if (m_memberAdapt.IsArray())
            {
                strReturn += "[]";
            }
            return strReturn;
        }

        public string GetJavaInterfaceType()
        {
            string strReturn = GetBaseJavaType();
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "CHAR":
                    return strReturn;
                default:
                    if (GetHasMessageType())
                    {
                        strReturn = GetMessageType().GetJavaInterfaceName();
                    }
                    break;
            }

            if (m_memberAdapt.IsArray())
            {
                strReturn += "[]";
            }
            return strReturn;
        }

        public string GetJavaTransformFn()
        {
            string strReturn = "";
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "SHORT":
                case "USHORT":
                case "WORD":
                case "WCHAR":
                    strReturn = ".shortValue()";
                    break;
                case "BYTE":
                    strReturn = ".byteValue()";
                case "CHAR":
                    strReturn = m_memberAdapt.IsArray() ? "" : ".byteValue()";
                    break;
                default:
                    break;
            }
            return strReturn;
        }

        public string GetJavaCast()
        {
            string strReturn = "";
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "CHAR":
                    return m_memberAdapt.IsArray() ? "" : "(char) ";
                default:
                    if (GetHasMessageType())
                    {
                        strReturn = "(Document) ";
                    }
                    break;
            }
            if (m_memberAdapt.IsArray())
            {
                strReturn += "(ArrayList<Document>) ";
            }
            return strReturn;
        }

        public string GetJavaInitialiser()
        {
            string strReturn = "**** UNKNOWN TYPE ****";
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "FLOAT":
                case "DOUBLE":
                    strReturn = "0.0";
                    break;
                case "INT":
                case "LONG":
                case "ULONG":
                case "UINT":
                case "DWORD":
                case "SHORT":
                case "USHORT":
                case "WORD":
                case "WCHAR":
                case "BYTE":
                    strReturn = "0";
                    break;
                case "BOOL":
                    strReturn = "false";
                    break;
                case "LONGLONG":
                case "__INT64":
                    strReturn = "0";
                    break;
                case "FILETIME":
                    strReturn = "new Date()";
                    break;
                case "SGENERALINFO":
                    strReturn = " new SGeneralInfo";
                    break;
                case "CHAR":
                    strReturn = m_memberAdapt.IsArray() ? "\"\"" : "0";
                    return strReturn;
                default:
                    if (GetHasMessageType())
                    {
                        strReturn = "new " + GetMessageType().Name;
                    }
                    else if (GetHasEnumType())
                    {
                        strReturn = GetEnumType().Name;
                    }
                    break;
            }
            if (m_memberAdapt.IsArray())
            {
                return "new " + GetBaseJavaType() + "[]";
            }

            return strReturn;
        }

        public string GetJavaBsonGetter()
        {
            string strReturn = "**** UNKNOWN TYPE ****";
            switch (m_memberAdapt.Type().ToUpper())
            {
                case "FLOAT":
                case "DOUBLE":
                    strReturn = "getDouble";
                    break;
                case "INT":
                case "LONG":
                case "ULONG":
                case "UINT":
                case "DWORD":
                case "SHORT":
                case "USHORT":
                case "WORD":
                case "WCHAR":
                case "BYTE":
                    strReturn = "getInteger";
                    break;
                case "BOOL":
                    strReturn = "getBoolean";
                    break;
                case "LONGLONG":
                case "__INT64":
                    strReturn = "getLong";
                    break;
                case "FILETIME":
                    strReturn = "getDate";
                    break;
                case "CHAR":
                    strReturn = m_memberAdapt.IsArray() ? "getString" : "getInteger";
                    return strReturn;
                default:
                    if (GetHasMessageType())
                    {
                        strReturn = "get";
                    }
                    else if (GetHasEnumType())
                    {
                        strReturn = "getString";
                    }
                    break;
            }

            return strReturn;
        }

        public string GetCppFullType()
        {
            string strReturn = GetCppType();
            if (GetIsArray() && !GetIsString())
            {
                strReturn = GetCppArrayType() + "<" + strReturn + ", " + GetArraySize() + ">";
            }
            return strReturn;
        }

        public string GetDartFullType()
        {
            string strReturn = GetDartType();
            if (GetIsArray() && !GetIsString())
            {
                strReturn = "List<" + strReturn + ">";
            }
            return strReturn;
        }

        public string GetCppParameterType()
        {
            string strReturn = GetCppFullType();
            if (GetHasMessageType())
            {
                strReturn = GetMessageType().GetPointerType();
                if (GetIsArray())
                {
                    strReturn = GetCppArrayType() + "<" + strReturn + ", " + GetArraySize() + ">";
                }
            }
            if (GetIsArray() || GetHasMessageType())
            {
                strReturn = "const " + strReturn + "&";
            }
            else
            {
                switch (m_memberAdapt.Type().ToUpper())
                {
                    case "CHAR":
                        if (m_memberAdapt.IsArray())
                        {
                            strReturn = "const " + strReturn + "&";
                        }
                        break;
                    case "FILETIME":
                        strReturn = "const FILETIME&";
                        break;
                    default:
                        break;
                }
            }
            return strReturn;
        }

        public string GetPrintfCode()
        {
            if
            (
                GetHasMessageType()
                ||
                GetHasEnumType()
            )
            {
                return "%s";
            }

            switch (m_memberAdapt.Type().ToUpper())
            {
                case "FLOAT":
                case "DOUBLE":
                    return "%f";
                case "INT":
                case "LONG":
                    return "%d";
                case "ULONG":
                case "UINT":
                    return "%u";
                case "USHORT":
                case "WORD":
                case "WCHAR":
                    return "%hu";
                case "DWORD":
                    return "%u";
                case "BYTE":
                    return "%hhu";
                case "SHORT":
                    return "%hd";
                case "__INT64":
                case "LONGLONG":
                    return "%lld";
                case "BOOL":
                case "CHAR":
                    return "%s";
                case "FILETIME":
                    return "%04d-%02d-%02dT%02d:%02d:%02d.%03dZ";
                default:
                    break;
            }
            return "UNKNOWN";
        }
        public int GetNameLength()
        {
            return GetStrippedName().Length;
        }

        public int GetValueAsStringLength()
        {
            int nLength = 0;
            if (GetHasMessageType())
            {
                nLength = GetMessageType().GetTotalJsonSize();
            }
            else if (GetHasEnumType())
            {
                nLength = GetEnumType().GetMaximumEnumLength();
            }
            else
            {
                switch (m_memberAdapt.Type().ToUpper())
                {
                    case "FLOAT":
                    case "DOUBLE":
                        nLength = 15;
                        break;
                    case "INT":
                    case "DWORD":
                    case "ULONG":
                        nLength = 9;
                        break;
                    case "USHORT":
                    case "WORD":
                    case "SHORT":
                        nLength = 6;
                        break;
                    case "CHAR":
                        nLength = GetIsArray() ? 1 : 3;
                        break;
                    case "BYTE":
                        nLength = 3;
                        break;
                    case "__INT64":
                    case "LONGLONG":
                        nLength = 15;
                        break;
                    case "BOOL":
                        nLength = 7;
                        break;
                    case "FILETIME":
                        nLength = "YYYY-MM-DDThh:mm:ss.mmm".Length;
                        break;
                    default:
                        nLength = 20;
                        break;
                }
            }
            if (GetIsArray())
            {
                nLength *= GetNumericArraySize();
                nLength += GetNumericArraySize() + 2;
            }
            return nLength;
        }

        public string GetOriginalType()
        {
            if (m_memberAdapt.IsArray())
            {
                return $"{m_memberAdapt.Type()}[{m_memberAdapt.ArraySize()}]";
            }
            return m_memberAdapt.Type();
        }

        public int GetOriginalByteSize()
        {
            return m_memberAdapt.OriginalByteSize();
        }

        public void BuildMessageAdapter()
        {
            if (m_memberAdapt.MessageType() != null)
            {
                m_messageTypeAdapter = MessageAdapterFactory.CreateMessageAdapter(m_memberAdapt.MessageType());
            }
        }

        public bool GetHasAdditionalAttribute()
        {
            return m_metaData.GetAdditionalAttr() != null;
        }

        public string GetAdditionalAttribute()
        {
            if (GetHasAdditionalAttribute())
            {
                return $"{m_metaData.GetAdditionalAttr().Name}({string.Join(", ", m_metaData.GetAdditionalAttr().Parameters)})";
            }
            return "*Error no attribute*";
        }

        public bool GetHasMetadata()
        {
            return m_metaData != null;
        }

        public MemberMetadata GetMetadata()
        {
            return m_metaData;
        }
    }
}




