using System;
using System.Collections.Generic;
using System.Text.Json;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class ArrayDeclareBuilder : CougarMessageBuilderBase
    {
        private string? _arraySize;
        private string? _define;

        private int? _value;
        private char? _operator;

        public ArrayDeclareBuilder(MemberBuilder builderParent) : base(builderParent)
        {
        }

        public string? ArraySize => _arraySize;

        public override void EnterConst_value(CougarParser.Const_valueContext ctx)
        {
            _arraySize = ctx.GetText();
        }

        public override void EnterConst_expression(CougarParser.Const_expressionContext ctx)
        {
            SetCurrentBuilder(new ConstExpressionBuilder(this));
        }

        public override void EnterExpression_define(CougarParser.Expression_defineContext ctx)
        {
            _define = ctx.GetText();
        }

        public override bool OnComplete(ParserObjectBuilder builderChild)
        {
            if (!builderChild.Used)
            {
                if (builderChild is ConstExpressionBuilder constExpression)
                {
                    if (constExpression.HasNumericValue)
                    {
                        _value = constExpression.NumericValue!.Value;
                    }
                    else
                    {
                        _define = constExpression.Define;
                        _operator = constExpression.Operator;
                        _value = constExpression.Value;
                    }

                    builderChild.Used = true;
                }
            }
            return base.OnComplete(builderChild);
        }

        public override void ExitArray_decl(CougarParser.Array_declContext ctx)
        {
            OnComplete(this);
        }

        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return null;
        }
    }
}

