#define JTP_VARIABLE_SIZE_ARRAY			1			//The minimum size for a variable length array field                                        Defined in JTProtocolPlatform.h
#define JTP_MAX_VARIATIONS  10

struct SEGMGameMeters
{
    //@description	SEGMGameMeters | EGM Game Meters |
    //				
    //@category		NONMESSAGE
    //@reason		meters structure for game meter messages
    //
    DWORD		m_dwStroke;				//@fielddesc	m_dwStroke | Stroke |
    DWORD		m_dwTurnover;			//@fielddesc	m_dwTurnover | Turnover |
    DWORD		m_dwWins;				//@fielddesc	m_dwWins | Wins |
    DWORD		m_dwProgressiveWin;		//@fielddesc	m_dwProgressiveWin | Progressive Win |
};

struct SVariationInformation
{
    //@description	SVariationInformation | Variation Information |
    //				
    //@category		NONMESSAGE
    //@reason		
    //
    BYTE		m_byVariationNumber;	//@fielddesc	m_byVariationNumber | Variation Number |
    ULONG		m_ulReturnToPlayer;		//@fielddesc	m_ulReturnToPlayer | Return To Player |
    int m_nMetersCount; //@fielddesc	m_nMetersCount | Number of meters|
    SEGMGameMeters   m_sMeters[JTP_VARIABLE_SIZE_ARRAY];              //@fielddesc    m_sMeters | Meters |
};

struct SUpdatedEGMGameMeters
{
    //@description    SUpdatedEGMGameMeters | Update EGM Game Meters |
    //                This structure contains all the possible meters that
    //                may be returned from an EGM (or nonsense)
    //
    //@category        EGM
    //@generator       FCC
    //@consumer        FCC
    //@alertlevel      ADVISORY
    //@reason          sent from fcc at site to update meters on higher level fcc's
    //@wabfilter       SUBSITE SITE GHOST:m_usSiteID
    //
    USHORT      m_usSiteID;                //@fielddesc    m_usSiteID | Site ID |
    DWORD       m_dwEGMSerialNumber;       //@fielddesc    m_dwEGMSerialNumber | EGM Serial Number |

    DWORD       m_dwGameVersionNumber;      //@fielddesc    m_dwGameVersionNumber | Game Version Number |
    DWORD       m_dwVariation;               //@fielddesc    m_dwVariation | Variation |
    FILETIME    m_ftTime;                    //@fielddesc    m_ftTime | Time |

    SVariationInformation   m_sVariation[JTP_MAX_VARIATIONS];  //@fielddesc    m_sVariation | Variation |
};

#define JTP_UpdatedEGMGameMeters							139
#define JTP_ThirdPartyFloorControllerLockRequest                1188
#define JTP_ThirdPartyFloorControllerLockReply                    1189
#define JTP_ThirdPartyFloorControllerUnlockRequest                1190
#define JTP_ThirdPartyFloorControllerUnlockReply                  1191
#define JTP_ThirdPartyFloorControllerRequestAllLocks              1192
#define JTP_ThirdPartyFloorControllerAllLocksRequested            1193
#define JTP_ThirdPartyFloorControllerLockStateChanged            1194
