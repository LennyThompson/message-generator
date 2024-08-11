using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Net.Cougar.Parser.DbTypes;
using Net.CougarMessage.Parser.MessageTypes;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;
using Net.Interfaces;

namespace Net.CougarMessage.Parser.Builders
{
    public class EnumBuilder : CougarMessageBuilderBase
    {
        private EnumDefinition _enumBuild;
        private int _valueOrdinal = 0;

        public EnumBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
            _enumBuild = new EnumDefinition();
        }

        public IEnum GetEnum()
        {
            return _enumBuild;
        }

        public override void ExitEnumDefinition(Net.CougarMessage.Grammar.CougarParser.EnumDefinitionContext ctx)
        {
            OnComplete(this);
        }

        public override void EnterEnumName(Net.CougarMessage.Grammar.CougarParser.EnumNameContext ctx)
        {
            _enumBuild.SetName(ctx.GetText());
        }

        public override void EnterEnumValueDefinition(Net.CougarMessage.Grammar.CougarParser.EnumValueDefinitionContext ctx)
        {
            SetCurrentBuilder(new EnumValueBuilder(this, _valueOrdinal++));
        }

        public override bool OnComplete(ParserObjectBuilder builderChild)
        {
            if (!builderChild.Used())
            {
                if (builderChild is EnumValueBuilder)
                {
                    _builderChildren.Add(builderChild);
                    _enumBuild.AddValue(((EnumValueBuilder)builderChild).GetValue());
                    builderChild.SetUsed();
                }
            }
            return base.OnComplete(builderChild);
        }

        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            DoChildFinalise(stackObjs);

            return new ObjectCompletion
            {
                DoCompletion = (ISchemaBase sqlSchema) =>
                {
                    _listCompleters.ForEach(completer => completer.DoCompletion(sqlSchema));
                }
            };
        }
    }

    public class ObjectCompletion
    {
        public Action<ISchemaBase> DoCompletion { get; set; }
    }
}

