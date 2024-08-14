using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes.Interfaces;

public class WabFilterTarget : IWabFilterAttribute.IWabFilterTarget
{
    private IWabFilterAttribute.Target m_target;
    private string[]? m_listMemberPath;

    public WabFilterTarget(IWabFilterAttribute.Target target, string[] listMemberPath)
    {
        m_target = target;
        m_listMemberPath = listMemberPath;
    }

    public IWabFilterAttribute.Target Target => m_target;

    public string Name => m_target.ToString();

    public string[]? MemberPath => m_listMemberPath;

    public string Filter => Name + ((m_listMemberPath != null && m_listMemberPath.Length > 0)
        ? ":" + string.Join(".", m_listMemberPath)
        : "");
}

namespace CougarMessage.Parser.MessageTypes
{
    public class WabFilterAttribute : Attribute, IWabFilterAttribute
    {

        private List<WabFilterTarget> m_listTargets;

        public WabFilterAttribute()
        {
            m_listTargets = new List<WabFilterTarget>();
        }

        public string GetFilter()
        {
            return string.Join(" ", m_listTargets.Select(t => t.Filter));
        }

        public void SetSite(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.Site, listMemberPath));
        }

        public void SetSubSite(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.Subsite, listMemberPath));
        }

        public void SetSiteHour(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.SiteHour, listMemberPath));
        }

        public void SetHost(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.Host, listMemberPath));
        }

        public void SetGroupHost(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.Ghost, listMemberPath));
        }

        public void SetWATSite(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.WatSite, listMemberPath));
        }

        public void SetWATHost(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.WatHost, listMemberPath));
        }

        public void SetGHostHour(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.GhostHour, listMemberPath));
        }

        public void SetLHost(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.LHost, listMemberPath));
        }

        public void SetNSAWAB(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.NsaWab, listMemberPath));
        }

        private bool FindTarget(IWabFilterAttribute.Target targetFilter)
        {
            return GetTarget(targetFilter) != null;
        }

        private IWabFilterAttribute.IWabFilterTarget GetTarget(IWabFilterAttribute.Target targetFilter)
        {
            return m_listTargets.FirstOrDefault(target => target.Target == targetFilter);
        }

        public bool IsSite => FindTarget(IWabFilterAttribute.Target.Site);
        public bool IsSubSite => FindTarget(IWabFilterAttribute.Target.Subsite);
        public bool IsSiteHour => FindTarget(IWabFilterAttribute.Target.SiteHour);
        public bool IsWATSite => FindTarget(IWabFilterAttribute.Target.WatSite);
        public bool IsHost => FindTarget(IWabFilterAttribute.Target.Host);
        public bool IsGroupHost => FindTarget(IWabFilterAttribute.Target.Ghost);
        public bool IsGHostHour => FindTarget(IWabFilterAttribute.Target.GhostHour);
        public bool IsLHost => FindTarget(IWabFilterAttribute.Target.LHost);
        public bool IsWATHost => FindTarget(IWabFilterAttribute.Target.WatHost);
        public bool IsNSAWAB => FindTarget(IWabFilterAttribute.Target.NsaWab);

        public IWabFilterAttribute.IWabFilterTarget SiteTarget => GetTarget(IWabFilterAttribute.Target.Site);
        public IWabFilterAttribute.IWabFilterTarget SubSiteTarget => GetTarget(IWabFilterAttribute.Target.Subsite);
        public IWabFilterAttribute.IWabFilterTarget SiteHourTarget => GetTarget(IWabFilterAttribute.Target.SiteHour);
        public IWabFilterAttribute.IWabFilterTarget WATSiteTarget => GetTarget(IWabFilterAttribute.Target.WatSite);
        public IWabFilterAttribute.IWabFilterTarget HostTarget => GetTarget(IWabFilterAttribute.Target.Host);
        public IWabFilterAttribute.IWabFilterTarget GroupHostTarget => GetTarget(IWabFilterAttribute.Target.Ghost);
        public IWabFilterAttribute.IWabFilterTarget LHostTarget => GetTarget(IWabFilterAttribute.Target.LHost);
        public IWabFilterAttribute.IWabFilterTarget GHostHourTarget => GetTarget(IWabFilterAttribute.Target.GhostHour);
        public IWabFilterAttribute.IWabFilterTarget WATHostTarget => GetTarget(IWabFilterAttribute.Target.WatHost);
        public IWabFilterAttribute.IWabFilterTarget NSAWABTarget => GetTarget(IWabFilterAttribute.Target.NsaWab);
    }
}

