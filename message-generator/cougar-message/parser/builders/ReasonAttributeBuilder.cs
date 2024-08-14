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
            AttrBuild.Name = "reason";
            AttrBuild.Type = IAttribute.AttributeType.REASON;
        }

        public override void ExitReasonAttribute(CougarParser.ReasonAttributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void ExitReasonList(CougarParser.ReasonListContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void ExitReasonDetail(CougarParser.ReasonDetailContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void EnterReasonWord(CougarParser.ReasonWordContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }

        public override void EnterReasonNumeric(CougarParser.ReasonNumericContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }

        public override void EnterReasonHex(CougarParser.ReasonHexContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }

        public override void EnterReasonParens(CougarParser.ReasonParensContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void ExitReasonParens(CougarParser.ReasonParensContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }
    }
}

