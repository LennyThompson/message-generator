struct SUnsuspendEGM 
{ 
//@description SUnsuspendEGM | Unsuspend EGM | 
//    Sent to suspend an EGM 
//@category  EGM 
//@generator WILDCAT 
//@consumer  FCC 
//@alertlevel ADVISORY 
//@reason  This message appears when the Wildcat application has been requested 
//    to unsuspend a machine. 
//@wabfilter SUBSITE SITE LHOST HOST:m_usSiteID GHOST:m_usSiteID 
// 
USHORT  m_usSiteID;        //@fielddesc m_usSiteID | Site ID | Site ID of where the machine is 
EEGMDeviceType  m_dwEGMSerialNumber;     //@fielddesc m_dwEGMSerialNumber | EGM Serial Number | Serial Number of the machine to unsuspend 
char  m_szReason[JTP_SUSPENDSTRING_SIZE]; //@fielddesc m_szReason | Reason | Comment regarding the reason for unsuspending the machine 
}; 
 
 
struct SEGMSuspended 
{ 
//@description SEGMSuspended | EGM Suspended | 
//    Sent once an EGM has been suspended 
//@category  EGM 
//@generator ECG 
//@consumer  FCC 
//@alertlevel ADVISORY 
//@reason  This message appears when the ECG has succesfully sent the suspend  
//    message to an EGM 
//@wabfilter SUBSITE SITE GHOST:m_usSiteID 
// 
USHORT  m_usSiteID;     //@fielddesc m_usSiteID | Site ID | Site ID this message is for 
DWORD  m_dwEGMSerialNumber;  //@fielddesc m_dwEGMSerialNumber | EGM Serial Number | Serial number of the EGM 
char  m_szReason[JTP_SUSPENDSTRING_SIZE]; //@fielddesc m_szReason | Reason | Reason the machine was suspended 
FILETIME m_ftTime;     //@fielddesc m_ftTime | Time | Time at which the machine was suspended 
DWORD  m_dwSequenceNumber;   //@fielddesc m_dwSequencerNumber | Sequence Number | Event sequence number 
}; 
 
 
struct SEGMUnsuspended 
{ 
//@description SEGMUnsuspended | EGM Unsuspended | 
//    Sent once an EGM has been unsuspended 
//@category  EGM 
//@generator ECG 
//@consumer  FCC 
//@alertlevel ADVISORY 
//@reason  This message appears when the ECG has succesfully sent the unsuspend  
//    message to an EGM 
//@wabfilter SUBSITE SITE GHOST:m_usSiteID 
// 
USHORT  m_usSiteID;     //@fielddesc m_usSiteID | Site ID | Site ID this message is for 
DWORD  m_dwEGMSerialNumber;  //@fielddesc m_dwEGMSerialNumber | EGM Serial Number | Serial number of the EGM 
char  m_szReason[JTP_SUSPENDSTRING_SIZE]; //@fielddesc m_szReason | Reason | Reason the machine was unsuspended 
FILETIME m_ftTime;     //@fielddesc m_ftTime | Time | Time at which the machine was suspended 
DWORD  m_dwSequenceNumber;   //@fielddesc m_dwSequencerNumber | Sequence Number | Event sequence number 
}; 
\nstruct SSetJackpotContributionMessageTimer 
{ 
//@description SSetJackpotContributionMessageTimer | Set Jackpot Contribution Message Timer | 
//     
//@category  JACKPOT 
//@generator JPC 
//@consumer  FCC 
//@alertlevel ADVISORY 
//@reason  sent by jpc to tell fcc how often to send contribution messages 
//@wabfilter LHOST:m_usFloorControllerSiteID HOST:m_usFloorControllerSiteID 
// 
USHORT  m_usJackpotSiteID;   //@fielddesc m_usJackpotSiteID | Jackpot Site ID | Jackpot Site ID 
DWORD  m_dwJackpotPoolNumber;  //@fielddesc m_dwJackpotPoolNumber | Jackpot Pool Number | Translates to the pool_number 
USHORT  m_usTimerValue;    //@fielddesc m_usTimerValue | Timer Value | Timer value in seconds 
USHORT  m_usFloorControllerSiteID; //@fielddesc m_usFloorControllerSiteID | Floor Controller Site ID | The site that the floor controller is on 
}; 
 
struct SEGMMeters 
{ 
   //@description	SEGMMeters | EGM Meters | 
   //				This structure contains all the possible meters that 
   //				may be returned from an EGM 
   //@category		NONMESSAGE 
   //@reason		 
   // 
   USHORT  m_usJackpotSiteID;   //@fielddesc m_usJackpotSiteID | Jackpot Site ID | Jackpot Site ID 
   DWORD m_dwTotalStroke;                //@fielddesc	m_dwTotalStroke | Total Stroke | 
   DWORD m_dwTotalTurnover;                //@fielddesc	m_dwTotalTurnover | Total Turnover | 
   DWORD m_dwTotalWins;                    //@fielddesc	m_dwTotalWins | Total Wins | 
   DWORD m_dwTotalCancelCredit;            //@fielddesc	m_dwTotalCancelCredit | Total Cancel Credit | 
   DWORD m_dwTotalCashTicketPrinterOut;    //@fielddesc	m_dwTotalCashTicketPrinterOut | Total Cash Ticket Printer Out | 
   DWORD m_dwTotalCentsIn;                //@fielddesc	m_dwTotalCentsIn | Total Cents In | 
}; 
 
struct SEGMMeters_V2 
{ 
   //@description	SEGMMeters | EGM Meters | 
   //				This structure contains all the possible meters that 
   //				may be returned from an EGM 
   //@category		NONMESSAGE 
   //@reason		 
   // 
   DWORD m_dwTotalStroke;                //@fielddesc	m_dwTotalStroke | Total Stroke | 
   DWORD m_dwTotalTurnover;                //@fielddesc	m_dwTotalTurnover | Total Turnover | 
   DWORD m_dwTotalWins;                    //@fielddesc	m_dwTotalWins | Total Wins | 
   DWORD m_dwTotalCancelCredit;            //@fielddesc	m_dwTotalCancelCredit | Total Cancel Credit | 
   DWORD m_dwTotalCashTicketPrinterOut;    //@fielddesc	m_dwTotalCashTicketPrinterOut | Total Cash Ticket Printer Out | 
   DWORD m_dwTotalCentsIn;                //@fielddesc	m_dwTotalCentsIn | Total Cents In | 
}; 
 
struct SSetJackpotContributionMessageTimer_V2 
{ 
//@description SSetJackpotContributionMessageTimer_V2 | Set Jackpot Contribution Message Timer V2 | 
//     
//@category  JACKPOT 
//@generator JPC 
//@consumer  FCC 
//@alertlevel ADVISORY 
//@reason  Sent by JPC to tell FCC current pool info and how often to send contribution messages. 
//@wabfilter LHOST:m_usFloorControllerSiteID HOST:m_usFloorControllerSiteID 
// 
DWORD  m_dwJackpotPoolNumber;      //@fielddesc m_dwJackpotPoolNumber | Jackpot Pool Number | Translates to the pool_number. 
SEGMMeters  m_usTimerValue;        //@fielddesc m_usTimerValue | Timer Value | Update timer value in seconds. 
char  m_szPoolName[JTP_JACKPOT_POOL_NAME_SIZE]; //@fielddesc m_szPoolName | Pool Name | Jackpot pool description. 
char  m_szPoolType[JTP_JACKPOT_POOL_TYPE_SIZE]; //@fielddesc m_szPoolType | Pool Type | Pool type (SYS, etc). 
char  m_szSubPoolType[JTP_SUB_POOL_TYPE_SIZE]; //@fielddesc m_szSubPoolType | Sub Type | Pool sub type (ERP, etc). 
DWORD  m_dwReset;         //@fielddesc m_dwReset | Reset | Reset value. 
DWORD  m_dwMaximumValue;       //@fielddesc m_dwMaximumValue | Max | Maximum displayable pool value. 
DWORD  m_dwCurrentOverflow;      //@fielddesc m_dwCurrentOverflow | Overflow | Contributions in excess of the maximum value. 
DWORD  m_dwMaxAutopay;        //@fielddesc m_dwMaxAutopay | Maximum amount for autopay 
double  m_dIncrement;        //@fielddesc m_dIncrement | Inc | Increment rate. 
USHORT  m_usFloorControllerSiteID; //@fielddesc m_usFloorControllerSiteID | Floor Controller Site ID | The site that the floor controller is on 
}; 

enum EEGMDeviceType 
{ 
    EGMDevice_Egm = 0, 
    EGMDevice_NoteAcceptor = 5 
}; 
#define JTP_SetJackpotContributionMessageTimer_V2    89 
#define JTP_SetJackpotContributionMessageTimer     90 
#define JTP_EGMUnsuspended     100 
#define JTP_EGMSuspended     101 
#define JTP_UnsuspendEGM     102 
#define JTP_EGMMeters_V2     111 

