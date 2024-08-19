using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Antlr4.Runtime.Atn;
using CougarMessage.Parser.MessageTypes;
using Interfaces;
using Expression = CougarMessage.Parser.MessageTypes.Expression;

namespace CougarMessage.Parser.Builders
{
    public class ExpressionBuilder : CougarMessageBuilderBase
    {
        private Expression? _expression;
        private ExpressionValue? _exprValueCurr;

        public ExpressionBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
        }

        public Expression? Expression
        {
            get => _expression;
        }

        public override void EnterNumeric_value(CougarParser.Numeric_valueContext ctx)
        {
            string strValue = ctx.GetText();
            try
            {
                _exprValueCurr = new ExpressionValue();
                _exprValueCurr.Value = int.Parse(strValue);
            }
            catch (FormatException)
            {
                throw;
            }
        }

        public override void EnterMacro_name(CougarParser.Macro_nameContext ctx)
        {
            _exprValueCurr = new ExpressionMacroValue();
            ((ExpressionMacroValue)_exprValueCurr).MacroValue = ctx.GetText();
        }

        public override void ExitMacro_operator(CougarParser.Macro_operatorContext ctx)
        {
            ExpressionOperator exprOperator = new();
            switch (ctx.GetText()[0])
            {
                case '+':
                    exprOperator.Operator = ExpressionOperator.OperatorType.Plus;
                    break;
                case '-':
                    exprOperator.Operator = ExpressionOperator.OperatorType.Minus;
                    break;
                case '*':
                    exprOperator.Operator = ExpressionOperator.OperatorType.Times;
                    break;
                case '/':
                    exprOperator.Operator = ExpressionOperator.OperatorType.Divide;
                    break;
                default:
                    throw new FormatException("Unknown operator: " + ctx.GetText());
            }
            _exprValueCurr.Operator = exprOperator;
            _expression.Values.Add(_exprValueCurr);
            _exprValueCurr = null;
        }
        public override bool OnComplete(ParserObjectBuilder builderChild)
        {
            return base.OnComplete(builderChild);
        }

        public override void ExitMacro_expr(CougarParser.Macro_exprContext ctx)
        {
            if (_exprValueCurr != null)
            {
                ExpressionOperator exprOperator = new();
                exprOperator.Operator = ExpressionOperator.OperatorType.None;
                _exprValueCurr.Operator = exprOperator;
                _expression.Values.Add(_exprValueCurr);
                _exprValueCurr = null;
            }
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

}

