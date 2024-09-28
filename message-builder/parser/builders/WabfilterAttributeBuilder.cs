using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class WabfilterAttributeBuilder : AttributeBuilderBase
    {
        private List<string> _listMemberParts;

        public WabfilterAttributeBuilder(ParserObjectBuilder? builderParent) : base(builderParent)
        {
            m_attrBuild = new WabFilterAttribute();
            m_attrBuild.Name = "wabfilter";
            m_attrBuild.Type = IAttribute.AttributeType.WabFilter;
            _listMemberParts = new List<string>();
        }

        public override void ExitWabfilter_attribute(CougarParser.Wabfilter_attributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void ExitWabfilter_site(CougarParser.Wabfilter_siteContext ctx)
        {
            ((WabFilterAttribute)m_attrBuild).SetSite(_listMemberParts.ToArray());
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_subsite(CougarParser.Wabfilter_subsiteContext ctx)
        {
            ((WabFilterAttribute)m_attrBuild).SetSubSite(_listMemberParts.ToArray());
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_host(CougarParser.Wabfilter_hostContext ctx)
        {
            ((WabFilterAttribute)m_attrBuild).SetHost(_listMemberParts.ToArray());
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_lhost(CougarParser.Wabfilter_lhostContext ctx)
        {
            ((WabFilterAttribute)m_attrBuild).SetLHost(_listMemberParts.ToArray());
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_ghost(CougarParser.Wabfilter_ghostContext ctx)
        {
            ((WabFilterAttribute)m_attrBuild).SetGroupHost(_listMemberParts.ToArray());
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_ghosthour(CougarParser.Wabfilter_ghosthourContext ctx)
        {
            ((WabFilterAttribute)m_attrBuild).SetGHostHour(_listMemberParts.ToArray());
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_sitehour(CougarParser.Wabfilter_sitehourContext ctx)
        {
            ((WabFilterAttribute)m_attrBuild).SetSiteHour(_listMemberParts.ToArray());
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_watsite(CougarParser.Wabfilter_watsiteContext ctx)
        {
            ((WabFilterAttribute)m_attrBuild).SetWATSite(_listMemberParts.ToArray());
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_wathost(CougarParser.Wabfilter_wathostContext ctx)
        {
            ((WabFilterAttribute)m_attrBuild).SetWATHost(_listMemberParts.ToArray());
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_nsawab(CougarParser.Wabfilter_nsawabContext ctx)
        {
            ((WabFilterAttribute)m_attrBuild).SetNSAWAB(_listMemberParts.ToArray());
            _listMemberParts.Clear();
        }

        public override void EnterWabfilter_member_part(CougarParser.Wabfilter_member_partContext ctx)
        {
            _listMemberParts.Add(ctx.GetText());
        }
        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return new AttributeObjectCompletion();
        }
    }
}

