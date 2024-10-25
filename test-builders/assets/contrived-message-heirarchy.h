#define JTP_CashierAdjustmentRequest                        815
#define JTP_CashierAdjustmentRequest_V1                        816
#define JTP_CashierAdjustmentRequest_V2                        817


struct SCashierAdjustmentRequest
{
    //@description    SCashierAdjustmentRequest | Cashier Adjustment Request |
    //                
    //@category        SYSTEM
    //@generator    CSSPOPB
    //@consumer        FAT
    //@alertlevel    ADVISORY
    //@reason        
    //@wabfilter    SITE
    //
    USHORT        m_usSiteID;                        //@fielddesc    m_usSiteID | Site ID |
    LONGLONG    m_llCBGAccountID;
    LONG        m_lCentsAmount;
    DWORD        m_dwCardID;
    char        m_szUserID[JTP_USER_ID_SIZE];
    char        m_szSupervisorID[JTP_USER_ID_SIZE];
    char        m_szComments[100 + 1];
    DWORD        m_dwEGMSerialNumber;
    BYTE        m_byCBGAccountCashTransactionTypeID;
    USHORT        m_usCSSSiteID;
    FILETIME    m_ftTransactionDT;
};


struct SCashierAdjustmentRequest_V1
{
    //@description    SCashierAdjustmentRequest_V1 | Cashier Adjustment Request V1 |
    //                
    //@category        SYSTEM
    //@generator    CSSPOPB
    //@consumer        FAT
    //@alertlevel    ADVISORY
    //@reason        
    //@wabfilter    SITE
    //
    USHORT        m_usSiteID;                        //@fielddesc    m_usSiteID | Site ID |
    LONGLONG    m_llCBGAccountID;
    LONG        m_lCentsAmount;
    DWORD        m_dwCardID;
    char        m_szUserID[JTP_USER_ID_SIZE];
    char        m_szSupervisorID[JTP_USER_ID_SIZE];
    char        m_szComments[100 + 1];
    DWORD        m_dwEGMSerialNumber;
    BYTE        m_byCBGAccountCashTransactionTypeID;
    USHORT        m_usCSSSiteID;
    FILETIME    m_ftTransactionDT;
    SCashierAdjustmentRequest m_adjustmentCashier;
};



struct SCashierAdjustmentRequest_V2
{
    //@description    SCashierAdjustmentRequest_V2 | Cashier Adjustment Request V2 |
    //                
    //@category        SYSTEM
    //@generator    CSSPOPB
    //@consumer        FAT
    //@alertlevel    ADVISORY
    //@reason        
    //@wabfilter    SITE
    //
    USHORT        m_usSiteID;                        //@fielddesc    m_usSiteID | Site ID |
    LONGLONG    m_llCBGAccountID;
    LONG        m_lCentsAmount;
    DWORD        m_dwCardID;
    char        m_szUserID[JTP_USER_ID_SIZE];
    char        m_szSupervisorID[JTP_USER_ID_SIZE];
    char        m_szComments[100 + 1];
    DWORD        m_dwEGMSerialNumber;
    BYTE        m_byCBGAccountCashTransactionTypeID;
    USHORT        m_usCSSSiteID;
    FILETIME    m_ftTransactionDT;
    SCashierAdjustmentRequest_V1 m_adjustmentCashier;
};
