using CougarMessage.Adapter;

public struct TraceMemberJsonAdapter
{
    public bool? HasExternalKey { get; set; }
    public string ExternalKey { get; set; }
    public string JsonPath { get; set; }
}

public class MemberPathAdapter
{
    private MessageAdapter _msgAdapt;
    private MessageAdapter _msgFromAdapt;
    public MemberPathAdapter(MessageAdapter msgAdapt, MessageAdapter msgFromAdapt)
    {
        _msgAdapt = msgAdapt;
        _msgFromAdapt = msgFromAdapt;
    }

    public MessageAdapter GeneratedMessage => _msgAdapt;
    public bool IsSameMessageAsGenerator => _msgAdapt.Name == _msgFromAdapt.Name;

    public List<TraceMemberJsonAdapter> TraceMemberJsonPath
    {
        get
        {
            var listAdapters = new List<TraceMemberJsonAdapter>
                                {
                                    new TraceMemberJsonAdapter()
                                    {
                                        HasExternalKey = false,
                                        ExternalKey = "",
                                        JsonPath =  _msgFromAdapt.UseTimestampRange(_msgAdapt) ? "\"_timestamp\" : { $gte : timestampFrom, $lt : timestampTo }" : "\"_timestamp\" : { $gte : timestampFrom }"
                                    }
                                };

            listAdapters.AddRange(
                _msgFromAdapt.TraceMembers
                    .Select(memberAssoc =>
                    {
                        var strExtKey = _msgFromAdapt.ExternalKey != null ? _msgFromAdapt.ExternalKey.Name : "";
                        string strThisPath = "****error****";
                        if (memberAssoc.Source == "externalKey")
                        {
                            strThisPath = strExtKey;
                        }
                        else if (_msgFromAdapt.HasMember(memberAssoc.Source, memberAssoc.Source))
                        {
                            strThisPath = "value." + string.Join(".",
                                _msgFromAdapt.GetMemberByName(memberAssoc.Source, memberAssoc.Source)
                                    .Select(memberAdapt => memberAdapt.StrippedName));
                        }

                        string strJsonPath = "";
                        var strMemberName = memberAssoc.Destinations
                            .FirstOrDefault(name => _msgAdapt.HasMember(name, name));
                        if (!string.IsNullOrEmpty(strMemberName))
                        {
                            strJsonPath = "\"" + string.Join(".",
                                _msgAdapt.GetMemberByName(strMemberName, strMemberName)
                                    .Select(memberAdapt => memberAdapt.StrippedName)) + "\" : " + strThisPath;
                        }

                        return new TraceMemberJsonAdapter()
                        {
                            HasExternalKey = !string.IsNullOrEmpty(strExtKey),
                            ExternalKey = strExtKey,
                            JsonPath = strJsonPath
                        };
                    })
                    .ToList()
                );
            return listAdapters;
        }
    }
}

public struct ExternalKeyAdapter
{
    private MessageAdapter _msgAdapt;

    public ExternalKeyAdapter(MessageAdapter msgAdapt)
    {
        _msgAdapt = msgAdapt;
    }
    public bool HasExternalKey => _msgAdapt.HasExternalKey;
    public string ExternalKey => _msgAdapt.ExternalKeyAdapter.ExternalKey;
    public string KeySnippet => _msgAdapt.ExternalKeyAdapter.KeySnippet;
}
