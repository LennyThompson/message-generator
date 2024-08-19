using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes;

public class ExpressionDefine : Define
{
    private Expression m_expression = new();
    private bool m_bEvaluated = false;

    public ExpressionDefine(Define define)
    {
        Name = define.Name;
        Value = define.Value;
        _numericValue = 0;
    }

    public Expression Expression
    {
        get => m_expression;
        set => m_expression = value;
    }

    public override bool IsExpression  => true;

    public override bool Evaluate(Func<string, IDefine?> fnFindDefine)
    {
        if (!m_bEvaluated)
        {
            _numericValue = EvalValue(0, fnFindDefine);
            m_bEvaluated = true;
        }

        return m_bEvaluated;
    }

    private int EvalValue(int nIndex, Func<string, IDefine?> fnFindDefine)
    {
        if (nIndex == m_expression.Values.Count - 1)
        {
            return m_expression.Values[nIndex].Evaluate(0, fnFindDefine);
        }
        else if (nIndex < m_expression.Values.Count - 1)
        {
            return m_expression.Values[nIndex].Evaluate(EvalValue(nIndex + 1, fnFindDefine), fnFindDefine);
        }

        throw new ArgumentException($"Recursing beyond end of array: {nIndex}");
    }
}