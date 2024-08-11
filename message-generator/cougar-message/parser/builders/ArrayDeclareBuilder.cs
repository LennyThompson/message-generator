using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Net.CougarMessage.Parser.Builders
{
    public class ArrayDeclareBuilder : CougarMessageBuilderBase
    {
        private string _arraySize;
        private string _define;

        private int _value;
        private char _operator;

        public ArrayDeclareBuilder(IParserObjectBuilder builderParent) : base(builderParent)
        {
        }

        public string ArraySize()
        {
            return _arraySize;
        }

        public override void EnterConstValue(CougarParser.ConstValueContext ctx)
        {
            _arraySize = ctx.GetText();
        }

        public override void EnterConstExpression(CougarParser.ConstExpressionContext ctx)
        {
            SetCurrentBuilder(new ConstExpressionBuilder(this));
        }

        public override void EnterExpressionDefine(CougarParser.ExpressionDefineContext ctx)
        {
            _define = ctx.GetText();
        }

        public override bool OnComplete(IParserObjectBuilder builderChild)
        {
            if (!builderChild.Used())
            {
                if (builderChild is ConstExpressionBuilder constExpression)
                {
                    if (constExpression.HasNumericValue())
                    {
                        _value = constExpression.NumericValue();
                    }
                    else
                    {
                        _define = constExpression.Define();
                        _operator = constExpression.Operator();
                        _value = constExpression.Value();
                    }

                    builderChild.SetUsed();
                }
            }
            return base.OnComplete(builderChild);
        }

        public override void ExitArrayDecl(CougarParser.ArrayDeclContext ctx)
        {
            OnComplete(this);
        }

        public override ObjectCompletion Finalise(Stack<IParserObjectBuilder> stackObjs)
        {
            return null;
        }
    }
}

