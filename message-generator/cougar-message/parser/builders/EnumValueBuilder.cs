using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Net.CougarMessage.Parser.Builders
{
    public class EnumValueBuilder : CougarMessageBuilderBase
    {
        private EnumValue m_enumValueBuild;

        protected EnumValueBuilder(ParserObjectBuilder builderParent, int nOrdinal)
            : base(builderParent)
        {
            m_enumValueBuild = new EnumValue(nOrdinal);
        }

        public IEnumValue GetValue()
        {
            return m_enumValueBuild;
        }

        public override void ExitEnumValueDefinition(CougarParser.EnumValueDefinitionContext ctx)
        {
            OnComplete(this);
        }

        public override void EnterEnumValueName(CougarParser.EnumValueNameContext ctx)
        {
            m_enumValueBuild.SetName(ctx.GetText());
        }

        public override void EnterEnumValue(CougarParser.EnumValueContext ctx)
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

    public class ObjectCompletion
    {
        public Action<ISchemaBase> DoCompletion { get; set; }
    }

    // Interfaces and other classes are assumed to be defined elsewhere
    public interface IEnumValue { }
    public interface ISchemaBase { }
    public interface ParserObjectBuilder { }
    public class EnumValue : IEnumValue
    {
        public EnumValue(int ordinal) { }
        public void SetName(string name) { }
        public void SetValue(int value) { }
    }
    public abstract class CougarMessageBuilderBase
    {
        protected CougarMessageBuilderBase(ParserObjectBuilder builderParent) { }
        protected void OnComplete(EnumValueBuilder builder) { }
    }
    public class CougarParser
    {
        public class EnumValueDefinitionContext { }
        public class EnumValueNameContext
        {
            public string GetText() { return string.Empty; }
        }
        public class EnumValueContext
        {
            public string GetText() { return string.Empty; }
        }
    }
}

