using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CougarMessage.Parser.Builders.Interfaces;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{

    public class MessageCompletion : ObjectCompletion
    {
        public Message _messageBuild;
        public List<ObjectCompletion> _listCompleters;
        public void DoCompletion(IMessageSchema sqlSchema)
        {
            var messageSchema = (IMessageSchema)sqlSchema;
            var defineMessage = messageSchema.Defines
                .FirstOrDefault(define => define.BaseName != null && define.BaseName.CompareTo(_messageBuild.BaseName) == 0);

            if (defineMessage != null)
            {
                _messageBuild.Define = defineMessage;
            }

            _messageBuild.UpdateVariableLengthArray();

            foreach (var completer in _listCompleters)
            {
                completer.DoCompletion(sqlSchema);
            }
        }
    }
    
    public class MessageBuilder : CougarMessageBuilderBase
    {
        private Message _messageBuild;

        public MessageBuilder(ParserObjectBuilder? builderParent, int nOrdinal) : base(builderParent)
        {
            _messageBuild = new Message(nOrdinal);
        }

        public IMessage Message()
        {
            return _messageBuild;
        }

        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            DoChildFinalise(stackObjs);
            return new MessageCompletion() { _messageBuild = _messageBuild, _listCompleters = _listCompleters };
        }

        public override void EnterDescription_attribute(CougarParser.Description_attributeContext ctx)
        {
            SetCurrentBuilder(new DescriptionAttributeBuilder(this));
        }

        public override void EnterAlertlevel_attribute(CougarParser.Alertlevel_attributeContext ctx)
        {
            SetCurrentBuilder(new AlertLevelAttributeBuilder(this));
        }

        public override void EnterCategory_attribute(CougarParser.Category_attributeContext ctx)
        {
            SetCurrentBuilder(new CategoryAttributeBuilder(this));
        }

        public override void EnterConsumer_attribute(CougarParser.Consumer_attributeContext ctx)
        {
            SetCurrentBuilder(new ConsumerAttributeBuilder(this));
        }

        public override void EnterGenerator_attribute(CougarParser.Generator_attributeContext ctx)
        {
            SetCurrentBuilder(new GeneratorAttributeBuilder(this));
        }

        public override void EnterWabfilter_attribute(CougarParser.Wabfilter_attributeContext ctx)
        {
            SetCurrentBuilder(new WabfilterAttributeBuilder(this));
        }

        public override void EnterReason_attribute(CougarParser.Reason_attributeContext ctx)
        {
            SetCurrentBuilder(new ReasonAttributeBuilder(this));
        }

        public override void EnterAttribute(CougarParser.AttributeContext ctx)
        {
            SetCurrentBuilder(new AttributeBuilder(this));
        }

        public override void EnterMember(CougarParser.MemberContext ctx)
        {
            SetCurrentBuilder(new MemberBuilder(this));
        }

        public override void EnterMessage_name(CougarParser.Message_nameContext ctx)
        {
            _messageBuild.Name = ctx.GetText();
        }

        public override bool OnComplete(ParserObjectBuilder builderChild)
        {
            if (!builderChild.Used)
            {
                if (builderChild is IAttributeBuilder attributeBuilder)
                {
                    _messageBuild.AddAttribute(attributeBuilder.Attribute);
                    builderChild.Used = true;
                }
                else if (builderChild is MemberBuilder memberBuilder)
                {
                    _builderChildren.Add(builderChild);
                    _messageBuild.AddMember(memberBuilder.Member);
                    builderChild.Used = true;
                }
            }
            return base.OnComplete(builderChild);
        }

        public override void ExitMessage_body(CougarParser.Message_bodyContext ctx)
        {
            OnComplete(this);
        }
    }
}

