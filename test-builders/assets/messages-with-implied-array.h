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


struct SEmployeeSessionStarted
{
    //@description    SEmployeeSessionStarted | EmployeeSessionStarted | An employee session has been started.
    //
    //@category        SYSTEM CARDS
    //@generator    SAM
    //@consumer        SRM
    //@alertlevel    ADVISORY
    //@reason        Employee functionality
    //@wabfilter    SUBSITE    
    //
    SEmployeeSessionHeader    m_Header;            //@fielddesc m_Header | Header | Common header fields
    BYTE                    m_byFuncLength;        //@fielddesc m_byFunctionalityLength | FLen | Actual length used in
                                                //    the m_achFunc field
    char                    m_achFunc[JTP_MAX_FUNCTIONALITY_SIZE];    //@fielddesc m_achFunc | Func | Functionality
                                                //    associated with the card. This is an array of ASCII 1's and 0's
                                                //    that describe the allowed functionality.
};
struct SCSSCardCreditSummaryLogged_V2
{
    //@description    SCSSCardCreditSummaryLogged | CSS Card Creidt Summary Logged |
    //
    //@category        CARDS SYSTEM
    //@generator    CSS
    //@consumer        CSS,CDI,TPI
    //@alertlevel    ADVISORY
    //@wabfilter    LHOST HOST:m_usSiteID GHOST:m_usSiteID

    USHORT            m_usSiteID;                                        //@fielddesc m_usSiteID | site | The site ID that this message belongs to
    DWORD            m_dwCardID;                                        //@fielddesc m_dwCardID | CID | ID of the card
    FILETIME        m_ftLastUpdateTime;                                //@fielddesc m_ftLastUpdateTime | Last | Time of the last update
    FILETIME        m_ftHostLoggedTime;                                //@fielddesc m_ftHostLoggedTime | Host Logged Time | Time of the last update
    DWORD            m_dwDeviceSerialNumber;                            //@fielddesc m_dDeviceSerialNumber | DSN | ID of the device associated with this message
    char            m_szGameDescription[JTP_GAME_DESCRIPTION_SIZE];    //@fielddesc m_szGameDescription | Game | Description of the game being played ????
    DWORD            m_dwStroke;                                        //@fielddesc m_dwStroke | ST | Stroke associated with the message
    DWORD            m_dwTurnover;                                    //@fielddesc m_dwTurnover | TO | Turnover associated with the message
    DWORD            m_dwWins;                                        //@fielddesc m_dwWins | WIN | Wins associated with the message
    DWORD            m_dwJackpotWins;                                //@fielddesc m_dwJackpotWins | JWINS | Jackpot Wins associated with the message
    DWORD            m_dwPointsAwarded;                                //@fielddesc m_dwPointsAwarded | Points Awarded | Amount of the points awarded
    BYTE            m_byBalanceSize;                                //@fielddesc m_byBalanceSize | SIze | Number of valid entries in m_asBalance
    USHORT            m_usSourceSiteID;                                //@fielddesc m_usSourceSiteID | Source site ID | Where it happend
    SBalanceTreble    m_asBalance[JTP_VARIABLE_SIZE_ARRAY];            //@fielddesc m_asBalance | Balance | Details of Balance
};

struct SUpdateCancelCreditTicketInfo
{
    //@description    SUpdateCancelCreditTicketInfo | Cancel Credit Ticket Info |
    //                
    //@category        EGM
    //@generator    SAM,MAD
    //@consumer        CDI
    //@alertlevel    ADVISORY
    //@reason        
    //@wabfilter    SUBSITE SITE GHOST:m_usSiteID
    //
    USHORT        m_usSiteID;                    //@fielddesc    m_usSiteID | Site ID | Site ID
    DWORD        m_dwEGMSerialNumber;        //@fielddesc    m_dwEGMSerialNumber | EGM Serial Number | Serial number of the EGM related to this message
    DWORD        m_dwTicketNumber;            //@fielddesc    m_dwTicketNumber | Ticket Number | 10 digit ticket number for the ticket
    FILETIME    m_ftEventDateTime;            //@fielddesc    m_ftHitTime | Hit Time | time of the cancel credit event related to this ticket
    DWORD        m_dwAmount;                    //@fielddesc    m_dwAmount | Value of the cancel credit lockup
    LONG        m_lCashAmount;                //@fielddesc    m_lCashAmount | Value of the cash amount after splitting and rounding
    LONG        m_lChequeAmount;            //@fielddesc    m_lChequeAmount | Value of the cheque amount after rounding
    DWORD        m_dwCardID;                    //@fielddesc    m_dwCardID | Value of the Card related to this ticket
    char        m_szEmployeeFlag;            //@fielddsec    m_szEmployeeFlag | Y if the card is an attendant card. N if it is a patron card 
};
