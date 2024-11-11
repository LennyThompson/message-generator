namespace adapter_interface;

public interface ITypeAdapter
{
    string Type { get; }
    string BaseType { get; }
    string DeclaredType { get; }
    string ArrayType { get; }
    bool HasMessageType { get; }
    bool HasEnumType { get; }
    IMessageAdapter? MessageAdapter { get; }
    IEnumAdapter? EnumAdapter { get; }
    
    public bool IsArray { get; }
    
    bool IsVariableLengthArray { get; }

    bool IsString { get;  }
    
    bool UseAsString { get; }
    bool IsParameterString { get;  }
    bool IsHashString { get;  }
    
    bool IsFiletime { get;  }
    bool IsBoolean { get;  }
    
    bool IsSameTypeAs(ITypeAdapter typeOther);
    bool IsSameTypeAs(string strOtherType);
    
    string SingleValueType { get; }
    string BinaryConversion { get; }
    
    string OverrideType { get;  }
    string CastType { get; }
    string Initialiser { get; }
    
    string ParameterType { get; }
}

