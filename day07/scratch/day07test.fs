require ffl/tst.fs
require day07.fs
page

t{ ." add-string" cr
  init
  s" foo" add-string
  strings count s" foo" ?str
  strings dup c@ + 1+ next-string @ = ?true
}t

t{ ." add-lit-token" cr
  init #tokens @ 0 ?s
  s" 123" add-lit-token drop
  #tokens @ 1 ?s
  0 token@
  dup tk->type TK-LIT ?s
  dup tk->string s" 123" ?str
  eval 123 ?s
}t

t{ ." add-var-token, new var" cr
  init
  s" foo" add-var-token drop
  0 token@ dup tk->type TK-VAR ?s
  dup tk->string s" foo" ?str
  tk->type TK-VAR ?s
}t

t{ ." find-token" cr
  init
  s" foo" add-var-token drop
  s" foo" find-token ?true 0 ?s
}t

t{ ." add-var-token, already added var" cr
  init
  s" foo" add-var-token drop
  s" bar" add-var-token drop
  s" foo" add-var-token drop
  #tokens @ 2 ?s
}t

t{ ." add-unary-token" cr
  init
  s" 123" add-lit-token drop
  s" 123" find-token drop TK-NOT add-unary-token drop
  #tokens @ 2 ?s
  1 token@ dup tk->type TK-NOT ?s
  tk->operand1 0 ?s
}t

t{ ." add-binary-token" cr
  init
  s" 123" add-lit-token drop
  s" 456" add-lit-token drop
  s" 123" find-token drop
  s" 456" find-token drop
  TK-AND add-binary-token drop
  #tokens @ 3 ?s
  2 token@ dup tk->type TK-AND ?s
  dup tk->operand1 0 ?s
  tk->operand2 1 ?s
}t 

t{ ." add-assignment-token" cr
  init
  s" 123" add-lit-token drop
  s" 456" add-lit-token drop
  s" 123" find-token drop TK-NOT add-unary-token drop
  last-token s" 456" find-token drop TK-AND add-binary-token drop
  last-token s" foo" add-var-token drop last-token add-assignment-token drop
  last-token token@ dup tk->type TK-ASSIGN ?s
  dup tk->operand1 3 ?s
  tk->operand2 4 ?s
}t

t{ ." find-assignemt" cr
  init
  s" foo" find-assignment ?false
  s" 123" add-lit-token drop
  s" 456" add-lit-token drop
  s" 123" find-token drop TK-NOT add-unary-token drop
  last-token s" 456" find-token drop TK-AND add-binary-token drop
  last-token s" foo" add-var-token drop last-token add-assignment-token drop
  s" foo" find-assignment ?true last-token ?s
}t

t{ ." get-instruction" cr
  s" x LSHIFT 2 -> g" get-instruction
  next-step s" x" compare 0= ?true
  next-step s" LSHIFT" compare 0= ?true
  next-step s" 2" compare 0= ?true
  next-step s" ->" compare 0= ?true
  next-step s" g" compare 0= ?true
  next-step s" " compare 0= ?true
}t

t{ ." token-type"  cr
  s" 123" token-type TK-LIT ?s
  s" foo" token-type TK-VAR ?s
  s" NOT" token-type TK-NOT ?s
  s" AND" token-type TK-AND ?s
  s" OR" token-type TK-OR ?s
  s" LSHIFT" token-type TK-LSHIFT ?s
  s" RSHIFT" token-type TK-RSHIFT ?s
  s" ->" token-type TK-ASSIGN ?s
}t

t{ ." record-steps" cr
  s" 123 -> x" get-instruction 
  record-steps
  #steps @ 3 ?s
  0 step-string s" 123" ?str
  1 step-string s" ->" ?str
  2 step-string s" x" ?str
  s" foo AND bar -> qux" get-instruction
  record-steps
  #steps @ 5 ?s
  0 step-string s" foo" ?str
  1 step-string s" AND" ?str
  2 step-string s" bar" ?str
  3 step-string s" ->" ?str
  4 step-string s" qux" ?str
}t

t{ ." reorder-steps" cr
  s" 123 -> x" get-instruction record-steps reorder-steps
  0 step-string s" 123" ?str
  1 step-string s" x" ?str
  2 step-string s" ->" ?str

  s" NOT 123 -> x" get-instruction record-steps reorder-steps
  0 step-string s" 123" ?str
  1 step-string s" NOT" ?str
  2 step-string s" x" ?str
  3 step-string s" ->" ?str

  s" 123 AND foo -> x" get-instruction record-steps reorder-steps
  0 step-string s" 123" ?str
  1 step-string s" foo" ?str
  2 step-string s" AND" ?str
  3 step-string s" x" ?str
  4 step-string s" ->" ?str
}t
t{ ." interpret-steps" cr
  init
  s" 123 -> x" get-instruction record-steps reorder-steps interpret-steps
  #tokens @ 3 ?s
  0 token@ tk->type TK-LIT ?s
  1 token@ tk->type TK-VAR ?s
  2 token@ tk->type TK-ASSIGN ?s
  init
  s" NOT 123 -> x" get-instruction record-steps reorder-steps interpret-steps
  #tokens @ 4 ?s
  0 token@ tk->type TK-LIT ?s
  1 token@ tk->type TK-NOT ?s
  2 token@ tk->type TK-VAR ?s
  3 token@ tk->type TK-ASSIGN ?s
  init
  s" FOO -> x" get-instruction record-steps reorder-steps interpret-steps
  #tokens @ 3 ?s
  0 token@ tk->type TK-VAR ?s
  0 token@ tk->string s" FOO" ?str
  1 token@ tk->type TK-VAR ?s
  2 token@ tk->type TK-ASSIGN ?s
  init
  s" 123 AND 456 -> x" get-instruction record-steps reorder-steps interpret-steps
  #tokens @ 5 ?s
  0 token@ tk->type TK-LIT ?s
  1 token@ tk->type TK-LIT ?s
  2 token@ tk->type TK-AND ?s
  3 token@ tk->type TK-VAR ?s
  4 token@ tk->type TK-ASSIGN ?s
}t

t{ ." eval" cr
  init
  s" 123 AND 456 -> x" get-instructions
  0 token@ eval 123 ?s
  2 token@ eval 123 456 and ?s
  s" x" find-assignment drop token@ eval 72 ?s
  init
  s" 123 -> x" get-instructions
  s" x AND 456 -> y" get-instructions
  s" y" find-assignment drop token@ eval 72 ?s
  init
  s" 123 -> x" get-instructions
  s" 456 -> y" get-instructions
  s" x AND y -> z" get-instructions
  #tokens @ 9 ?s
  8 token@ eval 72 ?s
}t
t{ ." example" cr
  init
  s" 123 -> x" get-instructions
  s" 456 -> y" get-instructions
  s" x AND y -> d" get-instructions
  s" x OR y -> e" get-instructions
  s" x LSHIFT 2 -> f" get-instructions
  s" y RSHIFT 2 -> g" get-instructions
  debug on
  s" NOT x -> h" get-instructions
  debug off
  s" NOT y -> i" get-instructions
  .tokens
  s" d" find-assignment drop token@ eval 72 ?s
  s" e" find-assignment drop token@ eval 507 ?s
  s" f" find-assignment drop token@ eval 492 ?s
  s" g" find-assignment drop token@ eval 114 ?s
  s" h" find-assignment drop token@ eval 65412 ?s
  s" i" find-assignment drop token@ eval 65079 ?s
  s" x" find-assignment drop token@ eval 123 ?s
  s" y" find-assignment drop token@ eval 456 ?s
}t
bye
