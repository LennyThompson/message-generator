struct SDisplayCannedMessage
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
#define JTP_DisplayCannedMessage             89
