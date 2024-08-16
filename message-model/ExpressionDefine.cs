using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes;

public class ExpressionDefine : Define
{
    private List<String> m_listValues = new();
    private bool m_bEvaluated = false;

    public ExpressionDefine(Define define)
    {
        Name = define.Name;
        Value = define.Value;
        _numericValue = 0;
        m_listValues.Add(Value);
    }

    public List<String> Values
    {
        get => m_listValues;
        set => m_listValues = value;
    }

    public override bool IsExpression  => true;

    public override bool Evaluate(List<IDefine> defines)
    {
        if (!m_bEvaluated)
        {
            _numericValue = m_listValues
                .Select
                (
                    value =>
                    {
                        return defines.Where(define => define.Name == value).FirstOrDefault();
                    }
                )
                .Select
                (
                    define =>
                    {

                        if (define != null)
                        {
                            if (!define.IsNumeric)
                            {
                                define.Evaluate(defines);
                            }

                            return define.NumericValue;
                        }

                        return 0;
                    }
                )
                .Sum();
            m_bEvaluated = true;
        }

        return m_bEvaluated;
    }
}