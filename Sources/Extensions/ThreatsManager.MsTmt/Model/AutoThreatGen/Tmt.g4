grammar Tmt;

parse  : expression EOF;

expression
 : LPAREN expression RPAREN                       #parentExpression
 | NOT expression                                 #notExpression
 | left=expression op=boolean right=expression    #booleanExpression
 | left=subjectproperty op=IS right=TEXT          #propertyExpression
 | left=subject op=IS right=TEXT                  #subjectExpression
 | left=FLOW op=operator right=TEXT               #flowExpression
;

operator : IS | CROSSES;

boolean  : AND | OR;

subjectproperty : (FLOW|subject) PROPERTY;

subject  : SOURCE|TARGET;
PROPERTY : '.' CHARACTER_POINT+;

fragment UPPERLETTER : [A-Z] ;
fragment LOWERLETTER : [a-z] ;
fragment DIGIT : [0-9] ;

FLOW       : 'flow';
SOURCE     : 'source';
TARGET     : 'target';

IS         : 'is';
CROSSES    : 'crosses';

AND        : 'and' ;
OR         : 'or' ;
NOT        : 'not';

LPAREN     : '(' ;
RPAREN     : ')' ;
CHARACTER : UPPERLETTER | LOWERLETTER | DIGIT | '-';
CHARACTER_POINT : CHARACTER | '.';
TEXT      : '\'' ~('\'')* '\'';

WHITESPACE          : (' '|'\t')+ -> skip ;