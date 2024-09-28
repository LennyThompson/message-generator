using System;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.Builders.Interfaces;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;
using Attribute = CougarMessage.Parser.MessageTypes.Attribute;

namespace CougarMessage.Parser.Builders
{
    public abstract class AttributeBuilderBase : CougarMessageBuilderBase, IAttributeBuilder
    {
        protected Attribute m_attrBuild;
        protected List<string> m_listValues;

        protected AttributeBuilderBase(ParserObjectBuilder? builderParent) : base(builderParent)
        {
            m_attrBuild = new Attribute();
            m_listValues = new List<string>();
        }

        public IAttribute Attribute => m_attrBuild;

        public override void EnterAttribute_extension(CougarParser.Attribute_extensionContext ctx)
        {
        }

        public override void ExitAttribute_extension(CougarParser.Attribute_extensionContext ctx)
        {
        }

        public override void EnterAttribute_extension_content_value(CougarParser.Attribute_extension_content_valueContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public override void EnterAttribute_extension_parenthesized_value(CougarParser.Attribute_extension_parenthesized_valueContext ctx)
        {
            m_listValues.Add("(");
        }

        public override void ExitAttribute_extension_parenthesized_value(CougarParser.Attribute_extension_parenthesized_valueContext ctx)
        {
            m_listValues.Add(")");
        }
    }
}

