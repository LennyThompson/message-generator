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
                Type = IAttribute.AttributeType.CONSUMER
            };
        }

        public override void ExitConsumerAttribute(CougarParser.ConsumerAttributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void ExitConsumerList(CougarParser.ConsumerListContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }

        public override void EnterConsumerName(CougarParser.ConsumerNameContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }
    }
}