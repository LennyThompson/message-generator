using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class EnumBuilder : CougarMessageBuilderBase
    {
        private EnumDefinition _enumBuild;
        private int _valueOrdinal = 0;

        public EnumBuilder(ParserObjectBuilder? builderParent) : base(builderParent)
        {
            _enumBuild = new EnumDefinition();
        }

        public IEnum GetEnum()
        {
            return _enumBuild;
        }

        public override void ExitEnum_definition(CougarParser.Enum_definitionContext ctx)
        {
            OnComplete(this);
        }

        public override void EnterEnum_name(CougarParser.Enum_nameContext ctx)
        {
            _enumBuild.Name = ctx.GetText();
        }

        public override void EnterEnum_value_definition(CougarParser.Enum_value_definitionContext ctx)
        {
            SetCurrentBuilder(new EnumValueBuilder(this, _valueOrdinal++));
        }

        public override bool OnComplete(ParserObjectBuilder builderChild)
        {
            if (!builderChild.Used)
            {
                if (builderChild is EnumValueBuilder)
                {
                    _builderChildren.Add(builderChild);
                    _enumBuild.AddValue(((EnumValueBuilder)builderChild).GetValue());
                    builderChild.Used = true;
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
}

