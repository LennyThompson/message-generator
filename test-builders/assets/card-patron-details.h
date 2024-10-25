struct SCardPatronDetailsResponse
{
    //@description    SCardPatronDetailsResponse | Patron Details Response |
    //
    //@category        TPS
    //@generator    PAD / FAT
    //@consumer        TPI
    //@alertlevel    ADVISORY
    //@reason       for wabfilter
    //@wabfilter    HOST:m_usSiteID
    //
    DWORD m_dwRequestID;
    USHORT m_usSiteID;
    ECardType m_cardType;
    ECardStatus m_cardStatus;
    SPatronDetailsRequested m_patronDetails;
    EPatronMembershipStatus m_memberStatus;
    BOOL m_bPINRequired;
    EPatronRequestStatus m_requestStatus;
};