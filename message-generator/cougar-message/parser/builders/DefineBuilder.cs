using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class DefineBuilder : CougarMessageBuilderBase
    {
        private Define m_defineBuild;

        public DefineBuilder(ParserObjectBuilder? builderParent) : base(builderParent)
        {
            m_defineBuild = new Define();
        }

        public IDefine GetDefine()
        {
            return m_defineBuild;
        }

        public override void ExitMacroDefine(CougarParser.Macro_defineContext ctx)
        {
            OnComplete(this);
        }

        public override void EnterDefineName(CougarParser.Define_nameContext ctx)
        {
            m_defineBuild.SetName(ctx.GetText());
        }

        public override void EnterDefineValue(CougarParser.Define_valueContext ctx)
        {
            m_defineBuild.SetValue(ctx.GetText());
        }

        public override void EnterNumericValue(CougarParser.Numeric_valueContext ctx)
        {
            NumericDefine numericDefine = new NumericDefine(m_defineBuild);
            string strValue = ctx.GetText();
            try
            {
                numericDefine.SetNumericValue(int.Parse(strValue));
            }
            catch (FormatException)
            {
                string[] listNumbers = strValue.Split('+');
                int nValue = listNumbers.Select(strNumber => int.Parse(strNumber)).Sum();
                numericDefine.SetNumericValue(nValue);
            }
            m_defineBuild = numericDefine;
        }

        public override void EnterMacroExpr(CougarParser.Macro_exprContext ctx)
        {
            SetCurrentBuilder(new ExpressionBuilder(this));
        }

        public override bool OnComplete(ParserObjectBuilder builderChild)
        {
            if (!builderChild.Used())
            {
                if (builderChild is ExpressionBuilder exprBuild)
                {
                    if (exprBuild.HasOnlyNumericValue())
                    {
                        NumericDefine numericDefine = new NumericDefine(m_defineBuild);
                        numericDefine.SetNumericValue(exprBuild.NumericValue());
                        m_defineBuild = numericDefine;
                    }
                    else if (exprBuild.HasMacroExpression())
                    {
                        ExpressionDefine exprDefine = new ExpressionDefine(m_defineBuild);
                        exprDefine.SetValues(exprBuild.ExpressionValues());
                        exprDefine.SetValue(exprBuild.NumericValue());
                        m_defineBuild = exprDefine;
                    }
                    builderChild.Used = true;
                }
            }
            return base.OnComplete(builderChild);
        }

        public override void EnterMacroName(CougarParser.Macro_nameContext ctx)
        {
            m_defineBuild.SetValue(ctx.GetText());
            m_defineBuild = new ExpressionDefine(m_defineBuild);
        }

        public override void EnterQuotedString(CougarParser.Quoted_stringContext ctx)
        {
            m_defineBuild = new QuotedStringDefine(m_defineBuild);
        }

        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return new ObjectCompletion
            {
                DoCompletion = (ISchemaBase schemaBase) =>
                {
                    if (m_defineBuild is ExpressionDefine expressionDefine)
                    {
                        expressionDefine.Evaluate(((MessageSchema)schemaBase).Defines());
                    }
                }
            };
        }
    }
}

