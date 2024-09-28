using System;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class EnumValueCompletion : ObjectCompletion
    {
        public void DoCompletion(IMessageSchema sqlSchema)
        {
        }
    }
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

        public override void ExitEnum_value_definition(global::CougarParser.Enum_value_definitionContext ctx)
        {
            OnComplete(this);
        }

        public override void EnterEnum_value_name(global::CougarParser.Enum_value_nameContext ctx)
        {
            m_enumValueBuild.Name = ctx.GetText();
        }

        public override void EnterEnum_value(global::CougarParser.Enum_valueContext ctx)
        {
            string strVal = ctx.GetText();
            if (strVal.ToUpper().IndexOf("0X") == 0)
            {
                m_enumValueBuild.Value = Convert.ToInt32(strVal.Substring(2), 16);
            }
            else
            {
                m_enumValueBuild.Value = int.Parse(strVal);
            }
        }

        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return new EnumValueCompletion();
        }
    }
}

