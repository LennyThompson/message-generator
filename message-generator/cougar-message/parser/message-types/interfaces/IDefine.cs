using System;
using System.Collections.Generic;

namespace Net.CougarMessage.Parser.MessageTypes.Interfaces
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

        bool Evaluate(List<IDefine> listDefines);
    }
}