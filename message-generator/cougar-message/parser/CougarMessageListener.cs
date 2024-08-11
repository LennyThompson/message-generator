using System;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using net.cougar_message.parser.builders;
using net.cougar_message.parser.builders.interfaces;
using net.cougar_message.parser.message_types.interfaces;
using net.interfaces;

namespace net.cougar_message.parser
{
    public class CougarMessageListener : net.cougar_message.grammar.CougarParserBaseListener
    {
        private Stack<CougarMessageObjectBuilder> m_stackBuilders;

        public static CougarMessageListener RunGrammar(TextReader readerFrom, CougarMessageListener msgListener)
        {
            if (msgListener == null)
            {
                msgListener = new CougarMessageListener();
            }

            var input = CharStreams.fromReader(readerFrom);
            var lexer = new net.cougar_message.grammar.CougarLexer(input);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new net.cougar_message.grammar.CougarParser(tokenStream);

            var walker = new ParseTreeWalker();
            parser.BuildParseTree = true;
            var parseTree = parser.cougar_messages();

            walker.Walk(msgListener, parseTree);

            return msgListener;
        }

        public CougarMessageListener()
        {
            m_stackBuilders = new Stack<CougarMessageObjectBuilder>();
            m_stackBuilders.Push(
                new MessageSchemaBuilder(
                    new CougarMessageBuilderBase(null)
                    {
                        Used = true,
                        SetUsed = () => { },
                        OnComplete = (builderChild) =>
                        {
                            if (m_stackBuilders.Count == 1)
                            {
                                throw new InvalidOperationException("Builder stack will be emptied by pop!");
                            }

                            m_stackBuilders.Pop();
                            return true;
                        },
                        Finalise = (stackObjs) => null,
                        SetCurrentBuilder = (builderCurrent) =>
                        {
                            m_stackBuilders.Push((CougarMessageObjectBuilder)builderCurrent);
                        },
                        OnSetChildContext = (strContext, cntxtType) => false,
                        AddModifiers = (builderTarget) => { }
                    }
                )
            );
        }

        public IMessageSchema Schema()
        {
            return ((MessageSchemaBuilder)m_stackBuilders.Peek()).Schema();
        }

        public override void EnterCougar_messages(net.cougar_message.grammar.CougarParser.Cougar_messagesContext ctx)
        {
            m_stackBuilders.Peek().EnterCougar_messages(ctx);
        }

        public override void ExitCougar_messages(net.cougar_message.grammar.CougarParser.Cougar_messagesContext ctx)
        {
            m_stackBuilders.Peek().ExitCougar_messages(ctx);
        }

        public override void EnterMessage_body(net.cougar_message.grammar.CougarParser.Message_bodyContext ctx)
        {
            m_stackBuilders.Peek().EnterMessage_body(ctx);
        }

        public override void ExitMessage_body(net.cougar_message.grammar.CougarParser.Message_bodyContext ctx)
        {
            m_stackBuilders.Peek().ExitMessage_body(ctx);
        }

        public override void EnterEnum_definition(net.cougar_message.grammar.CougarParser.Enum_definitionContext ctx)
        {
            m_stackBuilders.Peek().EnterEnum_definition(ctx);
        }

        public override void ExitEnum_definition(net.cougar_message.grammar.CougarParser.Enum_definitionContext ctx)
        {
            m_stackBuilders.Peek().ExitEnum_definition(ctx);
        }

        public override void EnterEnum_name(net.cougar_message.grammar.CougarParser.Enum_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterEnum_name(ctx);
        }

        public override void ExitEnum_name(net.cougar_message.grammar.CougarParser.Enum_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitEnum_name(ctx);
        }

        public override void EnterEnum_value_definition(
            net.cougar_message.grammar.CougarParser.Enum_value_definitionContext ctx)
        {
            m_stackBuilders.Peek().EnterEnum_value_definition(ctx);
        }

        public override void ExitEnum_value_definition(
            net.cougar_message.grammar.CougarParser.Enum_value_definitionContext ctx)
        {
            m_stackBuilders.Peek().ExitEnum_value_definition(ctx);
        }

        public override void EnterEnum_value_name(net.cougar_message.grammar.CougarParser.Enum_value_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterEnum_value_name(ctx);
        }

        public override void ExitEnum_value_name(net.cougar_message.grammar.CougarParser.Enum_value_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitEnum_value_name(ctx);
        }

        public override void EnterEnum_value(net.cougar_message.grammar.CougarParser.Enum_valueContext ctx)
        {
            m_stackBuilders.Peek().EnterEnum_value(ctx);
        }

        public override void ExitEnum_value(net.cougar_message.grammar.CougarParser.Enum_valueContext ctx)
        {
            m_stackBuilders.Peek().ExitEnum_value(ctx);
        }

        public override void EnterAttribute(net.cougar_message.grammar.CougarParser.AttributeContext ctx)
        {
            m_stackBuilders.Peek().EnterAttribute(ctx);
        }

        public override void ExitAttribute(net.cougar_message.grammar.CougarParser.AttributeContext ctx)
        {
            m_stackBuilders.Peek().ExitAttribute(ctx);
        }

        public override void EnterMember(net.cougar_message.grammar.CougarParser.MemberContext ctx)
        {
            m_stackBuilders.Peek().EnterMember(ctx);
        }

        public override void ExitMember(net.cougar_message.grammar.CougarParser.MemberContext ctx)
        {
            m_stackBuilders.Peek().ExitMember(ctx);
        }

        public override void EnterAttibute_key(net.cougar_message.grammar.CougarParser.Attibute_keyContext ctx)
        {
            m_stackBuilders.Peek().EnterAttibute_key(ctx);
        }

        public override void ExitAttibute_key(net.cougar_message.grammar.CougarParser.Attibute_keyContext ctx)
        {
            m_stackBuilders.Peek().ExitAttibute_key(ctx);
        }

        public override void EnterAttribute_value(net.cougar_message.grammar.CougarParser.Attribute_valueContext ctx)
        {
            m_stackBuilders.Peek().EnterAttribute_value(ctx);
        }

        public override void ExitAttribute_value(net.cougar_message.grammar.CougarParser.Attribute_valueContext ctx)
        {
            m_stackBuilders.Peek().ExitAttribute_value(ctx);
        }

        public override void EnterMember_name(net.cougar_message.grammar.CougarParser.Member_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterMember_name(ctx);
        }

        public override void ExitMember_name(net.cougar_message.grammar.CougarParser.Member_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitMember_name(ctx);
        }

        public override void EnterType_name(net.cougar_message.grammar.CougarParser.Type_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterType_name(ctx);
        }

        public override void ExitType_name(net.cougar_message.grammar.CougarParser.Type_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitType_name(ctx);
        }

        public override void EnterMessage_name(net.cougar_message.grammar.CougarParser.Message_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterMessage_name(ctx);
        }

        public override void ExitMessage_name(net.cougar_message.grammar.CougarParser.Message_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitMessage_name(ctx);
        }

        public override void EnterConst_value(net.cougar_message.grammar.CougarParser.Const_valueContext ctx)
        {
            m_stackBuilders.Peek().EnterConst_value(ctx);
        }

        public override void ExitConst_value(net.cougar_message.grammar.CougarParser.Const_valueContext ctx)
        {
            m_stackBuilders.Peek().ExitConst_value(ctx);
        }

        public override void EnterConst_expression(net.cougar_message.grammar.CougarParser.Const_expressionContext ctx)
        {
            m_stackBuilders.Peek().EnterConst_expression(ctx);
        }

        public override void ExitConst_expression(net.cougar_message.grammar.CougarParser.Const_expressionContext ctx)
        {
            m_stackBuilders.Peek().ExitConst_expression(ctx);
        }

        public override void EnterExpression_define(
            net.cougar_message.grammar.CougarParser.Expression_defineContext ctx)
        {
            m_stackBuilders.Peek().EnterExpression_define(ctx);
        }

        public override void ExitExpression_define(net.cougar_message.grammar.CougarParser.Expression_defineContext ctx)
        {
            m_stackBuilders.Peek().ExitExpression_define(ctx);
        }

        public override void EnterExpression_operator(
            net.cougar_message.grammar.CougarParser.Expression_operatorContext ctx)
        {
            m_stackBuilders.Peek().EnterExpression_operator(ctx);
        }

        public override void ExitExpression_operator(
            net.cougar_message.grammar.CougarParser.Expression_operatorContext ctx)
        {
            m_stackBuilders.Peek().ExitExpression_operator(ctx);
        }

        public override void EnterConst_numeric_value(
            net.cougar_message.grammar.CougarParser.Const_numeric_valueContext ctx)
        {
            m_stackBuilders.Peek().EnterConst_numeric_value(ctx);
        }

        public override void ExitConst_numeric_value(
            net.cougar_message.grammar.CougarParser.Const_numeric_valueContext ctx)
        {
            m_stackBuilders.Peek().ExitConst_numeric_value(ctx);
        }

        public override void EnterAttribute_extension(
            net.cougar_message.grammar.CougarParser.Attribute_extensionContext ctx)
        {
            m_stackBuilders.Peek().EnterAttribute_extension(ctx);
        }

        public override void ExitAttribute_extension(
            net.cougar_message.grammar.CougarParser.Attribute_extensionContext ctx)
        {
            m_stackBuilders.Peek().ExitAttribute_extension(ctx);
        }

        public override void EnterAttribute_extension_content(
            net.cougar_message.grammar.CougarParser.Attribute_extension_contentContext ctx)
        {
            m_stackBuilders.Peek().EnterAttribute_extension_content(ctx);
        }

        public override void ExitAttribute_extension_content(
            net.cougar_message.grammar.CougarParser.Attribute_extension_contentContext ctx)
        {
            m_stackBuilders.Peek().ExitAttribute_extension_content(ctx);
        }

        public override void EnterAttribute_extension_content_value(
            net.cougar_message.grammar.CougarParser.Attribute_extension_content_valueContext ctx)
        {
            m_stackBuilders.Peek().EnterAttribute_extension_content_value(ctx);
        }

        public override void ExitAttribute_extension_content_value(
            net.cougar_message.grammar.CougarParser.Attribute_extension_content_valueContext ctx)
        {
            m_stackBuilders.Peek().ExitAttribute_extension_content_value(ctx);
        }

        public override void EnterAttribute_extension_parenthesized_value(
            net.cougar_message.grammar.CougarParser.Attribute_extension_parenthesized_valueContext ctx)
        {
            m_stackBuilders.Peek().EnterAttribute_extension_parenthesized_value(ctx);
        }

        public override void ExitAttribute_extension_parenthesized_value(
            net.cougar_message.grammar.CougarParser.Attribute_extension_parenthesized_valueContext ctx)
        {
            m_stackBuilders.Peek().ExitAttribute_extension_parenthesized_value(ctx);
        }

        public override void EnterParenthesized_value(
            net.cougar_message.grammar.CougarParser.Parenthesized_valueContext ctx)
        {
            m_stackBuilders.Peek().EnterParenthesized_value(ctx);
        }

        public override void ExitParenthesized_value(
            net.cougar_message.grammar.CougarParser.Parenthesized_valueContext ctx)
        {
            m_stackBuilders.Peek().ExitParenthesized_value(ctx);
        }

        public override void EnterPart_attribute_value(
            net.cougar_message.grammar.CougarParser.Part_attribute_valueContext ctx)
        {
            m_stackBuilders.Peek().EnterPart_attribute_value(ctx);
        }

        public override void ExitPart_attribute_value(
            net.cougar_message.grammar.CougarParser.Part_attribute_valueContext ctx)
        {
            m_stackBuilders.Peek().ExitPart_attribute_value(ctx);
        }

        public override void EnterMacro_block(net.cougar_message.grammar.CougarParser.Macro_blockContext ctx)
        {
            m_stackBuilders.Peek().EnterMacro_block(ctx);
        }

        public override void ExitMacro_block(net.cougar_message.grammar.CougarParser.Macro_blockContext ctx)
        {
            m_stackBuilders.Peek().ExitMacro_block(ctx);
        }

        public override void EnterMacro(net.cougar_message.grammar.CougarParser.MacroContext ctx)
        {
            m_stackBuilders.Peek().EnterMacro(ctx);
        }

        public override void ExitMacro(net.cougar_message.grammar.CougarParser.MacroContext ctx)
        {
            m_stackBuilders.Peek().ExitMacro(ctx);
        }

        public override void EnterMacro_define(net.cougar_message.grammar.CougarParser.Macro_defineContext ctx)
        {
            m_stackBuilders.Peek().EnterMacro_define(ctx);
        }

        public override void ExitMacro_define(net.cougar_message.grammar.CougarParser.Macro_defineContext ctx)
        {
            m_stackBuilders.Peek().ExitMacro_define(ctx);
        }

        public override void EnterDefine_name(net.cougar_message.grammar.CougarParser.Define_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterDefine_name(ctx);
        }

        public override void ExitDefine_name(net.cougar_message.grammar.CougarParser.Define_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitDefine_name(ctx);
        }

        public override void EnterDefine_value(net.cougar_message.grammar.CougarParser.Define_valueContext ctx)
        {
            m_stackBuilders.Peek().EnterDefine_value(ctx);
        }

        public override void ExitDefine_value(net.cougar_message.grammar.CougarParser.Define_valueContext ctx)
        {
            m_stackBuilders.Peek().ExitDefine_value(ctx);
        }

        public override void EnterNumeric_value(net.cougar_message.grammar.CougarParser.Numeric_valueContext ctx)
        {
            m_stackBuilders.Peek().EnterNumeric_value(ctx);
        }

        public override void ExitNumeric_value(net.cougar_message.grammar.CougarParser.Numeric_valueContext ctx)
        {
            m_stackBuilders.Peek().ExitNumeric_value(ctx);
        }

        public override void EnterMacro_expr(net.cougar_message.grammar.CougarParser.Macro_exprContext ctx)
        {
            m_stackBuilders.Peek().EnterMacro_expr(ctx);
        }

        public override void ExitMacro_expr(net.cougar_message.grammar.CougarParser.Macro_exprContext ctx)
        {
            m_stackBuilders.Peek().ExitMacro_expr(ctx);
        }

        public override void EnterMacro_name(net.cougar_message.grammar.CougarParser.Macro_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterMacro_name(ctx);
        }

        public override void EnterQuoted_string(net.cougar_message.grammar.CougarParser.Quoted_stringContext ctx)
        {
            m_stackBuilders.Peek().EnterQuoted_string(ctx);
        }

        public override void EnterMacro_include(net.cougar_message.grammar.CougarParser.Macro_includeContext ctx)
        {
            m_stackBuilders.Peek().EnterMacro_include(ctx);
        }

        public override void ExitMacro_include(net.cougar_message.grammar.CougarParser.Macro_includeContext ctx)
        {
            m_stackBuilders.Peek().ExitMacro_include(ctx);
        }

        public override void EnterInclude_name(net.cougar_message.grammar.CougarParser.Include_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterInclude_name(ctx);
        }

        public override void ExitInclude_name(net.cougar_message.grammar.CougarParser.Include_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitInclude_name(ctx);
        }

        public override void EnterMacro_ifndef(net.cougar_message.grammar.CougarParser.Macro_ifndefContext ctx)
        {
            m_stackBuilders.Peek().EnterMacro_ifndef(ctx);
        }

        public override void ExitMacro_ifndef(net.cougar_message.grammar.CougarParser.Macro_ifndefContext ctx)
        {
            m_stackBuilders.Peek().ExitMacro_ifndef(ctx);
        }

        public override void EnterMacro_if(net.cougar_message.grammar.CougarParser.Macro_ifContext ctx)
        {
            m_stackBuilders.Peek().EnterMacro_if(ctx);
        }

        public override void ExitMacro_if(net.cougar_message.grammar.CougarParser.Macro_ifContext ctx)
        {
            m_stackBuilders.Peek().ExitMacro_if(ctx);
        }

        public override void EnterMacro_clause(net.cougar_message.grammar.CougarParser.Macro_clauseContext ctx)
        {
            m_stackBuilders.Peek().EnterMacro_clause(ctx);
        }

        public override void ExitMacro_clause(net.cougar_message.grammar.CougarParser.Macro_clauseContext ctx)
        {
            m_stackBuilders.Peek().ExitMacro_clause(ctx);
        }

        public override void EnterMacro_test(net.cougar_message.grammar.CougarParser.Macro_testContext ctx)
        {
            m_stackBuilders.Peek().EnterMacro_test(ctx);
        }

        public override void ExitMacro_test(net.cougar_message.grammar.CougarParser.Macro_testContext ctx)
        {
            m_stackBuilders.Peek().ExitMacro_test(ctx);
        }

        public override void EnterMacro_endif(net.cougar_message.grammar.CougarParser.Macro_endifContext ctx)
        {
            m_stackBuilders.Peek().EnterMacro_endif(ctx);
        }

        public override void ExitMacro_endif(net.cougar_message.grammar.CougarParser.Macro_endifContext ctx)
        {
            m_stackBuilders.Peek().ExitMacro_endif(ctx);
        }

        public override void EnterMacro_pragma(net.cougar_message.grammar.CougarParser.Macro_pragmaContext ctx)
        {
            m_stackBuilders.Peek().EnterMacro_pragma(ctx);
        }

        public override void ExitMacro_pragma(net.cougar_message.grammar.CougarParser.Macro_pragmaContext ctx)
        {
            m_stackBuilders.Peek().ExitMacro_pragma(ctx);
        }

        public override void EnterPragma_name(net.cougar_message.grammar.CougarParser.Pragma_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterPragma_name(ctx);
        }

        public override void ExitPragma_name(net.cougar_message.grammar.CougarParser.Pragma_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitPragma_name(ctx);
        }

        public override void EnterPragma_type(net.cougar_message.grammar.CougarParser.Pragma_typeContext ctx)
        {
            m_stackBuilders.Peek().EnterPragma_type(ctx);
        }

        public override void ExitPragma_type(net.cougar_message.grammar.CougarParser.Pragma_typeContext ctx)
        {
            m_stackBuilders.Peek().ExitPragma_type(ctx);
        }

        public override void EnterArray_decl(net.cougar_message.grammar.CougarParser.Array_declContext ctx)
        {
            m_stackBuilders.Peek().EnterArray_decl(ctx);
        }

        public override void ExitArray_decl(net.cougar_message.grammar.CougarParser.Array_declContext ctx)
        {
            m_stackBuilders.Peek().ExitArray_decl(ctx);
        }

        public override void EnterDescription_attribute(
            net.cougar_message.grammar.CougarParser.Description_attributeContext ctx)
        {
            m_stackBuilders.Peek().EnterDescription_attribute(ctx);
        }

        public override void ExitDescription_attribute(CougarParser.Description_attributeContext ctx)
        {
            m_stackBuilders.Peek().ExitDescription_attribute(ctx);
        }

        public override void EnterDescription_name(CougarParser.Description_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterDescription_name(ctx);
        }

        public override void ExitDescription_name(CougarParser.Description_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitDescription_name(ctx);
        }

        public override void EnterDescription_data(CougarParser.Description_dataContext ctx)
        {
            m_stackBuilders.Peek().EnterDescription_data(ctx);
        }

        public override void ExitDescription_data(CougarParser.Description_dataContext ctx)
        {
            m_stackBuilders.Peek().ExitDescription_data(ctx);
        }

        public override void EnterDescription_detail(CougarParser.Description_detailContext ctx)
        {
            m_stackBuilders.Peek().EnterDescription_detail(ctx);
        }

        public override void ExitDescription_detail(CougarParser.Description_detailContext ctx)
        {
            m_stackBuilders.Peek().ExitDescription_detail(ctx);
        }

        public override void EnterDescription_word(CougarParser.Description_wordContext ctx)
        {
            m_stackBuilders.Peek().EnterDescription_word(ctx);
        }

        public override void ExitDescription_word(CougarParser.Description_wordContext ctx)
        {
            m_stackBuilders.Peek().ExitDescription_word(ctx);
        }

        public override void EnterDescription_numeric(CougarParser.Description_numericContext ctx)
        {
            m_stackBuilders.Peek().EnterDescription_numeric(ctx);
        }

        public override void ExitDescription_numeric(CougarParser.Description_numericContext ctx)
        {
            m_stackBuilders.Peek().ExitDescription_numeric(ctx);
        }

        public override void EnterDescription_hex(CougarParser.Description_hexContext ctx)
        {
            m_stackBuilders.Peek().EnterDescription_hex(ctx);
        }

        public override void ExitDescription_hex(CougarParser.Description_hexContext ctx)
        {
            m_stackBuilders.Peek().ExitDescription_hex(ctx);
        }

        public override void EnterDescription_punctuation(CougarParser.Description_punctuationContext ctx)
        {
            m_stackBuilders.Peek().EnterDescription_punctuation(ctx);
        }

        public override void ExitDescription_punctuation(CougarParser.Description_punctuationContext ctx)
        {
            m_stackBuilders.Peek().ExitDescription_punctuation(ctx);
        }

        public override void EnterDescription_parens(CougarParser.Description_parensContext ctx)
        {
            m_stackBuilders.Peek().EnterDescription_parens(ctx);
        }

        public override void ExitDescription_parens(CougarParser.Description_parensContext ctx)
        {
            m_stackBuilders.Peek().ExitDescription_parens(ctx);
        }

        public override void EnterCategory_attribute(CougarParser.Category_attributeContext ctx)
        {
            m_stackBuilders.Peek().EnterCategory_attribute(ctx);
        }

        public override void ExitCategory_attribute(CougarParser.Category_attributeContext ctx)
        {
            m_stackBuilders.Peek().ExitCategory_attribute(ctx);
        }

        public override void EnterCategory_list(CougarParser.Category_listContext ctx)
        {
            m_stackBuilders.Peek().EnterCategory_list(ctx);
        }

        public override void ExitCategory_list(CougarParser.Category_listContext ctx)
        {
            m_stackBuilders.Peek().ExitCategory_list(ctx);
        }

        public override void EnterCategory_name(CougarParser.Category_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterCategory_name(ctx);
        }

        public override void ExitCategory_name(CougarParser.Category_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitCategory_name(ctx);
        }

        public override void EnterGenerator_attribute(CougarParser.Generator_attributeContext ctx)
        {
            m_stackBuilders.Peek().EnterGenerator_attribute(ctx);
        }

        public override void ExitGenerator_attribute(CougarParser.Generator_attributeContext ctx)
        {
            m_stackBuilders.Peek().ExitGenerator_attribute(ctx);
        }

        public override void EnterGenerator_list(CougarParser.Generator_listContext ctx)
        {
            m_stackBuilders.Peek().EnterGenerator_list(ctx);
        }

        public override void ExitGenerator_list(CougarParser.Generator_listContext ctx)
        {
            m_stackBuilders.Peek().ExitGenerator_list(ctx);
        }

        public override void EnterGenerator_name(CougarParser.Generator_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterGenerator_name(ctx);
        }

        public override void ExitGenerator_name(CougarParser.Generator_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitGenerator_name(ctx);
        }

        public override void EnterGenerator_parens(CougarParser.Generator_parensContext ctx)
        {
            m_stackBuilders.Peek().EnterGenerator_parens(ctx);
        }

        public override void ExitGenerator_parens(CougarParser.Generator_parensContext ctx)
        {
            m_stackBuilders.Peek().ExitGenerator_parens(ctx);
        }

        public override void EnterConsumer_attribute(CougarParser.Consumer_attributeContext ctx)
        {
            m_stackBuilders.Peek().EnterConsumer_attribute(ctx);
        }

        public override void ExitConsumer_attribute(CougarParser.Consumer_attributeContext ctx)
        {
            m_stackBuilders.Peek().ExitConsumer_attribute(ctx);
        }

        public override void EnterConsumer_list(CougarParser.Consumer_listContext ctx)
        {
            m_stackBuilders.Peek().EnterConsumer_list(ctx);
        }

        public override void ExitConsumer_list(CougarParser.Consumer_listContext ctx)
        {
            m_stackBuilders.Peek().ExitConsumer_list(ctx);
        }

        public override void EnterConsumer_name(CougarParser.Consumer_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterConsumer_name(ctx);
        }

        public override void ExitConsumer_name(CougarParser.Consumer_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitConsumer_name(ctx);
        }

        public override void EnterAlertlevel_attribute(CougarParser.Alertlevel_attributeContext ctx)
        {
            m_stackBuilders.Peek().EnterAlertlevel_attribute(ctx);
        }

        public override void ExitAlertlevel_attribute(CougarParser.Alertlevel_attributeContext ctx)
        {
            m_stackBuilders.Peek().ExitAlertlevel_attribute(ctx);
        }

        public override void EnterAlertlevel_list(CougarParser.Alertlevel_listContext ctx)
        {
            m_stackBuilders.Peek().EnterAlertlevel_list(ctx);
        }

        public override void ExitAlertlevel_list(CougarParser.Alertlevel_listContext ctx)
        {
            m_stackBuilders.Peek().ExitAlertlevel_list(ctx);
        }

        public override void EnterAlertlevel_name(CougarParser.Alertlevel_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterAlertlevel_name(ctx);
        }

        public override void ExitAlertlevel_name(CougarParser.Alertlevel_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitAlertlevel_name(ctx);
        }

        public override void EnterReason_attribute(CougarParser.Reason_attributeContext ctx)
        {
            m_stackBuilders.Peek().EnterReason_attribute(ctx);
        }

        public override void ExitReason_attribute(CougarParser.Reason_attributeContext ctx)
        {
            m_stackBuilders.Peek().ExitReason_attribute(ctx);
        }

        public override void EnterReason_list(CougarParser.Reason_listContext ctx)
        {
            m_stackBuilders.Peek().EnterReason_list(ctx);
        }

        public override void ExitReason_list(CougarParser.Reason_listContext ctx)
        {
            m_stackBuilders.Peek().ExitReason_list(ctx);
        }

        public override void EnterReason_detail(CougarParser.Reason_detailContext ctx)
        {
            m_stackBuilders.Peek().EnterReason_detail(ctx);
        }

        public override void ExitReason_detail(CougarParser.Reason_detailContext ctx)
        {
            m_stackBuilders.Peek().ExitReason_detail(ctx);
        }

        public override void EnterReason_word(CougarParser.Reason_wordContext ctx)
        {
            m_stackBuilders.Peek().EnterReason_word(ctx);
        }

        public override void ExitReason_word(CougarParser.Reason_wordContext ctx)
        {
            m_stackBuilders.Peek().ExitReason_word(ctx);
        }

        public override void EnterReason_numeric(CougarParser.Reason_numericContext ctx)
        {
            m_stackBuilders.Peek().EnterReason_numeric(ctx);
        }

        public override void ExitReason_numeric(CougarParser.Reason_numericContext ctx)
        {
            m_stackBuilders.Peek().ExitReason_numeric(ctx);
        }

        public override void EnterReason_hex(CougarParser.Reason_hexContext ctx)
        {
            m_stackBuilders.Peek().EnterReason_hex(ctx);
        }

        public override void ExitReason_hex(CougarParser.Reason_hexContext ctx)
        {
            m_stackBuilders.Peek().ExitReason_hex(ctx);
        }

        public override void EnterReason_parens(CougarParser.Reason_parensContext ctx)
        {
            m_stackBuilders.Peek().EnterReason_parens(ctx);
        }

        public override void ExitReason_parens(CougarParser.Reason_parensContext ctx)
        {
            m_stackBuilders.Peek().ExitReason_parens(ctx);
        }

        public override void EnterWabfilter_attribute(CougarParser.Wabfilter_attributeContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_attribute(ctx);
        }

        public override void ExitWabfilter_attribute(CougarParser.Wabfilter_attributeContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_attribute(ctx);
        }

        public override void EnterWabfilter_site(CougarParser.Wabfilter_siteContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_site(ctx);
        }

        public override void ExitWabfilter_site(CougarParser.Wabfilter_siteContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_site(ctx);
        }

        public override void EnterWabfilter_subsite(CougarParser.Wabfilter_subsiteContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_subsite(ctx);
        }

        public override void ExitWabfilter_subsite(CougarParser.Wabfilter_subsiteContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_subsite(ctx);
        }

        public override void EnterWabfilter_host(CougarParser.Wabfilter_hostContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_host(ctx);
        }

        public override void ExitWabfilter_host(CougarParser.Wabfilter_hostContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_host(ctx);
        }

        public override void EnterWabfilter_lhost(CougarParser.Wabfilter_lhostContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_lhost(ctx);
        }

        public override void ExitWabfilter_lhost(CougarParser.Wabfilter_lhostContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_lhost(ctx);
        }

        public override void EnterWabfilter_ghost(CougarParser.Wabfilter_ghostContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_ghost(ctx);
        }

        public override void ExitWabfilter_ghost(CougarParser.Wabfilter_ghostContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_ghost(ctx);
        }

        public override void EnterWabfilter_ghosthour(CougarParser.Wabfilter_ghosthourContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_ghosthour(ctx);
        }

        public override void ExitWabfilter_ghosthour(CougarParser.Wabfilter_ghosthourContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_ghosthour(ctx);
        }

        public override void EnterWabfilter_sitehour(CougarParser.Wabfilter_sitehourContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_sitehour(ctx);
        }

        public override void ExitWabfilter_sitehour(CougarParser.Wabfilter_sitehourContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_sitehour(ctx);
        }

        public override void EnterWabfilter_watsite(CougarParser.Wabfilter_watsiteContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_watsite(ctx);
        }

        public override void ExitWabfilter_watsite(CougarParser.Wabfilter_watsiteContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_watsite(ctx);
        }

        public override void EnterWabfilter_wathost(CougarParser.Wabfilter_wathostContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_wathost(ctx);
        }

        public override void ExitWabfilter_wathost(CougarParser.Wabfilter_wathostContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_wathost(ctx);
        }

        public override void EnterWabfilter_nsawab(CougarParser.Wabfilter_nsawabContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_nsawab(ctx);
        }

        public override void ExitWabfilter_nsawab(CougarParser.Wabfilter_nsawabContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_nsawab(ctx);
        }

        public override void EnterWabfilter_member(CougarParser.Wabfilter_memberContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_member(ctx);
        }

        public override void ExitWabfilter_member(CougarParser.Wabfilter_memberContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_member(ctx);
        }

        public override void EnterWabfilter_member_part(CougarParser.Wabfilter_member_partContext ctx)
        {
            m_stackBuilders.Peek().EnterWabfilter_member_part(ctx);
        }

        public override void ExitWabfilter_member_part(CougarParser.Wabfilter_member_partContext ctx)
        {
            m_stackBuilders.Peek().ExitWabfilter_member_part(ctx);
        }

        public override void EnterFielddesc_attribute(CougarParser.Fielddesc_attributeContext ctx)
        {
            m_stackBuilders.Peek().EnterFielddesc_attribute(ctx);
        }

        public override void ExitFielddesc_attribute(CougarParser.Fielddesc_attributeContext ctx)
        {
            m_stackBuilders.Peek().ExitFielddesc_attribute(ctx);
        }

        public override void EnterFielddesc_name(CougarParser.Fielddesc_nameContext ctx)
        {
            m_stackBuilders.Peek().EnterFielddesc_name(ctx);
        }

        public override void ExitFielddesc_name(CougarParser.Fielddesc_nameContext ctx)
        {
            m_stackBuilders.Peek().ExitFielddesc_name(ctx);
        }

        public override void EnterFielddesc_desc(CougarParser.Fielddesc_descContext ctx)
        {
            m_stackBuilders.Peek().EnterFielddesc_desc(ctx);
        }

        public override void ExitFielddesc_desc(CougarParser.Fielddesc_descContext ctx)
        {
            m_stackBuilders.Peek().ExitFielddesc_desc(ctx);
        }

        public override void EnterFielddesc_detail(CougarParser.Fielddesc_detailContext ctx)
        {
            m_stackBuilders.Peek().EnterFielddesc_detail(ctx);
        }

        public override void ExitFielddesc_detail(CougarParser.Fielddesc_detailContext ctx)
        {
            m_stackBuilders.Peek().ExitFielddesc_detail(ctx);
        }

        public override void EnterFielddesc_word(CougarParser.Fielddesc_wordContext ctx)
        {
            m_stackBuilders.Peek().EnterFielddesc_word(ctx);
        }

        public override void ExitFielddesc_word(CougarParser.Fielddesc_wordContext ctx)
        {
            m_stackBuilders.Peek().ExitFielddesc_word(ctx);
        }

        public override void EnterFielddesc_numeric(CougarParser.Fielddesc_numericContext ctx)
        {
            m_stackBuilders.Peek().EnterFielddesc_numeric(ctx);
        }

        public override void ExitFielddesc_numeric(CougarParser.Fielddesc_numericContext ctx)
        {
            m_stackBuilders.Peek().ExitFielddesc_numeric(ctx);
        }

        public override void EnterFielddesc_hex(CougarParser.Fielddesc_hexContext ctx)
        {
            m_stackBuilders.Peek().EnterFielddesc_hex(ctx);
        }

        public override void ExitFielddesc_hex(CougarParser.Fielddesc_hexContext ctx)
        {
            m_stackBuilders.Peek().ExitFielddesc_hex(ctx);
        }

        public override void EnterFielddesc_expr(CougarParser.Fielddesc_exprContext ctx)
        {
            m_stackBuilders.Peek().EnterFielddesc_expr(ctx);
        }

        public override void ExitFielddesc_expr(CougarParser.Fielddesc_exprContext ctx)
        {
            m_stackBuilders.Peek().ExitFielddesc_expr(ctx);
        }

        public override void EnterFielddesc_punctuation(CougarParser.Fielddesc_punctuationContext ctx)
        {
            m_stackBuilders.Peek().EnterFielddesc_punctuation(ctx);
        }

        public override void ExitFielddesc_punctuation(CougarParser.Fielddesc_punctuationContext ctx)
        {
            m_stackBuilders.Peek().ExitFielddesc_punctuation(ctx);
        }

        public override void EnterFielddesc_parens(CougarParser.Fielddesc_parensContext ctx)
        {
            m_stackBuilders.Peek().EnterFielddesc_parens(ctx);
        }

        public override void ExitFielddesc_parens(CougarParser.Fielddesc_parensContext ctx)
        {
            m_stackBuilders.Peek().ExitFielddesc_parens(ctx);
        }

        public override void EnterFielddesc_money(CougarParser.Fielddesc_moneyContext ctx)
        {
            m_stackBuilders.Peek().EnterFielddesc_money(ctx);
        }
        public override void ExitFielddescMoney(CougarParser.FielddescMoneyContext ctx)
        {
            m_stackBuilders.Peek().ExitFielddescMoney(ctx);
        }

        public override void EnterFielddescQuotedString(CougarParser.FielddescQuotedStringContext ctx)
        {
            m_stackBuilders.Peek().EnterFielddescQuotedString(ctx);
        }

        public override void ExitFielddescQuotedString(CougarParser.FielddescQuotedStringContext ctx)
        {
            m_stackBuilders.Peek().ExitFielddescQuotedString(ctx);
        }
    }
}


