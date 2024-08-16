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
        define.Value = "JTP_NUMERIC_ID_SIZE";
        
        ExpressionDefine expDefine = new ExpressionDefine(define);

        Assert.That(expDefine.IsNumeric, Is.False);
        Assert.That(expDefine.IsExpression, Is.True);
        Assert.That(expDefine.IsString, Is.False);

        List<IDefine> listDefineds = new List<IDefine>();
        listDefineds.Add(numericDefine);
        listDefineds.Add(expDefine);
        
        Assert.That(expDefine.Evaluate(listDefineds), Is.True);
        Assert.That(expDefine.NumericValue, Is.EqualTo(10));
    }
}   