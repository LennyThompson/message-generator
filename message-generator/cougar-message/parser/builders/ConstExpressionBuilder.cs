using System;
using System.Collections.Generic;
using System.Text.Json;

namespace CougarMessage.Parser.Builders
{
    public class ConstExpressionBuilder : CougarMessageBuilderBase
    {
        private string _define;
        private int _firstValue = 0;
        private int _secondValue = 0;
        private char _operator;

        public ConstExpressionBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
        }

        public bool HasNumericValue()
        {
            return _firstValue > 0 && _secondValue > 0;
        }

        public bool HasOnlyNumericValue()
        {
            return HasNumericValue();
        }

        public int NumericValue()
        {
            if (HasNumericValue())
            {
                return ComputeValue();
            }
            return -1;
        }

        public int Value()
        {
            if (HasNumericValue())
            {
                return NumericValue();
            }
            return _firstValue;
        }

        public bool HasDefine()
        {
            return _define != null;
        }

        public string Define()
        {
            return _define;
        }

        public char Operator()
        {
            return _operator;
        }

        public override void EnterExpression_define(CougarParser.Expression_defineContext ctx)
        {
            _define = ctx.GetText();
        }

        public override void EnterExpression_operator(CougarParser.Expression_operatorContext ctx)
        {
            _operator = ctx.GetText().Trim()[0];
        }

        public override void EnterConst_numeric_value(CougarParser.Const_numeric_valueContext ctx)
        {
            if (_firstValue == 0)
            {
                _firstValue = int.Parse(ctx.GetText().Trim());
            }
            else
            {
                _secondValue = int.Parse(ctx.GetText().Trim());
            }
        }

        public override void ExitConst_expression(CougarParser.Const_expressionContext ctx)
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

        private int ComputeValue()
        {
            if (HasNumericValue())
            {
                switch (_operator)
                {
                    case '+':
                        return _firstValue + _secondValue;
                    case '-':
                        return _firstValue - _secondValue;
                    case '*':
                        return _firstValue * _secondValue;
                    case '/':
                        return _firstValue / _secondValue;
                    default:
                        return -1;
                }
            }
            return -1;
        }
    }
}

