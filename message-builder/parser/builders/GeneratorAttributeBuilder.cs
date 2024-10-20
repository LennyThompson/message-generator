using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class GeneratorAttributeBuilder : AttributeBuilderBase
    {
        public GeneratorAttributeBuilder(ParserObjectBuilder? builderParent) : base(builderParent)
        {
            m_attrBuild = new ComponentAttribute
            {
                Name = "generator",
                Type = IAttribute.AttributeType.Generator
            };
        }

        public override void ExitGenerator_attribute(CougarParser.Generator_attributeContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
            base.OnComplete(this);
        }

        public override void EnterGenerator_name(CougarParser.Generator_nameContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterGenerator_parens(CougarParser.Generator_parensContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }

        public override void ExitGenerator_parens(CougarParser.Generator_parensContext ctx)
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