using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CougarMessage.Parser.Builders.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public abstract class CougarMessageBuilderBase(ParserObjectBuilder? builderParent)
        : CougarParserBaseListener, CougarMessageObjectBuilder
    {
        protected ParserObjectBuilder? _builderParent = builderParent;
        protected List<ParserObjectBuilder> _builderChildren = new();
        protected List<ObjectCompletion> _listCompleters = new();
        protected bool _used = false;

        protected void DoChildFinalise(Stack<ParserObjectBuilder> stackObjs)
        {
            stackObjs.Push(this);
            _listCompleters = _builderChildren
                .Select(builder => builder.Finalise(stackObjs))
                .ToList();
            stackObjs.Pop();
        }

        public bool Used
        {
            get => _used;
            set => _used = value;
        }

        public virtual bool OnComplete(ParserObjectBuilder builderChild)
        {
            return _builderParent.OnComplete(builderChild);
        }

        public virtual ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            throw new NotImplementedException();
        }

        public virtual void SetCurrentBuilder(ParserObjectBuilder builderCurrent)
        {
            _builderParent.SetCurrentBuilder(builderCurrent);
        }

        public virtual bool OnSetChildContext(string strContext, string cntxtType)
        {
            return false;
        }

        public virtual void AddModifiers(ParserObjectBuilder builderTarget)
        {
        }
    }
}