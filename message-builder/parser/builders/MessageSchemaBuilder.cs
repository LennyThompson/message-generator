using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class SchemaCompletion : ObjectCompletion
    {
        public List<ObjectCompletion> _listCompleters;
        public void DoCompletion(IMessageSchema sqlSchema)
        {
            _listCompleters.ForEach(completer => completer.DoCompletion(sqlSchema));
        }
    }
    public class MessageSchemaBuilder(ParserObjectBuilder? builderParent) : CougarMessageBuilderBase(builderParent)
    {
        private MessageSchema _messageSchema = new();
        private int _ordinal = 1;

        public MessageSchema Schema()
        {
            return _messageSchema;
        }

        public override bool OnComplete(ParserObjectBuilder builderChild)
        {
            if (!builderChild.Used)
            {
                if (builderChild is MessageBuilder messageBuilder)
                {
                    _builderChildren.Add(builderChild);
                    _messageSchema.AddMessage(messageBuilder.Message());
                    builderChild.Used = true;
                }
                else if (builderChild is DefineBuilder defineBuilder)
                {
                    _builderChildren.Add(builderChild);
                    _messageSchema.AddDefine(defineBuilder.GetDefine());
                    builderChild.Used = true;
                }
                else if (builderChild is EnumBuilder enumBuilder)
                {
                    _builderChildren.Add(builderChild);
                    _messageSchema.AddEnum(enumBuilder.GetEnum());
                    builderChild.Used = true;
                }
            }
            return base.OnComplete(builderChild);
        }

        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            DoChildFinalise(stackObjs);
            return new SchemaCompletion() { _listCompleters = _listCompleters };
        }

        public override void EnterMessage_body(CougarParser.Message_bodyContext ctx)
        {
            SetCurrentBuilder(new MessageBuilder(this, _ordinal++));
        }

        public override void EnterMacro_define(CougarParser.Macro_defineContext ctx)
        {
            SetCurrentBuilder(new DefineBuilder(this));
        }

        public override void ExitCougar_messages(CougarParser.Cougar_messagesContext ctx)
        {
            Stack<ParserObjectBuilder> stackObjs = new Stack<ParserObjectBuilder>();
            Finalise(stackObjs).DoCompletion(_messageSchema);
        }

        public override void EnterEnum_definition(CougarParser.Enum_definitionContext ctx)
        {
            SetCurrentBuilder(new EnumBuilder(this));
        }
    }
}

