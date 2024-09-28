using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class ReasonAttributeBuilder : AttributeBuilderBase
    {
        public ReasonAttributeBuilder(ParserObjectBuilder? builderParent) : base(builderParent)
        {
            m_attrBuild.Name = "reason";
            m_attrBuild.Type = IAttribute.AttributeType.Reason;
        }

        public override void ExitReason_attribute(CougarParser.Reason_attributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void ExitReason_list(CougarParser.Reason_listContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }

        public override void ExitReason_detail(CougarParser.Reason_detailContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }

        public override void EnterReason_word(CougarParser.Reason_wordContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterReason_numeric(CougarParser.Reason_numericContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterReason_hex(CougarParser.Reason_hexContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterReason_parens(CougarParser.Reason_parensContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }

        public override void ExitReason_parens(CougarParser.Reason_parensContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }
        
        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return new AttributeObjectCompletion();
        }

    }
}

