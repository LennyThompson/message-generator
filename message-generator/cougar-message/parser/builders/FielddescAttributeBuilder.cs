using System.Collections.Generic;
using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;
using Net.Interfaces;

namespace Net.CougarMessage.Parser.Builders
{
    public class FielddescAttributeBuilder : AttributeBuilderBase
    {
        public FielddescAttributeBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
            m_attrBuild = new FielddescAttribute();
            m_attrBuild.SetName("fielddesc");
            m_attrBuild.SetType(IAttribute.AttributeType.FIELDDESC);
        }

        public override void ExitFielddescAttribute(Net.CougarMessage.Grammar.CougarParser.FielddescAttributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void EnterFielddescName(Net.CougarMessage.Grammar.CougarParser.FielddescNameContext ctx)
        {
            ((FielddescAttribute)m_attrBuild).SetFieldName(ctx.GetText());
        }

        public override void ExitFielddescDesc(Net.CougarMessage.Grammar.CougarParser.FielddescDescContext ctx)
        {
            if (string.IsNullOrEmpty(((FielddescAttribute)m_attrBuild).FieldName()))
            {
                ((FielddescAttribute)m_attrBuild).SetFieldName(ctx.GetText());
            }
        }

        public override void EnterFielddescDetail(Net.CougarMessage.Grammar.CougarParser.FielddescDetailContext ctx)
        {
            m_listValues.Clear();
        }

        public override void ExitFielddescDetail(Net.CougarMessage.Grammar.CougarParser.FielddescDetailContext ctx)
        {
            m_attrBuild.AddValues(m_listValues);
            m_listValues.Clear();
        }

        public override void EnterFielddescWord(Net.CougarMessage.Grammar.CougarParser.FielddescWordContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddescNumeric(Net.CougarMessage.Grammar.CougarParser.FielddescNumericContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddescHex(Net.CougarMessage.Grammar.CougarParser.FielddescHexContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddescExpr(Net.CougarMessage.Grammar.CougarParser.FielddescExprContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddescPunctuation(Net.CougarMessage.Grammar.CougarParser.FielddescPunctuationContext ctx)
        {
        }

        public override void EnterFielddescParens(Net.CougarMessage.Grammar.CougarParser.FielddescParensContext ctx)
        {
            m_attrBuild.AddValues(m_listValues);
            m_listValues.Clear();
        }

        public override void ExitFielddescParens(Net.CougarMessage.Grammar.CougarParser.FielddescParensContext ctx)
        {
            m_attrBuild.AddValues(m_listValues);
            m_listValues.Clear();
        }

        public override void EnterFielddescMoney(Net.CougarMessage.Grammar.CougarParser.FielddescMoneyContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterFielddescQuotedString(Net.CougarMessage.Grammar.CougarParser.FielddescQuotedStringContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }
    }
}

