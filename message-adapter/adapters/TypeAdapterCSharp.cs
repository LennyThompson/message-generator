using adapter_interface;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Adapter;

public class TypeAdapterCSharp
    : TypeAdapterImpl
{
    public string Type
    {
        get
        {
            string strReturn = SingleValueType;
            switch (m_memberAdapt.Type.ToUpper())
            {
                case "CHAR":
                    return IsArray ? "string" : strReturn;
                default:
                    break;
            }

            if (IsArray)
            {
                strReturn += "[]";
            }
            return strReturn;
        }
    }

    public string ArrayType
    {
        get
        {
            if (IsArray)
            {
                return $"List<{SingleValueType}>";
            }

            return "****Not an array****";
        }
    }
    public bool IsArray
    {
        get
        {
            switch (m_memberAdapt.Type.ToUpper())
            {
                case "CHAR":
                    return false;
                default:
                    break;
            }
            return m_memberAdapt.IsArray;
        }
        
    }

    public bool IsString
    {
        get
        {
            if (HasEnumType)
            {
                return true;
            }
            else
            {
                switch (m_memberAdapt.Type.ToUpper())
                {
                    case "CHAR":
                        return IsArray;
                    case "FILETIME":
                        return true;
                    default:
                        break;
                }
            }

            return false;
        }
    }

    public bool UseAsString
    {
        get
        {
            switch (m_memberAdapt.Type.ToUpper())
            {
                case "CHAR":
                    return IsArray;
                default:
                    break;
            }

            return false;            
        }
    }

    public override string  SingleValueType
    {
        get
        {
            {
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
                            return MessageAdapter!.PlainName;
                        }
                        else if (HasEnumType)
                        {
                            return EnumAdapter.ShortName;
                        }
                        return "****UNKNOWN";
                }

        }
    }

    public override string BinaryConversion
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
                        return MessageAdapter.PlainName;
                    }
                    else if (HasEnumType)
                    {
                        return EnumAdapter.ShortName;
                    }

                    return "****UNKNOWN";
            }
        }
    }

    public string OverrideType { get; }
    public string CastType { get; }
    public string Initialiser { get; }
    public string ParameterType { get; }

    IMember m_memberAdapt;
    private IMessageAdapter? m_memberMessageTypeAdapter;
    private IEnumAdapter? m_memberEnumAdapter;
}