using System;
using System.Collections.Generic;
using System.Text.Json;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class ConstExpressionBuilder : CougarMessageBuilderBase
    {
        private string? _define;
        private int? _firstValue = 0;
        private int? _secondValue = 0;
        private char? _operator;

        public ConstExpressionBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
        }

        public bool HasNumericValue => (_firstValue != null && (_firstValue > 0)) && (_secondValue == null || (_secondValue != null && _secondValue > 0));

        public bool HasOnlyNumericValue => HasNumericValue;

        public int? NumericValue 
        {
            get
            {
                if (HasNumericValue)
                {
                    return ComputeValue();
                }

                return null;
            }
        }

        public int? Value
        {
            get
            {
                if (HasNumericValue)
                {
                    return NumericValue;
                }

                return _firstValue;
            }
        }

        public bool HasDefine => _define != null;

        public string? Define => _define;

        public char? Operator => _operator;

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

        private int? ComputeValue()
        {
            if (HasNumericValue && _operator != null)
            {
                switch (_operator)
                {
                    case '+':
                        return (_firstValue! + _secondValue!)!.Value;
                    case '-':
                        return (_firstValue! - _secondValue!)!.Value;
                    case '*':
                        return (_firstValue! * _secondValue!)!.Value;
                    case '/':
                        return (_firstValue! / _secondValue!)!.Value;
                    default:
                        return -1;
                }
            }
            return Value;
        }
    }
}

