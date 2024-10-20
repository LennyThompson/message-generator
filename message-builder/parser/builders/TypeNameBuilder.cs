using System.Collections.Generic;
using System.Text.Json;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class TypeNameBuilder : CougarMessageBuilderBase
    {
        private string _name;

        public TypeNameBuilder(CougarMessageBuilderBase? builderParent) : base(builderParent)
        {
        }

        public string TypeName => _name;

        public override void ExitType_name(CougarParser.Type_nameContext ctx)
        {
            _name = ctx.GetText();
            OnComplete(this);
        }

        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            return null;
        }
    }
}