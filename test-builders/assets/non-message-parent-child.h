struct SChild
{
    DWORD    m_dwValue;
    std::string    m_strSQLstring;
    std::vector<int>    m_listValues;
    std::map<int, std::string>    m_mapStrings;
};

struct SParent
{
    DWORD    m_dwSerialNumber;
    std::vector<SChild>    m_listChildren;
    SChild    m_child;
    std::string    m_strStuff;
};