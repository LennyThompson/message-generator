using System;
using System.Collections.Generic;

namespace CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IDefine
    {
        string Name { get; }
        string Value { get; }
        int NumericValue { get; }
        string BaseName { get; }
        bool IsNumeric { get; }
        bool IsExpression { get; }
        bool IsString { get; }

        bool Evaluate(Func<string, IDefine?> fnFindDefine);
    }
}