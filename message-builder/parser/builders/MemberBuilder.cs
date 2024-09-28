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
    public class MemberCompletion : ObjectCompletion
    {
        public Member m_memberBuild;
        public List<ObjectCompletion> _listCompleters;
        public void DoCompletion(IMessageSchema sqlSchema)
        {
            IMessageSchema messageSchema = (IMessageSchema)sqlSchema;
            IMessage? messageType = messageSchema.FindMessage(m_memberBuild.Type);
            if (messageType != null)
            {
                m_memberBuild.MessageType = messageType;
            }
            else
            {
                IEnum? enumType = messageSchema.FindEnum(m_memberBuild.Type);
                if (enumType != null)
                {
                    m_memberBuild.EnumType = enumType;
                }
            }
            if (m_memberBuild.IsArray && m_memberBuild.NumericArraySize < 0)
            {
                IDefine? defineArraySize = messageSchema.Defines
                    .FirstOrDefault(define => String.Compare(define.Name, m_memberBuild.ArraySize, StringComparison.Ordinal) == 0);
                if (defineArraySize != null)
                {
                    if (defineArraySize is { IsExpression: true, NumericValue: 0 })
                    {
                        defineArraySize.Evaluate(defName =>
                        {
                            return messageSchema.Defines.FirstOrDefault(def => def.Name == defName);
                        });
                    }
                    m_memberBuild.ArraySizeDefine = defineArraySize;
                }
            }
            foreach (var completer in _listCompleters)
            {
                completer.DoCompletion(sqlSchema);
            }
        }
    }
    public class MemberBuilder(MessageBuilder builderParent) : CougarMessageBuilderBase(builderParent)
    {
        private Member m_memberBuild = new();

        public IMember Member => m_memberBuild;

        public override void EnterFielddesc_attribute(CougarParser.Fielddesc_attributeContext ctx)
        {
            SetCurrentBuilder(new FielddescAttributeBuilder(this));
        }

        public override void EnterMember_name(CougarParser.Member_nameContext ctx)
        {
            m_memberBuild.Name = ctx.GetText();
        }

        public override void EnterType_name(CougarParser.Type_nameContext ctx)
        {
            SetCurrentBuilder(new TypeNameBuilder(this));
        }

        public override void EnterArray_decl(CougarParser.Array_declContext ctx)
        {
            SetCurrentBuilder(new ArrayDeclareBuilder(this));
        }

        public override void ExitMember(CougarParser.MemberContext ctx)
        {
            OnComplete(this);
        }

        public override bool OnComplete(ParserObjectBuilder builderChild)
        {
            if (!builderChild.Used)
            {
                if (builderChild is IAttributeBuilder attributeBuilder)
                {
                    m_memberBuild.AddAttribute(attributeBuilder.Attribute);
                    builderChild.Used = true;
                }
                else if (builderChild is TypeNameBuilder typeNameBuilder)
                {
                    m_memberBuild.Type = typeNameBuilder.TypeName;
                    builderChild.Used = true;
                }
                else if (builderChild is ArrayDeclareBuilder arrayDeclareBuilder)
                {
                    m_memberBuild.ArraySize = arrayDeclareBuilder.ArraySize;
                    builderChild.Used = true;
                }
            }
            return base.OnComplete(builderChild);
        }

        public override ObjectCompletion Finalise(Stack<ParserObjectBuilder> stackObjs)
        {
            if (stackObjs.Peek() is MessageBuilder messageBuilder)
            {
                m_memberBuild.UpdateName(messageBuilder.Message());
            }
            DoChildFinalise(stackObjs);
            return new MemberCompletion() { m_memberBuild = m_memberBuild, _listCompleters = _listCompleters };
        }
    }
}

