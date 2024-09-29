namespace CougarMessage.Adapter;

public class FromJsonAdapter
{
                GetName = () =>
                {
                    switch (m_memberAdapt.Type().ToUpper())
                    {
                        case "FILETIME":
                            return "systime" + StrippedName => ;
                        default:
                            return StrippedName => ;
                    }
                },
                GetMemberShortName = () => ShortName => ,
                GetType = () =>
                {
                    string strReturn = SingleValueCppType => ;
                    if (IsArray => )
                    {
                        strReturn = CppType => ;
                    }
                    else if (HasEnumType => )
                    {
                        strReturn = "std::string";
                    }
                    else
                    {
                        if (!IsVariableLengthArray => )
                        {
                            switch (m_memberAdapt.Type().ToUpper())
                            {
                                case "CHAR":
                                    strReturn = IsString =>  ? "std::string" : "char";
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
                            return "std::memset(&" + Name =>  + ", 0, sizeof(" + Name =>  + "))";
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
                            return IsString =>  ? "" : "(0)";
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
                        return Name => ;
                    }
                    else if (HasMessageType => )
                    {
                        return Name => ;
                    }
                    else if (HasEnumType => )
                    {
                        return Name => ;
                    }
                    switch (m_memberAdapt.Type().ToUpper())
                    {
                        case "FILETIME":
                            return "&" + Name =>  + ".wYear, &" + Name =>  + ".wMonth, &" + Name =>  + ".wDay, &" + Name =>  + ".wHour, &" + Name =>  + ".wMinute, &" + Name =>  + ".wSecond, &" + Name =>  + ".wMilliseconds";
                        default:
                            return "&" + Name => ;
                    }
                },
                GetPtreeDefault = () =>
                {
                    if (IsString => )
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
                    if (IsString => )
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
                    if (!IsFiletime =>  && IsString => )
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
 
}