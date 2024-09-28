using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class FielddescAttributeBuilder : AttributeBuilderBase
    {
        public FielddescAttributeBuilder(ParserObjectBuilder? builderParent) : base(builderParent)
        {
            m_attrBuild = new FielddescAttribute();
            m_attrBuild.Name = "fielddesc";
            m_attrBuild.Type = IAttribute.AttributeType.FieldDesc;
        }

        public override void ExitFielddesc_attribute(CougarParser.Fielddesc_attributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void EnterFielddesc_name(CougarParser.Fielddesc_nameContext ctx)
        {
            ((FielddescAttribute)m_attrBuild).FieldName = ctx.GetText();
        }

        public override void ExitFielddesc_desc(CougarParser.Fielddesc_descContext ctx)
        {
            if (string.IsNullOrEmpty(((FielddescAttribute)m_attrBuild).FieldName))
            {
                ((FielddescAttribute)m_attrBuild).FieldName = ctx.GetText();
            }
        }

        public override void EnterFielddesc_detail(CougarParser.Fielddesc_detailContext ctx)
        {
            m_listValues.Clear();
        }

        public override void ExitFielddesc_detail(CougarParser.Fielddesc_detailContext ctx)
        {
            m_attrBuild.AddValues(m_listValues);
            m_listValues.Clear();
        }

        public override void EnterFielddesc_word(CougarParser.Fielddesc_wordContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddesc_numeric(CougarParser.Fielddesc_numericContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddesc_hex(CougarParser.Fielddesc_hexContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddesc_expr(CougarParser.Fielddesc_exprContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddesc_punctuation(CougarParser.Fielddesc_punctuationContext ctx)
        {
        }

        public override void EnterFielddesc_parens(CougarParser.Fielddesc_parensContext ctx)
        {
            m_attrBuild.AddValues(m_listValues);
            m_listValues.Clear();
        }

        public override void ExitFielddesc_parens(CougarParser.Fielddesc_parensContext ctx)
        {
            m_attrBuild.AddValues(m_listValues);
            m_listValues.Clear();
        }

        public override void EnterFielddesc_money(CougarParser.Fielddesc_moneyContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddesc_quoted_string(CougarParser.Fielddesc_quoted_stringContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }
        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return new AttributeObjectCompletion();
        }

    }
}

