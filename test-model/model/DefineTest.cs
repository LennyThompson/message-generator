using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace test_model.model;

[TestFixture]
public class DefineTest
{
    [Test]
    public void TestDefine()
    {
        // This is the only circumstatnce when the parser would cause a straight define to be built
        // #define USE_MONGO
        Define define = new Define();
        define.Name = "USE_MONGO";

        Assert.That(define.IsNumeric, Is.False);
        Assert.That(define.IsExpression, Is.False);
        Assert.That(define.IsString, Is.False);
    }
    
    [Test]
    public void TestNumericDefine()
    {
        // #define JTP_NUMERIC_ID_SIZE              10
        Define define = new Define();
        define.Name = "JTP_NUMERIC_ID_SIZE";
        NumericDefine numericDefine = new NumericDefine(define);
        numericDefine.NumericValue = 10;
        
        Assert.That(numericDefine.IsNumeric, Is.True);
        Assert.That(numericDefine.NumericValue, Is.EqualTo(10));
        Assert.That(numericDefine.IsExpression, Is.False);
        Assert.That(numericDefine.IsString, Is.False);
    }
    
    [Test]
    public void TestMacroDefine()
    {
        // #define JTP_NUMERIC_ID_SIZE              10
        Define define = new Define();
        define.Name = "JTP_NUMERIC_ID_SIZE";
        
        NumericDefine numericDefine = new NumericDefine(define);
        numericDefine.NumericValue = 10;
        
        // #define JTP_TARGET_ID_SIZE              JTP_NUMERIC_ID_SIZE
        
        define = new Define();
        define.Name = "JTP_TARGET_ID_SIZE";
        ExpressionDefine expressionDefine = new ExpressionDefine(define);
        expressionDefine.Expression = new();
        ExpressionMacroValue macroValue = new ExpressionMacroValue() { Operator = new ExpressionOperator(){ Operator = ExpressionOperator.OperatorType.None}};
        macroValue.MacroValue = "JTP_NUMERIC_ID_SIZE";
        expressionDefine.Expression.Values.Add(macroValue);
        
        Assert.That(expressionDefine.IsNumeric, Is.False);
        Assert.That(expressionDefine.IsExpression, Is.True);
        Assert.That(expressionDefine.IsString, Is.False);

        List<IDefine> listDefines = new List<IDefine>();
        listDefines.Add(numericDefine);
        listDefines.Add(define);
        
        Assert.That(expressionDefine.Evaluate((string strDefineName) => listDefines.FirstOrDefault(define => define.Name == strDefineName)), Is.True);
        Assert.That(expressionDefine.NumericValue, Is.EqualTo(10));
    }
    
    [Test]
    public void TestExpressiohDefine()
    {
        // #define JTP_TYPE_SIZE              10
        Define define = new Define();
        define.Name = "JTP_TYPE_SIZE";
        NumericDefine numericDefine = new NumericDefine(define);
        numericDefine.NumericValue = 10;
        List<IDefine> listDefines = new List<IDefine>();
        listDefines.Add(numericDefine);
        // #define JTP_LESS              5
        Define defineLess = new Define();
        defineLess.Name = "JTP_LESS";
        NumericDefine numericDefineLess = new NumericDefine(defineLess);
        numericDefineLess.NumericValue = 5;
        listDefines.Add(numericDefineLess);
        
        // #define JTP_TARGET_ID_SIZE              JTP_TYPE_SIZE + 1 + 23 - JTP_LESS
        
        define = new Define();
        define.Name = "JTP_TARGET_ID_SIZE";
        ExpressionDefine expressionDefine = new ExpressionDefine(define);
        expressionDefine.Expression = new();
        
        ExpressionMacroValue macroValue = new ExpressionMacroValue()
        {
            MacroValue = "JTP_TYPE_SIZE", 
            Operator = new ExpressionOperator(){ Operator = ExpressionOperator.OperatorType.Plus}
        };
        expressionDefine.Expression.Values.Add(macroValue);
        ExpressionValue numericValue = new ExpressionValue()
        {
            Value = 1, 
            Operator = new ExpressionOperator(){ Operator = ExpressionOperator.OperatorType.Plus}
        };
        expressionDefine.Expression.Values.Add(numericValue);
        numericValue = new ExpressionValue()
        {
            Value = 23, 
            Operator = new ExpressionOperator(){ Operator = ExpressionOperator.OperatorType.Minus}
        };
        expressionDefine.Expression.Values.Add(numericValue);
        macroValue = new ExpressionMacroValue()
        {
            MacroValue = "JTP_LESS", 
            Operator = new ExpressionOperator(){ Operator = ExpressionOperator.OperatorType.None}
        };
        expressionDefine.Expression.Values.Add(macroValue);
        
        Assert.That(expressionDefine.IsNumeric, Is.False);
        Assert.That(expressionDefine.IsExpression, Is.True);
        Assert.That(expressionDefine.IsString, Is.False);

        listDefines.Add(define);
        
        Assert.That(expressionDefine.Evaluate((string strDefineName) => listDefines.FirstOrDefault(define => define.Name == strDefineName)), Is.True);
        Assert.That(expressionDefine.NumericValue, Is.EqualTo(29));
    }
    
    [Test]
    public void TestNumericExpressionDefine()
    {
        // #define JTP_TARGET_TYPE_SIZE            100 + 1
        Define define = new Define();
        define.Name = "JTP_TARGET_TYPE_SIZE";
        
        NumericDefine numericDefine = new NumericDefine(define);

        numericDefine.NumericValue = 100 + 1;
        Assert.That(numericDefine.IsNumeric, Is.True);
        Assert.That(numericDefine.IsExpression, Is.False);
        Assert.That(numericDefine.IsString, Is.False);
    }

    [Test]
    public void TestQuotedStringDefine()
    {
        // #define JTP_QUOTED_STRING            "This is a quoted string"
        
        Define define = new Define();
        define.Name = "JTP_QUOTED_STRING";
        
        QuotedStringDefine quotedDefine = new QuotedStringDefine(define);

        quotedDefine.Value = "his is a quoted string";
        Assert.That(quotedDefine.IsNumeric, Is.False);
        Assert.That(quotedDefine.IsExpression, Is.False);
        Assert.That(quotedDefine.IsString, Is.True);
        
    }
}   