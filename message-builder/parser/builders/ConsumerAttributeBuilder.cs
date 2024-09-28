using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class ConsumerAttributeBuilder : AttributeBuilderBase
    {
        public ConsumerAttributeBuilder(ParserObjectBuilder? builderParent) : base(builderParent)
        {
            m_attrBuild = new ComponentAttribute
            {
                Name = "consumer",
                Type = IAttribute.AttributeType.Consumer
            };
        }

        public override void ExitConsumer_attribute(CougarParser.Consumer_attributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void ExitConsumer_list(CougarParser.Consumer_listContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }

        public override void EnterConsumer_name(CougarParser.Consumer_nameContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }
        
        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return new AttributeObjectCompletion();
        }

    }
}