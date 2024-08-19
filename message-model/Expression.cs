using System.Data;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Parser.MessageTypes;

public class ExpressionOperator
{
    public enum OperatorType { Plus, Minus, Times, Divide, None }
    
    public OperatorType Operator { get; set; } = OperatorType.None;

    public int Apply(int nValue, int nTotal)
    {
        switch (Operator)
        {
            case OperatorType.Plus:
                return nValue + nTotal;
            case OperatorType.Minus:
                return nValue - nTotal;
            case OperatorType.Times:
                return nValue * nTotal;
            case OperatorType.Divide:
                if (nTotal != 0)
                {
                    return nValue / nTotal;
                }
                return 0;
            default:
                break;
        }

        return nValue;
    }

}

public class ExpressionValue
{
    public int Value { get; set; }
    public ExpressionOperator Operator { get; set; } = new ExpressionOperator();

    public virtual int Evaluate(int nTotal, Func<string, IDefine?> fnFindDefine)
    {
        return Operator.Apply(Value, nTotal);
    }

}
public class ExpressionMacroValue : ExpressionValue
{
    public string MacroValue { get; set; } = "";
    public override int Evaluate(int nTotal, Func<string, IDefine?> fnFindDefine)
    {
        if (!string.IsNullOrEmpty(MacroValue))
        {
            IDefine? define = fnFindDefine(MacroValue);
            if (define == null)
            {
                throw new DataException("Define not found");
            }

            if(define.Evaluate(fnFindDefine))
            {
                return Operator.Apply(define.NumericValue, nTotal);
            }
        }
        else
        {
            throw new DataException("Macro value is empty");
        }

        return 0;
    }

}
public class Expression
{
    public List<ExpressionValue> Values { get; } = new List<ExpressionValue>();
}