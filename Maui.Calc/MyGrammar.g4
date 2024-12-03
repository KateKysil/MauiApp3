grammar MyGrammar;

compileUnit: expression EOF;

expression
    : '-' expression                                    # UnaryMinus
    | '+' expression                                    # UnaryPlus
    | expression '^' expression                         # Exponent
    | expression '*' expression                         # Multiply
    | expression '/' expression                         # Divide
    | expression '+' expression                         # Add
    | expression '-' expression                         # Subtract
    | 'max' '(' expression ',' expression ')'           # MaxFunction
    | 'min' '(' expression ',' expression ')'           # MinFunction
    | '(' expression ')'                                # Parens
    | NUMBER                                            # Number
    | IDENTIFIER #IdentifierExpr
    ;

NUMBER: [0-9]+;
IDENTIFIER : [a-zA-Z]+[1-9][0-9]*;

WS: [ \t\r\n]+ -> skip;