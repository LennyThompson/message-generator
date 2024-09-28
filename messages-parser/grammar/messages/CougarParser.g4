parser grammar CougarParser;

@header {
}
options { tokenVocab=CougarLexer; }

cougar_messages
    :
    (
        macro_block
        | message_body
        | enum_definition
    )*

    ;

message_body    :
    STRUCT message_name message_comment? OPEN_BRACKET
    empty_comment*
    (
        attribute
        |
        description_attribute
        |
        category_attribute
        |
        generator_attribute
        |
        consumer_attribute
        |
        alertlevel_attribute
        |
        reason_attribute
        |
        wabfilter_attribute
        |
        empty_comment
        |
        attribute_extension
    )* (member | attribute_extension)*
    empty_comment*
    CLOSE_BRACKET END_OF_LINE
    ;

message_comment
    : MSG_COMMENT MSG_WORD* MSG_END
    ;
empty_comment
    : MSG_COMMENT MSG_END;

enum_definition
    : ENUM enum_name enum_comment?
    OPEN_ENUM_BRACKET
    (enum_comment | enum_value_definition)+
    CLOSE_ENUM_BRACKET END_OF_LINE
    ;

enum_name
    : ENUM_NAME
    ;

enum_value_definition
    : enum_value_name (ENUM_ASSIGN enum_value)? ENUM_COMMA? enum_comment?
    ;

enum_comment
    : ENUM_COMMENT ENUM_COMMENT_END
    ;

enum_value_name
    : ENUM_NAME
    ;

enum_value
    : ENUM_HEX_VALUE | ENUM_VALUE
    ;

attribute_detail
    : attribute_value? (ATTRIBUTE_SEPARATOR attribute_value?)* ATTR_END attribute_extension*?
    ;

// DESCRIPTION_MODE

description_attribute
    : DESCRIPTION_ATTRIBUTE description_name DESCRIPTION_COLON? description_detail* DESCRIPTION_END attribute_extension*
    ;

description_name
    : description_word*
    ;

description_data
    : (description_word | description_numeric | description_hex | description_punctuation | description_parens)*
    ;

description_detail
    : DESCRIPTION_SEPARATOR DESCRIPTION_DASH? description_data
    ;

description_word
    : (DESCRIPTION_WORD | DECSRIPTION_KEYWORD | DESCRIPTION_DASH)
    ;

description_numeric
    : DESCRIPTION_NUMERIC
    ;

description_hex
    : DESCRIPTION_HEX
    ;

description_punctuation
    : DESCRIPTION_PUNC
    ;

//description_item
//    : DESCRIPTION_ITEM
//    ;

description_parens
    : DESCRIPTION_OPEN description_data DESCRIPTION_CLOSE
    ;

// CATEGORY_MODE

category_attribute
    : CATEGORY_ATTRIBUTE category_list? CATEGORY_END attribute_extension*
    ;

category_list
    : category_name (CATEGORY_SEPARATOR? category_name)* ;

category_name
    : CATEGORY_WORD ;

// DESCRIPTION_MODE

generator_attribute
    : GENERATOR_ATTRIBUTE generator_list? GENERATOR_END
    ;

generator_list
    : generator_name generator_parens? (GENERATOR_SEPARATOR? generator_name)* ;

generator_name
    : GENERATOR_WORD ;

generator_parens
    : GENERATOR_OPEN generator_name GENERATOR_CLOSE
    ;

// DESCRIPTION_MODE

consumer_attribute
    : CONSUMER_ATTRIBUTE consumer_list? CONSUMER_END attribute_extension*
    ;

consumer_list
    : consumer_name (CONSUMER_SEPARATOR? consumer_name)* ;

consumer_name
    : CONSUMER_WORD ;

// DESCRIPTION_MODE

alertlevel_attribute
    : ALERTLEVEL_ATTRIBUTE alertlevel_list? ALERTLEVEL_END attribute_extension*
    ;

alertlevel_list
    : alertlevel_name (ALERTLEVEL_SEPARATOR alertlevel_name)* ;

alertlevel_name
    : ALERTLEVEL_WORD ;

// DESCRIPTION_MODE

reason_attribute
    : REASON_ATTRIBUTE reason_list? reason_detail* REASON_END attribute_extension*
    ;

reason_list
    : (reason_word | reason_numeric | reason_hex | reason_parens)*
    ;

reason_detail
    : REASON_SEPARATOR reason_list
    ;

reason_word
    : REASON_WORD
    ;
reason_numeric
    : REASON_NUMERIC
    ;
reason_hex
    : REASON_HEX
    ;
reason_parens
    : REASON_OPEN (reason_word | reason_numeric | reason_hex)+ REASON_CLOSE
    ;
// WABFILTER_MODE

wabfilter_attribute
    :
    WABFILTER_ATTRIBUTE (wabfilter_subsite | wabfilter_site | wabfilter_lhost | wabfilter_ghost | wabfilter_host | wabfilter_ghosthour | wabfilter_sitehour | wabfilter_watsite | wabfilter_wathost | wabfilter_nsawab)* WABFILTER_END attribute_extension*
    ;

wabfilter_site
    : SITE (WABFILTER_COLON wabfilter_member)?
    ;
wabfilter_subsite
    : SUBSITE (WABFILTER_COLON wabfilter_member)?
    ;
wabfilter_host
    : HOST (WABFILTER_COLON wabfilter_member)?
    ;

wabfilter_lhost
    : LHOST (WABFILTER_COLON wabfilter_member)?
    ;
wabfilter_ghost
    : GHOST (WABFILTER_COLON wabfilter_member)?
    ;
wabfilter_ghosthour
    : GHOSTHOUR (WABFILTER_COLON wabfilter_member)?
    ;
wabfilter_sitehour
    : SITEHOUR (WABFILTER_COLON wabfilter_member)?
    ;

wabfilter_watsite
    : WATSITE (WABFILTER_COLON wabfilter_member)?
    ;

wabfilter_wathost
    : WATHOST (WABFILTER_COLON wabfilter_member)?
    ;

wabfilter_nsawab
    : NSAWAB (WABFILTER_COLON wabfilter_member)?
    ;

wabfilter_member
    : wabfilter_member_part (WABFILTER_PERIOD wabfilter_member_part)*
    ;
wabfilter_member_part
    : WABFILTER_VALUE
    ;

// FIELDDESSC_MODE

fielddesc_attribute
    : FIELDDESC_ATTRIBUTE  (fielddesc_name | fielddesc_desc) fielddesc_detail* FIELDDESC_END attribute_extension*
    ;

fielddesc_name
    : FIELDDESC_WORD+
    ;

fielddesc_desc
    : (fielddesc_word | fielddesc_numeric | fielddesc_hex | fielddesc_parens | fielddesc_expr | fielddesc_punctuation | fielddesc_money | fielddesc_quoted_string)*
    ;

fielddesc_detail
    : FIELDDESC_SEPARATOR fielddesc_desc
    ;

fielddesc_word
    : FIELDDESC_WORD
    ;
fielddesc_numeric
    : FIELDDESC_NUMERIC
    ;
fielddesc_hex
    : FIELDDESC_HEX
    ;

fielddesc_expr
    : FIELDDESC_EXPR
    ;

fielddesc_punctuation
    : FIELDDESC_PUNCTUATION
    ;

fielddesc_parens
    : FIELDDESC_OPEN fielddesc_desc FIELDDESC_CLOSE
    ;

fielddesc_money
    : FIELDDESC_DOLLARS
    ;

fielddesc_quoted_string
    : FIELDDESC_QUOTE fielddesc_desc FIELDDESC_QUOTE
    ;

attribute
    : ATTRIBUTE attibute_key attribute_detail?
    ;

member
    : type_name member_name array_decl? MEMBER_END (attribute_extension* | fielddesc_attribute)?
    ;

array_decl : OPEN_SQUARE_BRACKET const_value CLOSE_SQUARE_BRACKET ;

attribute_value
    : (part_attribute_value | numeric_attribute_value | currency_attribute_value | attribute_enum | parenthesized_value)+ PERIOD?
    ;

parenthesized_value
    : ATTRIBUTE_OPEN_BRACE (part_attribute_value | numeric_attribute_value | currency_attribute_value | attribute_enum)+ ATTRIBUTE_CLOSE_BRACE
    ;

currency_attribute_value
    : CURRENCY_VALUE numeric_attribute_value?
    ;

numeric_attribute_value: NUMERIC_ATTR_VALUE;

attribute_enum
    : ATTRIBUTE_ENUM (part_attribute_value | numeric_attribute_value | currency_attribute_value)* PERIOD? ENUM_SEPARATOR?;

part_attribute_value
    :
    ATTRIBUTE_VALUE
    | ATTRIBUTE_QUOTED_VALUE
    | ATTRIBUTE_COLON
    | HASH_ATTRIBUTE_VALUE
    | QUESTION_ATTRIBUTE_VALUE
    | OPERATOR_ATTRIBUTE_VALUE
    | MEMBER_REFERENCE
    ;
attibute_key: ATTRIBUTE_VALUE;

member_name: message_name | MEMBER_NAME;

vector_type
    :
    STD VECTOR OPEN_ANG_BRACE type_name CLOSE_ANG_BRACE
    ;

string_type
    :
    STD STRING
    ;

map_type
    :
     STD MAP OPEN_ANG_BRACE type_name TYPE_DELIM type_name CLOSE_ANG_BRACE
     ;

std_type
    :
    vector_type | string_type | map_type
    ;

type_name
    :
    SHORT
    | INT
    | USHORT
    | ULONG
    | CHAR
    | BOOL
    | DWORD
    | FILETIME
    | BYTE
    | FLOAT
    | DOUBLE
    | LONGLONG
    | LONG
    | std_type
    | message_name
    ;

message_name
    : NAME;

const_value
    : expression_define | const_numeric_value | const_expression;

const_expression
    : (expression_define | const_numeric_value) expression_operator const_numeric_value;

expression_define
    : SIZE_VALUE+;

expression_operator
    : OPERATOR;

const_numeric_value
    : CONST_NUMBER;

attribute_extension
    : MSG_COMMENT (attribute_extension_content)? MSG_END;

attribute_extension_content
    : (attribute_extension_content_value | attribute_extension_parenthesized_value)+ MSG_PERIOD?
    ;
attribute_extension_content_value
    : (MSG_PERIOD | MSG_WORD | MSG_NUMBER | MSG_CURRENCY_VALUE | MSG_UNKNOWN | MSG_COLON)
    ;

attribute_extension_parenthesized_value: MSG_OPEN_BRACE attribute_extension_content+ MSG_CLOSE_BRACE;

macro_block
    : macro+;

macro
    :
    (
        macro_define
        |
        macro_include
        |
        macro_pragma
        |
        macro_ifndef
        |
        macro_if
        |
        macro_endif
    )
    (MACRO_END | EOF)
    ;


macro_define
    : HASH_MACRO DEFINE define_name define_value?;

define_name
    : MACRO_NAME;

define_value
    : MACRO_OPEN? (numeric_value | hex_numeric_value | quoted_string | macro_expr | macro_name) MACRO_CLOSE?;

macro_name
    : MACRO_NAME;

macro_expr
    : macro_name (macro_operator (macro_name | numeric_value))+;

numeric_value
    : MACRO_NUMBER;

macro_operator
    : (MACRO_PLUS | MACRO_MINUS);
    
hex_numeric_value
    : MACRO_HEX_NUMBER;

macro_include
    : HASH_MACRO INCLUDE (quoted_string | ((MACRO_OPEN_ANGLE_BRACE) include_name (MACRO_CLOSE_ANGLE_BRACE)));

include_name
    : MACRO_NAME HEADER_EXTENSION;

macro_ifndef
    : HASH_MACRO IFNDEF MACRO_NAME;

macro_if
    : HASH_MACRO IF
    macro_clause ( ( AND | OR ) macro_clause )*
    ;

macro_clause
    :
    ( MACRO_NOT? DEFINED MACRO_OPEN define_name MACRO_CLOSE)
    |
    ( MACRO_OPEN define_name macro_test numeric_value MACRO_CLOSE )
    ;

macro_test
    : (GREATER_THAN_EQUALS | LESS_THAN_EQUALS | EQUALS);

macro_endif
    : HASH_MACRO ENDIF
    ;

macro_pragma
    : HASH_MACRO PRAGMA
    (
        ONCE
        |
        pragma_name (MACRO_OPEN pragma_type? MACRO_COMMA? numeric_value? MACRO_CLOSE)?
    )
    ;

pragma_name
    : MACRO_NAME;

pragma_type
    : MACRO_NAME;

quoted_string
    : MACRO_QUOTED_STRING;
