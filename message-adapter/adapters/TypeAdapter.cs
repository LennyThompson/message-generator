using adapter_interface;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Adapter;

public abstract class TypeAdapterImpl
    : ITypeAdapter
{
    public string Type => SingleValueType;

    public string BaseType => SingleValueType;

    public string DeclaredType => m_memberAdapt.Type;
    public abstract string ArrayType { get; }

    public bool HasMessageType => m_memberAdapt.MessageType != null;
    public bool HasEnumType => m_memberAdapt.EnumType != null;

    public IMessageAdapter? MessageAdapter
    {
        get => m_memberMessageTypeAdapter;
        set => m_memberMessageTypeAdapter = value;
    }
    public IEnumAdapter? EnumAdapter 
    {
        get => m_memberEnumAdapter;
        set => m_memberEnumAdapter = value;
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

    public bool IsVariableLengthArray => m_memberAdapt.IsVariableLengthArray;

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

    public bool IsParameterString => UseAsString;
    public bool IsHashString  => UseAsString;

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
    public bool IsSameTypeAs(ITypeAdapter typeOther)
    {
        return typeOther.IsSameTypeAs(m_memberAdapt.Type);
    }

    public bool IsSameTypeAs(string strOtherType)
    {
        return m_memberAdapt.Type.ToUpper() == strOtherType.ToUpper();
    }

    public abstract string SingleValueType { get; }

    public abstract string BinaryConversion { get; }

    public abstract string OverrideType { get; }
    public abstract string CastType { get; }
    public abstract string Initialiser { get; }
    public abstract string ParameterType { get; }

    IMember m_memberAdapt;
    private IMessageAdapter? m_memberMessageTypeAdapter;
    private IEnumAdapter? m_memberEnumAdapter;
}