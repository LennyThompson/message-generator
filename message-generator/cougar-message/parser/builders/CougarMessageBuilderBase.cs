using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Net.CougarMessage.Parser.Builders
{
    public abstract class CougarMessageBuilderBase : Net.CougarMessage.Grammar.CougarParserBaseListener, ICougarMessageObjectBuilder
    {
        protected IParserObjectBuilder _builderParent;
        protected List<IParserObjectBuilder> _builderChildren;
        protected List<ObjectCompletion> _listCompleters;
        protected bool _used = false;

        protected CougarMessageBuilderBase(IParserObjectBuilder builderParent)
        {
            _builderParent = builderParent;
            _builderChildren = new List<IParserObjectBuilder>();
        }

        protected void DoChildFinalise(Stack<IParserObjectBuilder> stackObjs)
        {
            stackObjs.Push(this);
            _listCompleters = _builderChildren
                .Select(builder => builder.Finalise(stackObjs))
                .ToList();
            stackObjs.Pop();
        }

        public virtual bool Used()
        {
            return _used;
        }

        public virtual void SetUsed()
        {
            _used = true;
        }

        public virtual bool OnComplete(IParserObjectBuilder builderChild)
        {
            return _builderParent.OnComplete(builderChild);
        }

        public virtual void SetCurrentBuilder(IParserObjectBuilder builderCurrent)
        {
            _builderParent.SetCurrentBuilder(builderCurrent);
        }

        public virtual bool OnSetChildContext(string strContext, string cntxtType)
        {
            return false;
        }

        public virtual void AddModifiers(IParserObjectBuilder builderTarget)
        {
        }
    }
}