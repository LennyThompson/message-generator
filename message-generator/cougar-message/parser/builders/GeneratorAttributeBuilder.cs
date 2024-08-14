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
                Type = IAttribute.AttributeType.GENERATOR
            };
        }

        public override void ExitGeneratorAttribute(CougarParser.GeneratorAttributeContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
            base.OnComplete(this);
        }

        public override void EnterGeneratorName(CougarParser.GeneratorNameContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterGeneratorParens(CougarParser.GeneratorParensContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }

        public override void ExitGeneratorParens(CougarParser.GeneratorParensContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }
    }
}