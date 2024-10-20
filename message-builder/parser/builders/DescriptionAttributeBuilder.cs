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
            m_attrBuild.Name = "description";
            m_attrBuild.Type = IAttribute.AttributeType.Description;
        }

        public override void ExitDescription_attribute(CougarParser.Description_attributeContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
            base.OnComplete(this);
        }

        public override void ExitDescription_name(CougarParser.Description_nameContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }

        public override void ExitDescription_detail(CougarParser.Description_detailContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }

        public override void EnterDescription_word(CougarParser.Description_wordContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterDescription_numeric(CougarParser.Description_numericContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterDescription_hex(CougarParser.Description_hexContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterDescription_punctuation(CougarParser.Description_punctuationContext ctx)
        {
        }

        public override void EnterDescription_parens(CougarParser.Description_parensContext ctx)
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

