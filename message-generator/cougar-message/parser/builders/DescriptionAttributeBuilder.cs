using System;
using System.Collections.Generic;
using System.Text.Json;
using Net.CougarMessage.Parser.Builders.Interfaces;
using Net.CougarMessage.Parser.MessageTypes;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;
using Net.Interfaces;

namespace Net.CougarMessage.Parser.Builders
{
    public class DescriptionAttributeBuilder : AttributeBuilderBase
    {
        public DescriptionAttributeBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
            AttrBuild.Name = "description";
            AttrBuild.Type = IAttribute.AttributeType.DESCRIPTION;
        }

        public override void ExitDescriptionAttribute(Net.CougarMessage.Grammar.CougarParser.Description_attributeContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
            base.OnComplete(this);
        }

        public override void ExitDescriptionName(Net.CougarMessage.Grammar.CougarParser.Description_nameContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void ExitDescriptionDetail(Net.CougarMessage.Grammar.CougarParser.Description_detailContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void EnterDescriptionWord(Net.CougarMessage.Grammar.CougarParser.Description_wordContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }

        public override void EnterDescriptionNumeric(Net.CougarMessage.Grammar.CougarParser.Description_numericContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }

        public override void EnterDescriptionHex(Net.CougarMessage.Grammar.CougarParser.Description_hexContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }

        public override void EnterDescriptionPunctuation(Net.CougarMessage.Grammar.CougarParser.Description_punctuationContext ctx)
        {
        }

        public override void EnterDescriptionParens(Net.CougarMessage.Grammar.CougarParser.Description_parensContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }
    }
}

