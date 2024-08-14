using System;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.Builders.Interfaces;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class DescriptionAttributeBuilder : AttributeBuilderBase
    {
        public DescriptionAttributeBuilder(ParserObjectBuilder? builderParent) : base(builderParent)
        {
            AttrBuild.Name = "description";
            AttrBuild.Type = IAttribute.AttributeType.DESCRIPTION;
        }

        public override void ExitDescriptionAttribute(CougarParser.Description_attributeContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
            base.OnComplete(this);
        }

        public override void ExitDescriptionName(CougarParser.Description_nameContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void ExitDescriptionDetail(CougarParser.Description_detailContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void EnterDescriptionWord(CougarParser.Description_wordContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }

        public override void EnterDescriptionNumeric(CougarParser.Description_numericContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }

        public override void EnterDescriptionHex(CougarParser.Description_hexContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }

        public override void EnterDescriptionPunctuation(CougarParser.Description_punctuationContext ctx)
        {
        }

        public override void EnterDescriptionParens(CougarParser.Description_parensContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }
    }
}

