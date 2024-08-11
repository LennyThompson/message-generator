using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Parser.MessageTypes
{
    public class ExpressionDefine : Define
    {
        private List<string> _listValues;
        private bool _evaluated = false;

        public ExpressionDefine(IDefine defineFrom)
        {
            SetName(defineFrom.Name());
            SetValue(defineFrom.Value());
            NumericValue = 0;
            _listValues = new List<string> { Value() };
        }

        public void SetValues(List<string> listValues)
        {
            _listValues = listValues;
        }

        public void SetValue(int value)
        {
            NumericValue += value;
        }

        public bool Evaluate(List<IDefine> listDefines)
        {
            if (!_evaluated)
            {
                try
                {
                    if (_listValues.Count > 0)
                    {
                        NumericValue += _listValues
                            .Select(value => listDefines.FirstOrDefault(define => define.Name() == value))
                            .Select(define =>
                            {
                                if (define != null)
                                {
                                    if (!define.IsNumeric())
                                    {
                                        define.Evaluate(listDefines);
                                    }
                                    return define.NumericValue();
                                }
                                return 0;
                            })
                            .Sum();

                        _evaluated = true;
                    }
                }
                catch (Exception)
                {
                    // Exception handling (empty in the original code)
                }
            }
            return _evaluated;
        }

        public override bool IsExpression()
        {
            return true;
        }
    }
}

