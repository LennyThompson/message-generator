using System;
using System.Collections.Generic;
using System.Linq;
using adapter_interface;
using CougarMessage.Adapter;
using CougarMessage.Metadata;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Adapter
{
    public interface IFromJSONAdapter
    {
        string Name { get; }
        string MemberShortName { get; }
        string Type { get; }
        string Initialiser { get; }
        bool HasJsonGetterCast { get; }
        string JsonGetterCast { get; }
        bool IsMultiLineDeclaration { get; }
        string Append { get; }
        string ScanfArg { get; }
        string PtreeDefault { get; }
        string RapidJSONGetter { get; }
        string UpdaterFunction { get; }
    }

    public class MemberAdapter : IMemberAdapter
    {
        protected IMember m_memberAdapt;
        private MemberMetadata m_metaData;
        DefineAdapter? m_defineAdapter = null;
        ITypeAdapter m_typeAdapter;


        public void SetMetaData(MemberMetadata metaData, bool bForce)
        {
            if (m_metaData == null && !bForce)
            {
                if (m_memberAdapt.Type.Equals(metaData.TargetType, StringComparison.OrdinalIgnoreCase))
                {
                    m_metaData = metaData;
                }
            }
            else if (bForce)
            {
                m_metaData = metaData;
            }
        }

        public MemberAdapter(IMember memberAdapt, ITypeAdapter typeAdapter)
        {
            m_memberAdapt = memberAdapt;
            m_typeAdapter = typeAdapter;
        }

        public string Name => m_memberAdapt.Name;
        public string Type => m_memberAdapt.Type;

        public bool HasMessageType => m_memberAdapt.MessageType != null;

        public bool HasEnumType => m_memberAdapt.EnumType != null;

        public IMessageAdapter? MessageType
        {
            get
            {
                if (m_typeAdapter.HasMessageType)
                {
                    return m_typeAdapter.MessageAdapter;
                }

                return null;
            }
        }

        public IEnumAdapter? EnumType
        {
            get
            {
                if (m_typeAdapter.HasEnumType)
                {
                    return m_typeAdapter.EnumAdapter;
                }

                return null;
            }
        }

        public bool HasFieldDescriptionAttribute =>
            m_memberAdapt.Attributes?.Count > 0 && m_memberAdapt.Attributes[0] is IFielddescAttribute;

        public IAttributeAdapter? FieldDescriptionAttribute
        {
            get
            {
                if (HasFieldDescriptionAttribute)
                {
                    return new FieldDescriptionAttributeAdapter(m_memberAdapt.Attributes?[0]);
                }

                return null;
            }
        }

        public int NumericArraySize => m_memberAdapt.NumericArraySize;

        public bool HasNumericArraySize
        {
            get
            {
                if (m_memberAdapt.IsArray)
                {
                    return NumericArraySize > 0;
                }

                return false;
            }
        }

        public bool IsArray
        {
            get
            {
                return m_typeAdapter.IsArray;
            }
        }

        public bool IsDeclaredArray => m_memberAdapt.IsArray;

        public bool IsArrayPointer => m_memberAdapt.IsArrayPointer;

        public bool IsVariableLengthArray => m_typeAdapter.IsVariableLengthArray;

        public bool HasArraySizeDefine => m_memberAdapt.IsArray && !m_memberAdapt.IsVariableLengthArray &&
                                          !string.IsNullOrEmpty(m_memberAdapt.ArraySize);

        public DefineAdapter? ArraySizeDefine => m_defineAdapter;

        public DefineAdapter? BuildArraySizeDefine(List<IMember> otherMembers)
        {
            if (m_defineAdapter == null && HasArraySizeDefine)
            {
                m_defineAdapter = DefineAdapterFactory.CreateDefineAdapter(new Define()
                    { Name = m_memberAdapt.ArraySize, Value = m_memberAdapt.ArraySize });
                /*
                GetMemberName = () => m_memberAdapt.StrippedName(),
                GetPostFix = () => otherMembers.Any(member => member.StrippedName().Equals(m_memberAdapt.StrippedName() + "Length", StringComparison.OrdinalIgnoreCase)) ? "Max" : ""
            */
            }

            return ArraySizeDefine;
        }

        public string CppArrayType
        {
            get
            {
                if (IsVariableLengthArray)
                {
                    return "std::vector";
                }
                else if (IsArray)
                {
                    return "std::array";
                }

                return m_typeAdapter.ArrayType;
            }
        }

        public bool GenerateArrayMember
        {
            get
            {
                if (IsArray)
                {
                    if (!HasMessageType || !MessageType.IsNonMessage)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public string ArraySize => m_memberAdapt.IsArray ? m_memberAdapt.ArraySize : "";

        public string ShortName => m_memberAdapt.ShortName;

        public string StrippedName => m_memberAdapt.StrippedName;

        public bool IsString
        {
            get
            {
                if (HasEnumType)
                {
                    return true;
                }

                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "CHAR":
                        return m_memberAdapt.IsArray;
                    case "FILETIME":
                        return true;
                    default:
                        break;
                }

                return false;
            }
        }

        private bool TreatAsCppString
        {
            get
            {
                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "CHAR":
                        return m_memberAdapt.IsArray;
                    default:
                        break;
                }

                return false;
            }
        }

        public bool IsParameterString => TreatAsCppString;

        public bool IsHashString => TreatAsCppString;

        public bool IsFiletime
        {
            get
            {
                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "FILETIME":
                        return true;
                    default:
                        break;
                }

                return false;
            }
        }

        public bool IsBoolean
        {
            get 
            {
                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "BOOL":
                        return true;
                    default:
                        break;
                }
                return false;
            }
        }

        public bool GetIsSameType(string strType)
        {
            return m_memberAdapt.Type.ToUpper().Equals(strType, StringComparison.OrdinalIgnoreCase);
        }

        public string SingleValueCppType
        {
            get
            {
                switch (m_memberAdapt.Type.ToUpper())
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
                        if (HasMessageType)
                        {
                            return MessageType.Name;
                        }
                        else if (HasEnumType)
                        {
                            return EnumType.Name;
                        }

                        return "****UNKNOWN";
                }
            }
        }

        public string SingleValueDartType
        {
            get
            {
                switch (m_memberAdapt.Type.ToUpper())
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
                        if (HasMessageType)
                        {
                            return MessageType.Name;
                        }
                        else if (HasEnumType)
                        {
                            return EnumType.Name;
                        }

                        return "****UNKNOWN";
                }
            }
        }

        public string CSharpBinaryConversion
        {
            get
            {
                switch (m_memberAdapt.Type.ToUpper())
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
                        return IsString ? "ToString" : "ToChar";
                    case "FILETIME":
                        return "ToDateTime";
                    default:
                        if (HasMessageType)
                        {
                            return MessageType.PlainName;
                        }
                        else if (HasEnumType)
                        {
                            return EnumType.ShortName;
                        }

                        return "****UNKNOWN";
                }
            }
        }

        private string CSharpOverrideType
        {
            get
            {
                if (m_metaData != null && m_metaData.IsCSharp && m_metaData.HasTypeUpdate )
                {
                    return m_metaData.TypeUpdate;
                }
                return "";
            }
        }

        public string SingleValueCSharpType
        {
            get
            {
                string strType = CSharpOverrideType;
                if (!string.IsNullOrEmpty(strType))
                {
                    return strType;
                }

                switch (m_memberAdapt.Type.ToUpper())
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
                        if (HasMessageType)
                        {
                            return MessageType.PlainName;
                        }
                        else if (HasEnumType)
                        {
                            return EnumType.Name;
                        }

                        return "****UNKNOWN";
                }
            }
        }

        public bool HasValueConversion => m_metaData != null && m_metaData.HasConversion;

        public string ValueConversion
        {
            get
            {
                if (HasValueConversion)
                {
                    if (m_metaData.Conversion.Contains("*"))
                    {
                        return m_metaData.Conversion.Replace("*", StrippedName);
                    }
                    return m_metaData.Conversion;
                }

                return "";
            }
        }

        public string BsonTransformFn
        {
            get
            {
                if (IsString)
                {
                    return "";
                }

                switch (m_memberAdapt.Type.ToUpper())
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
        }

        public string BsonGetter
        {
            get
            {
                if (IsString)
                {
                    return "_utf8 => .value.to_string()";
                }

                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "FLOAT":
                    case "DOUBLE":
                        return "_double => ";
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
                        return "_int32 => ";
                    case "BOOL":
                        return "_bool => ";
                    case "LONGLONG":
                    case "__INT64":
                        return "_int64 => ";
                    case "FILETIME":
                        return "_date => ";
                    default:
                        return "****UNKNOWN";
                }
            }
        }

        public bool HasBsonCast
        {
            get
            {
                if (IsString)
                {
                    return false;
                }

                switch (m_memberAdapt.Type.ToUpper())
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
        }

        public string BsonCast
        {
            get
            {
                if (IsString)
                {
                    return "";
                }

                switch (m_memberAdapt.Type.ToUpper())
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
        }

        public string JSONCast
        {
            get
            {
                if (IsString)
                {
                    return "";
                }

                switch (m_memberAdapt.Type.ToUpper())
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
        }

        public string CppType
        {
            get
            {
                string strReturn = SingleValueCppType;
                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "CHAR":
                        return m_memberAdapt.IsArray ? "std::string" : strReturn;
                    default:
                        break;
                }

                return strReturn;
            }
        }

        public string DartType
        {
            get
            {
                string strReturn = SingleValueDartType;
                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "CHAR":
                        return m_memberAdapt.IsArray ? "String" : strReturn;
                    default:
                        break;
                }

                return strReturn;
            }
        }
        

        public string BaseJavaType
        {
            get
            {
                string strReturn = "**** UNKNOWN TYPE ****";
                switch (m_memberAdapt.Type.ToUpper())
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
                        strReturn = "int"; // Due to restriction in bson deserialisers
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
                        strReturn = m_memberAdapt.IsArray ? "String" : "char";
                        break;
                    default:
                        if (HasMessageType)
                        {
                            strReturn = MessageType.JavaClassName;
                        }
                        else if (HasEnumType)
                        {
                            strReturn = EnumType.Name;
                        }

                        break;
                }

                return strReturn;
            }
        }

        public string JavaType
        {
            get
            {
                string strReturn = BaseJavaType;
                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "CHAR":
                        return strReturn;
                    default:
                        break;
                }

                if (m_memberAdapt.IsArray)
                {
                    strReturn += "[]";
                }

                return strReturn;
            }
        }

        public string CSharpType
        {
            get
            {
                string strReturn = CSharpOverrideType;
                if (!string.IsNullOrEmpty(strReturn))
                {
                    return strReturn;
                }

                strReturn = SingleValueCSharpType;
                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "CHAR":
                        return (m_memberAdapt.IsArray || m_memberAdapt.IsVariableLengthArray)
                            ? "string"
                            : strReturn;
                    default:
                        break;
                }

                if (m_memberAdapt.IsArray)
                {
                    strReturn += "[]";
                }

                return strReturn;
            }
        }

        public string JavaInterfaceType
        {
            get
            {
                string strReturn = BaseJavaType;
                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "CHAR":
                        return strReturn;
                    default:
                        if (HasMessageType)
                        {
                            strReturn = MessageType.JavaInterfaceName;
                        }

                        break;
                }

                if (m_memberAdapt.IsArray)
                {
                    strReturn += "[]";
                }

                return strReturn;
            }
        }

        public string JavaTransformFn
        {
            get
            {
                string strReturn = "";
                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "SHORT":
                    case "USHORT":
                    case "WORD":
                    case "WCHAR":
                        strReturn = ".shortValue()";
                        break;
                    case "BYTE":
                        strReturn = ".byteValue()";
                        break;
                    case "CHAR":
                        strReturn = m_memberAdapt.IsArray ? "" : ".byteValue()";
                        break;
                    default:
                        break;
                }

                return strReturn;
            }
        }

        public string JavaCast
        {
            get
            {
                string strReturn = "";
                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "CHAR":
                        return m_memberAdapt.IsArray ? "" : "(char) ";
                    default:
                        if (HasMessageType)
                        {
                            strReturn = "(Document) ";
                        }

                        break;
                }

                if (m_memberAdapt.IsArray)
                {
                    strReturn += "(ArrayList<Document>) ";
                }

                return strReturn;
            }
        }

        public string JavaInitialiser
        {
            get
            {
                string strReturn = "**** UNKNOWN TYPE ****";
                switch (m_memberAdapt.Type.ToUpper())
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
                        strReturn = m_memberAdapt.IsArray ? "\"\"" : "0";
                        return strReturn;
                    default:
                        if (HasMessageType)
                        {
                            strReturn = "new " + MessageType.Name;
                        }
                        else if (HasEnumType)
                        {
                            strReturn = EnumType.Name;
                        }

                        break;
                }

                if (m_memberAdapt.IsArray)
                {
                    return "new " + BaseJavaType + "[]";
                }

                return strReturn;
            }
        }

        public string JavaBsonGetter
        {
            get
            {
                string strReturn = "**** UNKNOWN TYPE ****";
                switch (m_memberAdapt.Type.ToUpper())
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
                        strReturn = m_memberAdapt.IsArray ? "getString" : "getInteger";
                        return strReturn;
                    default:
                        if (HasMessageType)
                        {
                            strReturn = "get";
                        }
                        else if (HasEnumType)
                        {
                            strReturn = "getString";
                        }

                        break;
                }

                return strReturn;
            }
        }

        public string CppFullType
        {
            get
            {
                string strReturn = CppType;
                if (IsArray && !IsString)
                {
                    strReturn = CppArrayType +"<" + strReturn + ", " + ArraySize + ">";
                }
                return strReturn;
            }
        }

        public string DartFullType
        {
            get
            {
                string strReturn = DartType;
                if (IsArray && !IsString)
                {
                    strReturn = "List<" + strReturn + ">";
                }
                return strReturn;
            }
        }

        public string CppParameterType
        {
            get
            {
                string strReturn = CppFullType;
                if (HasMessageType)
                {
                    strReturn = MessageType.PointerType;
                    if (IsArray)
                    {
                        strReturn = CppArrayType + "<" + strReturn + ", " + ArraySize + ">";
                    }
                }

                if (IsArray || HasMessageType)
                {
                    strReturn = "const " + strReturn + "&";
                }
                else
                {
                    switch (m_memberAdapt.Type.ToUpper())
                    {
                        case "CHAR":
                            if (m_memberAdapt.IsArray)
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
        }

        public string PrintfCode
        {
            get
            {
                if
                (
                    HasMessageType
                    ||
                    HasEnumType
                    )
                {
                    return "%s";
                }

                switch (m_memberAdapt.Type.ToUpper())
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
        }
        public int NameLength => StrippedName.Length;

        public int ValueAsStringLength
        {
            get
            {
                int nLength = 0;
                if (HasMessageType )
                {
                    nLength = MessageType.TotalJsonSize;
                }
                else if (HasEnumType)
                {
                    nLength = EnumType.MaximumEnumLength;
                }
                else
                {
                    switch (m_memberAdapt.Type.ToUpper())
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
                            nLength = IsArray ? 1 : 3;
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

                if (IsArray)
                {
                    nLength *= NumericArraySize;
                    nLength += NumericArraySize + 2;
                }

                return nLength;
            }
        }

        public string OriginalType 
        {
            get
            {
                if (m_memberAdapt.IsArray)
                {
                    return $"{m_memberAdapt.Type}[{m_memberAdapt.ArraySize}]";
                }

                return m_memberAdapt.Type;
            }
        }

        public int OriginalByteSize => m_memberAdapt.OriginalByteSize;

        public void BuildMessageAdapter()
        {
            if (m_memberAdapt.MessageType  != null)
            {
                m_messageTypeAdapter = MessageAdapterFactory.CreateMessageAdapter(m_memberAdapt.MessageType);
            }
        }

        public bool HasAdditionalAttribute => m_metaData.AdditionalAttr != null;

        public string AdditionalAttribute
        {
            get
            {
                if (HasAdditionalAttribute)
                {
                    return $"{m_metaData.AdditionalAttr.Name}({string.Join(", ", m_metaData.AdditionalAttr.Parameters)})";
                }

                return "*Error no attribute*";
            }
        }

        public bool HasMetadata => m_metaData != null;
        public MemberMetadata Metadata => m_metaData;
    }
}




