using System;
using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.Builders;

namespace Interfaces
{
    public interface ParserObjectBuilder
    {
        public interface IObjectCompletion
        {
            void DoCompletion(SchemaBase sqlSchema);
        }

        bool Used { get; public set; }

        bool OnComplete(ParserObjectBuilder builderChild);
        ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs);

        void SetCurrentBuilder(ParserObjectBuilder builderCurrent);
        bool OnSetChildContext(string strContext, string cntxtType);

        void AddModifiers(ParserObjectBuilder builderTarget);
    }
}