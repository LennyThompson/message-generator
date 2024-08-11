using System.Collections.Generic;
using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;
using Net.Interfaces;

namespace Net.CougarMessage.Parser.Builders
{
    public class ConsumerAttributeBuilder : AttributeBuilderBase
    {
        public ConsumerAttributeBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
            m_attrBuild = new ComponentAttribute
            {
                Name = "consumer",
                Type = IAttribute.AttributeType.CONSUMER
            };
        }

        public override void ExitConsumerAttribute(Net.CougarMessage.Grammar.CougarParser.ConsumerAttributeContext ctx)
        {
            base.OnComplete(this);
        }

        public override void ExitConsumerList(Net.CougarMessage.Grammar.CougarParser.ConsumerListContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }

        public override void EnterConsumerName(Net.CougarMessage.Grammar.CougarParser.ConsumerNameContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }
    }
}