using System;

namespace CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IWabFilterAttribute : IAttribute
    {
        public enum Target 
        { 
            Site, 
            Subsite, 
            SiteHour, 
            WatSite, 
            Host, 
            Ghost, 
            GhostHour, 
            LHost, 
            WatHost, 
            NsaWab 
        };

        public interface IWabFilterTarget
        {
            Target Target { get; }
            string Name { get; }
            string[]? MemberPath { get; }
            string Filter { get; }
        }

        bool IsSite { get; }
        bool IsSubSite { get; }
        bool IsSiteHour { get; }
        bool IsWATSite { get; }

        bool IsHost { get; }
        bool IsGroupHost { get; }
        bool IsGHostHour { get; }
        bool IsLHost { get; }
        bool IsWATHost { get; }

        bool IsNSAWAB { get; }

        IWabFilterTarget SiteTarget { get; }
        IWabFilterTarget SubSiteTarget { get; }
        IWabFilterTarget SiteHourTarget { get; }
        IWabFilterTarget WATSiteTarget { get; }

        IWabFilterTarget HostTarget { get; }
        IWabFilterTarget GroupHostTarget { get; }
        IWabFilterTarget LHostTarget { get; }
        IWabFilterTarget GHostHourTarget { get; }
        IWabFilterTarget WATHostTarget { get; }

        IWabFilterTarget NSAWABTarget { get; }
    }
}