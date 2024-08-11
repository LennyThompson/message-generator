using System;
using System.Collections.Generic;
using System.Text.Json;
using Net.CougarMessage.Grammar;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;
using Net.Interfaces;

namespace Net.CougarMessage.Parser.Builders
{
    public class AlertLevelAttributeBuilder : AttributeBuilderBase
    {
        public AlertLevelAttributeBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
            AttrBuild.Name = "alertlevel";
            AttrBuild.Type = IAttribute.AttributeType.ALERTLEVEL;
        }

        public override void ExitAlertlevel_attribute(CougarParser.Alertlevel_attributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void ExitAlertlevel_list(CougarParser.Alertlevel_listContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void EnterAlertlevel_name(CougarParser.Alertlevel_nameContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }
    }
}