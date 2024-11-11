using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes;

public class ArrayMember : Member, IArrayMember
{
    private const string DYNAMIC_ARRAY_SIZE = "JTP_VARIABLE_SIZE_ARRAY";
    private bool m_bIsArrayPointer = false;
    private string? m_strArraySize;
    private int m_nArraySize;
    private IDefine? m_defineArraySize;
    private bool m_bIsArray;

    public ArrayMember(Member memberFrom)
    {
        Name = memberFrom.Name;
        Type = memberFrom.Type;
        if (memberFrom.Attributes != null)
        {
            foreach (IAttribute attr in memberFrom.Attributes)
            {
                AddAttribute(attr);
            }
        }
    }

    public new bool IsArray => true;

    public IDefine? ArraySizeDefine
    {
        get => m_defineArraySize;
        set
        {
            m_defineArraySize = value;
            if (m_defineArraySize?.IsNumeric ?? false)
            {
                m_nArraySize = m_defineArraySize.NumericValue;
            }
        }
    }

    public bool IsArrayPointer
    {
        get => m_bIsArrayPointer;
    }

    public string? ArraySize
    {
        get => m_strArraySize;
        set => SetArraySize(value);
    }

    private void SetArraySize(string? strSize)
    {
        if (strSize != null)
        {
            m_strArraySize = strSize;
            m_nArraySize = -1;
            try
            {
                m_nArraySize = int.Parse(m_strArraySize);
            }
            catch (Exception)
            {
                if (String.Compare(m_strArraySize!, DYNAMIC_ARRAY_SIZE, StringComparison.Ordinal) == 0)
                {
                    m_nArraySize = 1;
                }
            }

            if (m_nArraySize < 0)
            {
                try
                {
                    m_nArraySize = m_strArraySize.Split('+')
                        .Select(int.Parse)
                        .Sum();
                }
                catch (Exception)
                {
                }
            }

            m_bIsArrayPointer = true;
            m_bIsArray = true;
        }
    }


    public int NumericArraySize
    {
        get => m_nArraySize;
    }

    public bool IsVariableLengthArray
    {
        get => IsArray && NumericArraySize == 1;
    }

    public int OriginalByteSize
    {
        get
        {
            int nSizeReturn = base.OriginalByteSize;

            if (IsArray)
            {
                nSizeReturn *= NumericArraySize;
            }

            return nSizeReturn;
        }
    }

    public void UpdateName(IMessage msg)
    {
        if (StrippedName == msg.BaseName)
        {
            StrippedName = StrippedName + "Array";
        }
    }


}