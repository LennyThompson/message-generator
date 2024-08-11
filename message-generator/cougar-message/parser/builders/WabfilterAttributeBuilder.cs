using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Net.CougarMessage.Grammar;
using Net.CougarMessage.Parser.MessageTypes;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;
using Net.Interfaces;

namespace Net.CougarMessage.Parser.Builders
{
    public class WabfilterAttributeBuilder : AttributeBuilderBase
    {
        private List<string> _listMemberParts;

        public WabfilterAttributeBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
            _attrBuild = new WabFilterAttribute();
            _attrBuild.Name = "wabfilter";
            _attrBuild.Type = IAttribute.AttributeType.WABFILTER;
            _listMemberParts = new List<string>();
        }

        public override void ExitWabfilter_attribute(CougarParser.Wabfilter_attributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void ExitWabfilter_site(CougarParser.Wabfilter_siteContext ctx)
        {
            ((WabFilterAttribute)_attrBuild).Site = _listMemberParts.ToArray();
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_subsite(CougarParser.Wabfilter_subsiteContext ctx)
        {
            ((WabFilterAttribute)_attrBuild).SubSite = _listMemberParts.ToArray();
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_host(CougarParser.Wabfilter_hostContext ctx)
        {
            ((WabFilterAttribute)_attrBuild).Host = _listMemberParts.ToArray();
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_lhost(CougarParser.Wabfilter_lhostContext ctx)
        {
            ((WabFilterAttribute)_attrBuild).LHost = _listMemberParts.ToArray();
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_ghost(CougarParser.Wabfilter_ghostContext ctx)
        {
            ((WabFilterAttribute)_attrBuild).GroupHost = _listMemberParts.ToArray();
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_ghosthour(CougarParser.Wabfilter_ghosthourContext ctx)
        {
            ((WabFilterAttribute)_attrBuild).GHostHour = _listMemberParts.ToArray();
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_sitehour(CougarParser.Wabfilter_sitehourContext ctx)
        {
            ((WabFilterAttribute)_attrBuild).SiteHour = _listMemberParts.ToArray();
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_watsite(CougarParser.Wabfilter_watsiteContext ctx)
        {
            ((WabFilterAttribute)_attrBuild).WATSite = _listMemberParts.ToArray();
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_wathost(CougarParser.Wabfilter_wathostContext ctx)
        {
            ((WabFilterAttribute)_attrBuild).WATHost = _listMemberParts.ToArray();
            _listMemberParts.Clear();
        }

        public override void ExitWabfilter_nsawab(CougarParser.Wabfilter_nsawabContext ctx)
        {
            ((WabFilterAttribute)_attrBuild).NSAWAB = _listMemberParts.ToArray();
            _listMemberParts.Clear();
        }

        public override void EnterWabfilter_member_part(CougarParser.Wabfilter_member_partContext ctx)
        {
            _listMemberParts.Add(ctx.GetText());
        }
    }
}

