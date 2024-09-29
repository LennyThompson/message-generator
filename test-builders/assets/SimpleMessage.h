#define JTP_CSS_ENABLEDFLAG_SIZE 12
#define JTP_DESCRIPTION_SIZE 30
struct SVariationInformation
{
    //@description    SVariationInformation | Variation Information |
                        
    //@category        NONMESSAGE
    //@reason        
    //
    BYTE        m_byVariationNumber;    //@fielddesc    m_byVariationNumber | Variation Number |
    CHAR        m_szVariationNumber[10];    //@fielddesc    m_szVariationNumber | Variation Number as string |
    ULONG        m_ulReturnToPlayer;        //@fielddesc    m_ulReturnToPlayer | Return To Player |
};
struct SCSSEquationUpdated
{
    //@description    SCSSEquationUpdated | Equation Updated |
    //
    //@category        CARDS
    //@generator    CDC
    //@consumer        CLC CSS
    //@alertlevel    ADVISORY

    USHORT        m_usSiteID;                                //@fielddesc m_usSiteID | site ID |
    LONGLONG    m_llID;                                    //@fielddesc m_llID | ID |
    USHORT        m_usSystemID;                                //@fielddesc m_usSystemID | System ID |
    USHORT        m_usCssSiteID;                                //@fielddesc m_usCssSiteID | CSS site ID |
    USHORT        m_usTrackingTypeID;                            //@fielddesc m_usTrackingTypeID | Tracking Type ID | 
    char        m_szDescription[JTP_DESCRIPTION_SIZE];        //@fielddesc m_szDescription | Description |
    char        m_szExpression[JTP_CSS_EXPRESSION_SIZE];    //@fielddesc m_szExpression | Expression |
    char        m_szMethodType[JTP_CSS_METHODTYPE_SIZE];    //@fielddesc m_szMethodType | Method Type |
    char        m_szEnabledFlag[JTP_CSS_ENABLEDFLAG_SIZE];    //@fielddesc m_szEnabledFlag | Enabled Flag |
};
struct SConfigMongoQuery
{
    //@category        NONMESSAGE
    char m_szMongoUrl[250];
    char m_szMongoDb[250];
    char m_szMongoCollection[250];
};
