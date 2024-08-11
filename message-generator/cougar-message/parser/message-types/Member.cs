using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.CougarMessage.Parser.MessageTypes
{
    public class Member : IMember
    {
        private enum AttributeKeys { FIELDDESC }
        private static string DYNAMIC_ARRAY_SIZE = "JTP_VARIABLE_SIZE_ARRAY";
        private static string MEMBER_PREFIX = "m_";
        private string m_strName;
        private string m_strStrippedName;
        private string m_strType;
        private IMessage m_messageType;
        private IEnum m_enumType;
        private IDefine m_defineArraySize;

        private bool m_bIsArray;
        private bool m_bIsArrayPointer = false;
        private string m_strArraySize;
        private int m_nArraySize;
        private Dictionary<string, IAttribute> m_mapAttributes;

        public Member()
        {
            m_mapAttributes = new Dictionary<string, IAttribute>();
        }

        public void SetName(string strName)
        {
            m_strName = strName;
        }

        public void SetType(string strType)
        {
            m_strType = strType;
        }

        public void SetArraySize(string strSize)
        {
            m_strArraySize = strSize;
            m_nArraySize = -1;
            try
            {
                m_nArraySize = int.Parse(m_strArraySize);
            }
            catch (Exception ex)
            {
                if (m_strArraySize.CompareTo(DYNAMIC_ARRAY_SIZE) == 0)
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
                catch (Exception ex)
                {
                }
            }
            m_bIsArrayPointer = true;
            m_bIsArray = true;
        }

        public void SetArraySizeDefine(IDefine defineArraySize)
        {
            m_defineArraySize = defineArraySize;
            if (m_defineArraySize.IsNumeric())
            {
                m_nArraySize = m_defineArraySize.NumericValue();
            }
        }

        public void AddAttribute(IAttribute attrAdd)
        {
            m_mapAttributes[attrAdd.Name().ToUpper()] = attrAdd;
        }

        public void SetMessageType(IMessage messageType)
        {
            m_messageType = messageType;
        }

        public void SetEnumType(IEnum enumType)
        {
            m_enumType = enumType;
        }

        public string Name()
        {
            return m_strName;
        }

        public string ShortName()
        {
            if (Name().IndexOf(MEMBER_PREFIX) == 0)
            {
                return Name().Substring(2);
            }
            return m_strName;
        }

        public string StrippedName()
        {
            if (m_strStrippedName == null)
            {
                m_strStrippedName = string.Concat(ShortName().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(part => char.IsUpper(part[0])));
                if (string.IsNullOrEmpty(m_strStrippedName))
                {
                    m_strStrippedName = char.ToUpper(ShortName()[0]) + ShortName().Substring(1);
                }
            }
            return m_strStrippedName;
        }

        public string Prefix()
        {
            return ShortName().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(part => char.IsLower(part[0]));
        }

        public string Type()
        {
            return m_strType;
        }

        public IMessage MessageType()
        {
            return m_messageType;
        }

        public IEnum EnumType()
        {
            return m_enumType;
        }

        public bool IsArray()
        {
            return m_bIsArray;
        }

        public bool IsArrayPointer()
        {
            return m_bIsArrayPointer;
        }

        public bool IsVariableLengthArray()
        {
            return IsArray() && m_nArraySize == 1;
        }

        public string ArraySize()
        {
            return m_strArraySize;
        }

        public int NumericArraySize()
        {
            return m_nArraySize;
        }

        public List<IAttribute> Attributes()
        {
            return m_mapAttributes.Values.ToList();
        }

        public int OriginalByteSize()
        {
            int nSizeReturn = 1;
            switch (Type().ToUpper())
            {
                case "FLOAT":
                    nSizeReturn = 4;
                    break;
                case "DOUBLE":
                    nSizeReturn = 8;
                    break;
                case "INT":
                case "LONG":
                case "ULONG":
                case "UINT":
                case "DWORD":
                case "BOOL":
                    nSizeReturn = 4;
                    break;
                case "SHORT":
                case "USHORT":
                case "WORD":
                case "WCHAR":
                    nSizeReturn = 2;
                    break;
                case "BYTE":
                case "CHAR":
                    break;
                case "LONGLONG":
                case "__INT64":
                    nSizeReturn = 8;
                    break;
                case "FILETIME":
                    nSizeReturn = 16;
                    break;
                default:
                    if (MessageType() != null)
                    {
                        nSizeReturn = MessageType().MessageByteSize();
                    }
                    break;
            }
            if (IsArray())
            {
                nSizeReturn *= NumericArraySize();
            }

            return nSizeReturn;
        }

        public string ShortFieldDescription()
        {
            if (m_mapAttributes.ContainsKey(AttributeKeys.FIELDDESC.ToString()) &&
                m_mapAttributes[AttributeKeys.FIELDDESC.ToString()].Values().Count > 0)
            {
                return ((IFielddescAttribute)(m_mapAttributes[AttributeKeys.FIELDDESC.ToString()])).ShortDescription();
            }
            return "";
        }

        public string LongFieldDescription()
        {
            if (m_mapAttributes.ContainsKey(AttributeKeys.FIELDDESC.ToString()) &&
                m_mapAttributes[AttributeKeys.FIELDDESC.ToString()].Values().Count > 1)
            {
                return ((IFielddescAttribute)(m_mapAttributes[AttributeKeys.FIELDDESC.ToString()])).LongDescription();
            }
            return "";
        }

        public void SetStrippedName(string strStrippedName)
        {
            m_strStrippedName = strStrippedName;
        }

        public static void UpdateStrippedNames(IMember memberClash, IMember memberAdd)
        {
            if (memberClash.ShortName().CompareTo(memberAdd.ShortName()) == 0)
            {
                UpdateMemberStrippedName(memberClash);
                UpdateMemberStrippedName(memberAdd);
            }
            else
            {
                UpdateMemberStrippedName(memberAdd);
            }
        }

        private static void UpdateMemberStrippedName(IMember member)
        {
            switch (member.Prefix())
            {
                case "aby":
                case "ach":
                case "adw":
                case "all":
                case "ab":
                case "as":
                case "aw":
                case "a":
                case "by":
                case "b":
                case "ch":
                case "c":
                case "dw":
                case "d":
                case "e":
                case "ft":
                case "f":
                case "i64":
                case "i":
                case "l":
                case "ll":
                case "s":
                case "si":
                case "str":
                case "sz":
                case "ui":
                case "ul":
                case "us":
                case "wsz":
                case "wa":
                case "w":
                    member.SetStrippedName(member.StrippedName() + GetAppend(member));
                    break;
                default:
                    string strPrefix = member.Prefix();
                    strPrefix = char.ToUpper(strPrefix[0]) + strPrefix.Substring(1);
                    member.SetStrippedName(strPrefix + member.StrippedName());
                    break;
            }
        }

        private static string GetAppend(IMember member)
        {
            switch (member.Type().ToUpper())
            {
                case "FLOAT":
                    return "Float";
                case "DOUBLE":
                    return "Double";
                case "INT":
                    return "Int32";
                case "LONG":
                    return "Int32";
                case "ULONG":
                    return "UInt32";
                case "UINT":
                    return "UInt32";
                case "USHORT":
                case "WORD":
                case "WCHAR":
                    return "UInt16";
                case "DWORD":
                    return "UInt32";
                case "SHORT":
                    return "Int16";
                case "BOOL":
                    return "Bool";
                case "LONGLONG":
                case "__INT64":
                    return "Int64";
                case "BYTE":
                    return "Byte";
                case "CHAR":
                    return "Char";
                case "FILETIME":
                    return "DateTime";
                default:
                    return "_";
            }
        }

        public void UpdateName(IMessage msg)
        {
            if (StrippedName().CompareTo(msg.BaseName()) == 0)
            {
                if (IsArray())
                {
                    m_strStrippedName = StrippedName() + "Array";
                }
                else
                {
                    m_strStrippedName = StrippedName() + "Var";
                }
            }
        }
    }
}

