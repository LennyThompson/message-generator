using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Parser.MessageTypes
{
    public class WabFilterAttribute : Attribute, IWabFilterAttribute
    {
        public class WabFilterTarget : IWabFilterAttribute.IWabFilterTarget
        {
            private IWabFilterAttribute.Target m_target;
            private string[] m_listMemberPath;

            public WabFilterTarget(IWabFilterAttribute.Target target, string[] listMemberPath)
            {
                m_target = target;
                m_listMemberPath = listMemberPath;
            }

            public IWabFilterAttribute.Target Target() => m_target;

            public string Name() => m_target.ToString();

            public string[] MemberPath() => m_listMemberPath;

            public string Filter() => Name() + ((m_listMemberPath != null && m_listMemberPath.Length > 0)
                    ? ":" + string.Join(".", m_listMemberPath)
                    : "");
        }

        private List<WabFilterTarget> m_listTargets;

        public WabFilterAttribute()
        {
            m_listTargets = new List<WabFilterTarget>();
        }

        public string GetFilter()
        {
            return string.Join(" ", m_listTargets.Select(t => t.Filter()));
        }

        public void SetSite(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.SITE, listMemberPath));
        }

        public void SetSubSite(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.SUBSITE, listMemberPath));
        }

        public void SetSiteHour(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.SITEHOUR, listMemberPath));
        }

        public void SetHost(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.HOST, listMemberPath));
        }

        public void SetGroupHost(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.GHOST, listMemberPath));
        }

        public void SetWATSite(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.WATSITE, listMemberPath));
        }

        public void SetWATHost(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.WATHOST, listMemberPath));
        }

        public void SetGHostHour(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.GHOSTHOUR, listMemberPath));
        }

        public void SetLHost(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.LHOST, listMemberPath));
        }

        public void SetNSAWAB(string[] listMemberPath)
        {
            m_listTargets.Add(new WabFilterTarget(IWabFilterAttribute.Target.NSAWAB, listMemberPath));
        }

        private bool FindTarget(IWabFilterAttribute.Target targetFilter)
        {
            return GetTarget(targetFilter) != null;
        }

        private IWabFilterAttribute.IWabFilterTarget GetTarget(IWabFilterAttribute.Target targetFilter)
        {
            return m_listTargets.FirstOrDefault(target => target.Target() == targetFilter);
        }

        public bool IsSite() => FindTarget(IWabFilterAttribute.Target.SITE);
        public bool IsSubSite() => FindTarget(IWabFilterAttribute.Target.SUBSITE);
        public bool IsSiteHour() => FindTarget(IWabFilterAttribute.Target.SITEHOUR);
        public bool IsWATSite() => FindTarget(IWabFilterAttribute.Target.WATSITE);
        public bool IsHost() => FindTarget(IWabFilterAttribute.Target.HOST);
        public bool IsGroupHost() => FindTarget(IWabFilterAttribute.Target.GHOST);
        public bool IsGHostHour() => FindTarget(IWabFilterAttribute.Target.GHOSTHOUR);
        public bool IsLHost() => FindTarget(IWabFilterAttribute.Target.LHOST);
        public bool IsWATHost() => FindTarget(IWabFilterAttribute.Target.WATHOST);
        public bool IsNSAWAB() => FindTarget(IWabFilterAttribute.Target.NSAWAB);

        public IWabFilterAttribute.IWabFilterTarget SiteTarget() => GetTarget(IWabFilterAttribute.Target.SITE);
        public IWabFilterAttribute.IWabFilterTarget SubSiteTarget() => GetTarget(IWabFilterAttribute.Target.SUBSITE);
        public IWabFilterAttribute.IWabFilterTarget SiteHourTarget() => GetTarget(IWabFilterAttribute.Target.SITEHOUR);
        public IWabFilterAttribute.IWabFilterTarget WATSiteTarget() => GetTarget(IWabFilterAttribute.Target.WATSITE);
        public IWabFilterAttribute.IWabFilterTarget HostTarget() => GetTarget(IWabFilterAttribute.Target.HOST);
        public IWabFilterAttribute.IWabFilterTarget GroupHostTarget() => GetTarget(IWabFilterAttribute.Target.GHOST);
        public IWabFilterAttribute.IWabFilterTarget LHostTarget() => GetTarget(IWabFilterAttribute.Target.LHOST);
        public IWabFilterAttribute.IWabFilterTarget GHostHourTarget() => GetTarget(IWabFilterAttribute.Target.GHOSTHOUR);
        public IWabFilterAttribute.IWabFilterTarget WATHostTarget() => GetTarget(IWabFilterAttribute.Target.WATHOST);
        public IWabFilterAttribute.IWabFilterTarget NSAWABTarget() => GetTarget(IWabFilterAttribute.Target.NSAWAB);
    }
}

