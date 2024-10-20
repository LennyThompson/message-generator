using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class DefineCompletion : ObjectCompletion
    {
        public Define m_defineBuild;
        public void DoCompletion(IMessageSchema sqlSchema)
        {
            if (m_defineBuild is ExpressionDefine expressionDefine)
            {
                expressionDefine.Evaluate((string strName) => ((MessageSchema)sqlSchema).Defines.FirstOrDefault(define => define.Name == strName));
            }
        }
    }
    public class DefineBuilder : CougarMessageBuilderBase
    {
        private Define m_defineBuild = new Define();

        public DefineBuilder(ParserObjectBuilder? builderParent) : base(builderParent)
        {
        }

        public IDefine GetDefine()
        {
            return m_defineBuild;
        }

        public override void ExitMacro_define(CougarParser.Macro_defineContext ctx)
        {
            OnComplete(this);
        }

        public override void EnterDefine_name(CougarParser.Define_nameContext ctx)
        {
            m_defineBuild.Name = ctx.GetText();
        }

        public override void EnterDefine_value(CougarParser.Define_valueContext ctx)
        {
            m_defineBuild.Value = ctx.GetText();
        }

        public override void EnterNumeric_value(CougarParser.Numeric_valueContext ctx)
        {
            NumericDefine numericDefine = new NumericDefine(m_defineBuild);
            string strValue = ctx.GetText();
            try
            {
                numericDefine.NumericValue = int.Parse(strValue);
            }
            catch (FormatException)
            {
                throw;
            }
            m_defineBuild = numericDefine;
        }

        public override void EnterMacro_expr(CougarParser.Macro_exprContext ctx)
        {
            SetCurrentBuilder(new ExpressionBuilder(this));
        }

        public override bool OnComplete(ParserObjectBuilder builderChild)
        {
            if (!builderChild.Used)
            {
                if (builderChild is ExpressionBuilder exprBuild)
                {
                    ExpressionDefine exprDefine = new ExpressionDefine(m_defineBuild);
                    exprDefine.Expression = exprBuild.Expression;
                    m_defineBuild = exprDefine;
                    builderChild.Used = true;
                }
            }
            return base.OnComplete(builderChild);
        }

        public override void EnterMacro_name(CougarParser.Macro_nameContext ctx)
        {
            ExpressionDefine exprDefine = new(m_defineBuild);
            exprDefine.Expression = new Expression();
            ExpressionMacroValue macroValue = new ExpressionMacroValue();
            macroValue.MacroValue = ctx.GetText();
            exprDefine.Expression.Values.Add(macroValue);
            m_defineBuild = exprDefine;
        }

        public override void EnterQuoted_string(CougarParser.Quoted_stringContext ctx)
        {
            m_defineBuild = new QuotedStringDefine(m_defineBuild);
        }

        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return new DefineCompletion() { m_defineBuild = m_defineBuild };
        }
    }
}

