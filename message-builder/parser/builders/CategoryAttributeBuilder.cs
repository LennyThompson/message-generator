using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class CategoryAttributeBuilder : AttributeBuilderBase
    {
        public CategoryAttributeBuilder(ParserObjectBuilder? builderParent) : base(builderParent)
        {
            m_attrBuild.Name = "category";
            m_attrBuild.Type = IAttribute.AttributeType.Category;
        }

        public override void ExitCategory_attribute(CougarParser.Category_attributeContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.Values.Add(m_listValues);
                m_listValues.Clear();
            }
            base.OnComplete(this);
        }

        public override void ExitCategory_list(CougarParser.Category_listContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
        }

        public override void ExitCategory_name(CougarParser.Category_nameContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }
        
        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return new AttributeObjectCompletion();
        }

    }
}