using System.Collections.Generic;
using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;
using Net.Interfaces;

namespace Net.CougarMessage.Parser.Builders
{
    public class ReasonAttributeBuilder : AttributeBuilderBase
    {
        public ReasonAttributeBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
            AttrBuild.Name = "reason";
            AttrBuild.Type = IAttribute.AttributeType.REASON;
        }

        public override void ExitReasonAttribute(Net.CougarMessage.Grammar.CougarParser.ReasonAttributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void ExitReasonList(Net.CougarMessage.Grammar.CougarParser.ReasonListContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void ExitReasonDetail(Net.CougarMessage.Grammar.CougarParser.ReasonDetailContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void EnterReasonWord(Net.CougarMessage.Grammar.CougarParser.ReasonWordContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }

        public override void EnterReasonNumeric(Net.CougarMessage.Grammar.CougarParser.ReasonNumericContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }

        public override void EnterReasonHex(Net.CougarMessage.Grammar.CougarParser.ReasonHexContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }

        public override void EnterReasonParens(Net.CougarMessage.Grammar.CougarParser.ReasonParensContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void ExitReasonParens(Net.CougarMessage.Grammar.CougarParser.ReasonParensContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }
    }
}

