using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Net.CougarMessage.Parser.Builders
{
    public class ExpressionBuilder : CougarMessageBuilderBase
    {
        private List<string> _expressionValues;
        private int _value;

        public ExpressionBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
            _expressionValues = new List<string>();
            _value = 0;
        }

        public bool HasNumericValue()
        {
            return _value > 0;
        }

        public bool HasOnlyNumericValue()
        {
            return HasNumericValue() && !HasMacroExpression();
        }

        public int NumericValue()
        {
            return _value;
        }

        public bool HasMacroExpression()
        {
            return _expressionValues.Count > 0;
        }

        public List<string> ExpressionValues()
        {
            return _expressionValues;
        }

        public override void EnterNumeric_value(CougarParser.Numeric_valueContext ctx)
        {
            string strValue = ctx.GetText();
            try
            {
                _value += int.Parse(strValue);
            }
            catch (FormatException)
            {
                string[] listNumbers = strValue.Split('+');
                _value = listNumbers.Select(int.Parse).Sum();
            }
        }

        public override void EnterMacro_name(CougarParser.Macro_nameContext ctx)
        {
            _expressionValues.Add(ctx.GetText());
        }

        public override void ExitMacro_expr(CougarParser.Macro_exprContext ctx)
        {
            OnComplete(this);
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
    }

    public interface ISchemaBase { }

    public interface ParserObjectBuilder { }

    public abstract class CougarMessageBuilderBase
    {
        protected CougarMessageBuilderBase(ParserObjectBuilder builderParent) { }

        protected virtual void OnComplete(ExpressionBuilder builder) { }
    }

    public class ObjectCompletion
    {
        public Action<ISchemaBase> DoCompletion { get; set; }
    }

    public class CougarParser
    {
        public class Numeric_valueContext
        {
            public string GetText() { return string.Empty; }
        }

        public class Macro_nameContext
        {
            public string GetText() { return string.Empty; }
        }

        public class Macro_exprContext { }
    }
}

