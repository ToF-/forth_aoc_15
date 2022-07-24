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
  s" 123" add-lit-token
  #tokens @ 1 ?s
  0 token@
  dup tk->type TK-LIT ?s
  dup tk->string s" 123" ?str
  eval 123 ?s
}t

t{ ." add-var-token, new var" cr
  init
  s" foo" add-var-token
  0 token@ dup tk->type TK-VAR ?s
  dup tk->string s" foo" ?str
  tk->type TK-VAR ?s
}t

t{ ." find-token" cr
  init
  s" foo" add-var-token
  s" foo" find-token ?true 0 ?s
}t

t{ ." add-var-token, already added var" cr
  init
  s" foo" add-var-token
  s" bar" add-var-token
  s" foo" add-var-token
  #tokens @ 2 ?s
}t

t{ ." add-unary-token" cr
  init
  s" 123" add-lit-token
  s" 123" find-token drop TK-NOT add-unary-token
  #tokens @ 2 ?s
  1 token@ dup tk->type TK-NOT ?s
  tk->operand1 0 ?s
}t

t{ ." add-binary-token" cr
  init
  s" 123" add-lit-token
  s" 456" add-lit-token
  s" 123" find-token drop
  s" 456" find-token drop
  TK-AND add-binary-token
  #tokens @ 3 ?s
  2 token@ dup tk->type TK-AND ?s
  dup tk->operand1 0 ?s
  tk->operand2 1 ?s
}t 

t{ ." add-assignment-token" cr
  init
  s" 123" add-lit-token
  s" 456" add-lit-token
  s" 123" find-token drop TK-NOT add-unary-token
  last-token s" 456" find-token drop TK-AND add-binary-token
  last-token s" foo" add-var-token last-token add-assignment-token
  last-token token@ dup tk->type TK-ASSIGN ?s
  dup tk->operand1 3 ?s
  tk->operand2 4 ?s
}t

t{ ." find-assignemt" cr
  init
  s" foo" find-assignment ?false
  s" 123" add-lit-token
  s" 456" add-lit-token
  s" 123" find-token drop TK-NOT add-unary-token
  last-token s" 456" find-token drop TK-AND add-binary-token
  last-token s" foo" add-var-token last-token add-assignment-token
  s" foo" find-assignment ?true last-token ?s
}t
bye
