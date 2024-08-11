using System;
using System.Collections.Generic;
using System.Text.Json;
using Net.CougarMessage.Parser.Builders.Interfaces;
using Net.CougarMessage.Parser.MessageTypes;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;
using Net.Interfaces;

namespace Net.CougarMessage.Parser.Builders
{
    public class AttributeBuilderBase : CougarMessageBuilderBase, IAttributeBuilder
    {
        protected Attribute m_attrBuild;
        protected List<string> m_listValues;

        protected AttributeBuilderBase(ParserObjectBuilder builderParent) : base(builderParent)
        {
            m_attrBuild = new Attribute();
            m_listValues = new List<string>();
        }

        public Attribute GetAttribute()
        {
            return m_attrBuild;
        }

        public void EnterAttribute_extension(Net.CougarMessage.Grammar.CougarParser.Attribute_extensionContext ctx)
        {
        }

        public void ExitAttribute_extension(Net.CougarMessage.Grammar.CougarParser.Attribute_extensionContext ctx)
        {
        }

        public void EnterAttribute_extension_content_value(Net.CougarMessage.Grammar.CougarParser.Attribute_extension_content_valueContext ctx)
        {
            m_listValues.Add(ctx.GetText());
        }

        public void EnterAttribute_extension_parenthesized_value(Net.CougarMessage.Grammar.CougarParser.Attribute_extension_parenthesized_valueContext ctx)
        {
            m_listValues.Add("(");
        }

        public void ExitAttribute_extension_parenthesized_value(Net.CougarMessage.Grammar.CougarParser.Attribute_extension_parenthesized_valueContext ctx)
        {
            m_listValues.Add(")");
        }

        public ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return new ObjectCompletion
            {
                DoCompletion = (schemaBase) =>
                {
                    // Nothing to do here
                }
            };
        }
    }

    public class ObjectCompletion
    {
        public Action<ISchemaBase> DoCompletion { get; set; }
    }
}

