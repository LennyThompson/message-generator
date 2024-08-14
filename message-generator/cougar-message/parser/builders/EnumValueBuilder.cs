using System;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class EnumValueBuilder : CougarMessageBuilderBase
    {
        private EnumValue m_enumValueBuild;

        public EnumValueBuilder(ParserObjectBuilder? builderParent, int nOrdinal)
            : base(builderParent)
        {
            m_enumValueBuild = new EnumValue(nOrdinal);
        }

        public IEnumValue GetValue()
        {
            return m_enumValueBuild;
        }

        public override void ExitEnumValueDefinition(global::CougarParser.EnumValueDefinitionContext ctx)
        {
            OnComplete(this);
        }

        public override void EnterEnumValueName(global::CougarParser.EnumValueNameContext ctx)
        {
            m_enumValueBuild.SetName(ctx.GetText());
        }

        public override void EnterEnumValue(global::CougarParser.EnumValueContext ctx)
        {
            string strVal = ctx.GetText();
            if (strVal.ToUpper().IndexOf("0X") == 0)
            {
                m_enumValueBuild.SetValue(Convert.ToInt32(strVal.Substring(2), 16));
            }
            else
            {
                m_enumValueBuild.SetValue(int.Parse(strVal));
            }
        }

        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return new ObjectCompletion
            {
                DoCompletion = (ISchemaBase sqlSchema) => { }
            };
        }
    }
}

