using System;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.Builders;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace Interfaces
{
    public interface ObjectCompletion
    {
        void DoCompletion(IMessageSchema sqlSchema);
    }

    public interface ParserObjectBuilder
    {
        public bool Used { get; set; }

        bool OnComplete(ParserObjectBuilder builderChild);
        ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs);

        void SetCurrentBuilder(ParserObjectBuilder builderCurrent);
        bool OnSetChildContext(string strContext, string cntxtType);

        void AddModifiers(ParserObjectBuilder builderTarget);
    }
}