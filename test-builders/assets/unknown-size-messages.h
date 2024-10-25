struct SPostOfficePrivateUnregister
{
    //@description    SPostOfficePrivateUnregister | Post Office Private Unregister |
    //                Unregisters interest in the nominated message.
    //@category        POSTOFFICE
    //@generator    POMCLIENT
    //@consumer        POM
    //@alertlevel    ADVISORY
    //@reason        You would send this if you were implementing a PO Client
    //@wabfilter    
    //
    WORD        m_wLower;        //@fielddesc    m_wLower | Lower Bound | specifies the lower bound that this registration handles
    WORD        m_wUpper;        //@fielddesc    m_wUpper | Upper Bound | specifies the Upper bound that this registration handles
    WORD        m_wOffset;        //@fielddesc    m_Offset | Offset for filter | specifies the offset into the message in which to look for the filter bytes
    BYTE        m_byFilterType;    //@fielddesc    m_byFilterType | Fileter Type | specifies the type of filter being used for these messages
    BYTE        m_abyData[JTP_VARIABLE_SIZE_ARRAY];    //@fielddesc    m_abyData | Fileter Data | the bytes to filter against - actually >= 1!
};

struct SPostOfficePrivateSetPersistentData
{
    //@description    SPostOfficePrivateSetPersistentData | Post Office Set Persistent Data |
    //                Uses the key supplied to locate the data and assigns the data supplied.
    //@category        POSTOFFICE
    //@generator    POMCLIENT
    //@consumer        POM
    //@alertlevel    ADVISORY
    //@reason        You would send this if you were implementing a PO Client
    //@wabfilter    
    //
    BYTE        m_byKey[JTP_POM_KEY_SIZE];    //@fielddesc    m_byKey | Persistent Data Key | the (upto) 16 bytes of data that specifies the key to set
    BYTE        m_byData[JTP_VARIABLE_SIZE_ARRAY];    //@fielddesc    m_byData | Key Data | the data to associate with the key - actual length determined by total message length
};

struct SUpdatePatronComment
{
    //@description    SUpdatePatronComment | Update Patron Comment |
    //
    //@category        CARDS
    //@generator    CSSPOPB
    //@consumer        
    //@alertlevel    ADVISORY
    //@wabfilter    SUBSITE SITE GHOST:m_usSiteID LHOST HOST:m_usSiteID
    //@reason        
    //
    USHORT        m_usSiteID;                                //@fielddesc m_usSiteID | SUBSITE SITE ID |
    LONGLONG    m_llPatronID;                            //@fielddesc m_llPatronID | Patron ID |
    DWORD        m_dwNullFlags;                            //@fielddesc m_dwNullFlags | Null Flags |
    char        m_szUserID[JTP_USERID_SIZE];            //@fielddesc m_szUserID | User ID |
    DWORD        m_dwLength;                                //@fielddesc    m_dwLength | length of comment string
    char        m_szComment[JTP_VARIABLE_SIZE_ARRAY];    //@fielddesc    m_szComment | Comment |
};