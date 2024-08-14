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
            m_attrBuild.SetName("fielddesc");
            m_attrBuild.SetType(IAttribute.AttributeType.FIELDDESC);
        }

        public override void ExitFielddescAttribute(CougarParser.FielddescAttributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void EnterFielddescName(CougarParser.FielddescNameContext ctx)
        {
            ((FielddescAttribute)m_attrBuild).SetFieldName(ctx.GetText());
        }

        public override void ExitFielddescDesc(CougarParser.FielddescDescContext ctx)
        {
            if (string.IsNullOrEmpty(((FielddescAttribute)m_attrBuild).FieldName()))
            {
                ((FielddescAttribute)m_attrBuild).SetFieldName(ctx.GetText());
            }
        }

        public override void EnterFielddescDetail(CougarParser.FielddescDetailContext ctx)
        {
            m_listValues.Clear();
        }

        public override void ExitFielddescDetail(CougarParser.FielddescDetailContext ctx)
        {
            m_attrBuild.AddValues(m_listValues);
            m_listValues.Clear();
        }

        public override void EnterFielddescWord(CougarParser.FielddescWordContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddescNumeric(CougarParser.FielddescNumericContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddescHex(CougarParser.FielddescHexContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddescExpr(CougarParser.FielddescExprContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddescPunctuation(CougarParser.FielddescPunctuationContext ctx)
        {
        }

        public override void EnterFielddescParens(CougarParser.FielddescParensContext ctx)
        {
            m_attrBuild.AddValues(m_listValues);
            m_listValues.Clear();
        }

        public override void ExitFielddescParens(CougarParser.FielddescParensContext ctx)
        {
            m_attrBuild.AddValues(m_listValues);
            m_listValues.Clear();
        }

        public override void EnterFielddescMoney(CougarParser.FielddescMoneyContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddescQuotedString(CougarParser.FielddescQuotedStringContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }
    }
}

