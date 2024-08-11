using System;
using System.Collections.Generic;
using System.Text.Json;
using Net.Cougar.Parser.DbTypes.Interfaces;
using Net.CougarMessage.Parser.MessageTypes;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;
using Net.Interfaces;

namespace Net.CougarMessage.Parser.Builders
{
    public class AttributeBuilder : AttributeBuilderBase
    {
        public AttributeBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
            m_attrBuild = new Attribute();
        }

        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return new ObjectCompletion
            {
                DoCompletion = (schemaBase) =>
                {
                    // Nothing to do here
                }
            };
        }

        public override void EnterAttibute_key(CougarParser.Attibute_keyContext ctx)
        {
            m_attrBuild.SetName(ctx.GetText());
        }

        public override void EnterAttribute_value(CougarParser.Attribute_valueContext ctx)
        {
            List<string> listValues = new List<string>();
            for (int nIndex = 0; nIndex < ctx.ChildCount; nIndex++)
            {
                listValues.Add(ctx.GetChild(nIndex).GetText());
            }
            m_attrBuild.AddValues(listValues);
        }

        public override void EnterAttribute_extension_content(CougarParser.Attribute_extension_contentContext ctx)
        {
            m_listValues.Clear();
            for (int nIndex = 0; nIndex < ctx.ChildCount; nIndex++)
            {
                m_listValues.Add(ctx.GetChild(nIndex).GetText());
            }
            m_attrBuild.AddValues(m_listValues);
            m_listValues.Clear();
        }

        public override void ExitAttribute(CougarParser.AttributeContext ctx)
        {
            if (m_listValues.Count > 0)
            {
                m_attrBuild.AddValues(m_listValues);
                m_listValues.Clear();
            }
            base.OnComplete(this);
        }

        public override bool OnComplete(ParserObjectBuilder builderChild)
        {
            m_attrBuild = ((AttributeBuilder)builderChild).m_attrBuild;
            builderChild.SetUsed();
            return base.OnComplete(builderChild);
        }
    }
}

