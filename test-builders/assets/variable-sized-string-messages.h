struct SLogEvent_V2
{
    //@description    SLogEvent_V2 | Logging Event to Database |
    //                
    //@category        SYSTEM
    //@generator    
    //@consumer        CEL
    //@alertlevel    ADVISORY
    //@reason        Transfering load from the FCC and CDI component to the CEL
    //@wabfilter    
    //
    DWORD    m_dwLength;                    //@fielddesc    m_dwLength | length of sql string
    char    m_szSQLstring;                //@fielddesc    m_szSQLstring | SQL string |
};

struct SLogEvent_V3
{
    //@description    SLogEvent_V3 | Logging Event to Database |
    //                
    //@category        SYSTEM
    //@generator    
    //@consumer        CEL
    //@alertlevel    ADVISORY
    //@reason        Transfering load from the FCC and CDI component to the CEL
    //@wabfilter    
    //
    DWORD    m_dwSerialNumber;
    DWORD    m_dwLength;                    //@fielddesc    m_dwLength | length of sql string
    char    m_szSQLstring;                //@fielddesc    m_szSQLstring | SQL string |
};
struct SUpdateDisplaySlot
{
    //@description    
    //                
    //@category        KENO
    //@generator    KTC
    //@consumer        JDS
    //@alertlevel    NORMAL
    //@reason        Notification
    //@wabfilter    
    //
    WORD        m_wSlot;
    BYTE        m_bySecondsToDisplay;
    BYTE        m_byDisplayFunction;
    WORD        m_wTextLength;
    char        m_strText[1];
};