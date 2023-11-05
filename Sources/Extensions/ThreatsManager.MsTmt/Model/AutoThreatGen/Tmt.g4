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

fragment A : [aA]; // match either an 'a' or 'A'
fragment B : [bB];
fragment C : [cC];
fragment D : [dD];
fragment E : [eE];
fragment F : [fF];
fragment G : [gG];
fragment H : [hH];
fragment I : [iI];
fragment J : [jJ];
fragment K : [kK];
fragment L : [lL];
fragment M : [mM];
fragment N : [nN];
fragment O : [oO];
fragment P : [pP];
fragment Q : [qQ];
fragment R : [rR];
fragment S : [sS];
fragment T : [tT];
fragment U : [uU];
fragment V : [vV];
fragment W : [wW];
fragment X : [xX];
fragment Y : [yY];
fragment Z : [zZ];

FLOW       : F L O W;
SOURCE     : S O U R C E;
TARGET     : T A R G E T;

IS         : I S;
CROSSES    : C R O S S E S;

AND        : A N D;
OR         : O R;
NOT        : N O T;

LPAREN     : '(' ;
RPAREN     : ')' ;
CHARACTER : UPPERLETTER | LOWERLETTER | DIGIT | '-';
CHARACTER_POINT : CHARACTER | '.';
TEXT      : '\'' ~('\'')* '\'';

WHITESPACE          : (' '|'\t')+ -> skip ;