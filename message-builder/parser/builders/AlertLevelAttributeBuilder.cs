using System;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class AlertLevelAttributeBuilder : AttributeBuilderBase
    {
        public AlertLevelAttributeBuilder(ParserObjectBuilder? builderParent) : base(builderParent)
        {
            m_attrBuild.Name = "alertlevel";
            m_attrBuild.Type = IAttribute.AttributeType.AlertLevel;
        }

        public override void ExitAlertlevel_attribute(CougarParser.Alertlevel_attributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void ExitAlertlevel_list(CougarParser.Alertlevel_listContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                Attribute.Values.Add(m_listValues);
            }
        }

        public override void EnterAlertlevel_name(CougarParser.Alertlevel_nameContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }
        
        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return new AttributeObjectCompletion();
        }
    }
}