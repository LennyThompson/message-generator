using System.Collections.Generic;
using System.Text.Json;

namespace Net.CougarMessage.Parser.Builders
{
    public class TypeNameBuilder : CougarMessageBuilderBase
    {
        private string _name;

        public TypeNameBuilder(CougarMessageBuilderBase builderParent) : base(builderParent)
        {
        }

        public string TypeName()
        {
            return _name;
        }

        public override void ExitTypeName(Net.CougarMessage.Grammar.CougarParser.TypeNameContext ctx)
        {
            _name = ctx.GetText();
            OnComplete(this);
        }

        public override ObjectCompletion Finalise(Stack<IParserObjectBuilder> stackObjs)
        {
            return null;
        }
    }
}