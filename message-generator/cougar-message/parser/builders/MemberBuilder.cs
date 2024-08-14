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
    public class MemberBuilder(MessageBuilder builderParent) : CougarMessageBuilderBase(builderParent)
    {
        private Member m_memberBuild = new();

        public IMember GetMember()
        {
            return m_memberBuild;
        }

        public override void EnterFielddesc_attribute(CougarParser.Fielddesc_attributeContext ctx)
        {
            SetCurrentBuilder(new FielddescAttributeBuilder(this));
        }

        public override void EnterMember_name(CougarParser.Member_nameContext ctx)
        {
            m_memberBuild.SetName(ctx.GetText());
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
            if (!builderChild.Used())
            {
                if (builderChild is IAttributeBuilder attributeBuilder)
                {
                    m_memberBuild.AddAttribute(attributeBuilder.GetAttribute());
                    builderChild.Used = true;
                }
                else if (builderChild is TypeNameBuilder typeNameBuilder)
                {
                    m_memberBuild.SetType(typeNameBuilder.TypeName());
                    builderChild.Used = true;
                }
                else if (builderChild is ArrayDeclareBuilder arrayDeclareBuilder)
                {
                    m_memberBuild.SetArraySize(arrayDeclareBuilder.ArraySize());
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
            return new ObjectCompletion
            {
                DoCompletion = (schemaBase) =>
                {
                    IMessageSchema messageSchema = (IMessageSchema)schemaBase;
                    IMessage messageType = messageSchema.FindMessage(m_memberBuild.Type());
                    if (messageType != null)
                    {
                        m_memberBuild.SetMessageType(messageType);
                    }
                    else
                    {
                        IEnum enumType = messageSchema.FindEnum(m_memberBuild.Type());
                        if (enumType != null)
                        {
                            m_memberBuild.SetEnumType(enumType);
                        }
                    }
                    if (m_memberBuild.IsArray() && m_memberBuild.NumericArraySize() < 0)
                    {
                        IDefine defineArraySize = messageSchema.Defines()
                            .FirstOrDefault(define => define.Name.CompareTo(m_memberBuild.ArraySize()) == 0);
                        if (defineArraySize != null)
                        {
                            if (defineArraySize.IsExpression && defineArraySize.NumericValue == 0)
                            {
                                defineArraySize.Evaluate(messageSchema.Defines());
                            }
                            m_memberBuild.SetArraySizeDefine(defineArraySize);
                        }
                    }
                    foreach (var completer in _listCompleters)
                    {
                        completer.DoCompletion(schemaBase);
                    }
                }
            };
        }
    }
}

