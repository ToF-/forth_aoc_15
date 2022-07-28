require ffl/tst.fs
require day07.fs

t{ ." strings" cr
  init
  s" foo" string-index 0 ?s
  s" bar" string-index 1 ?s
  s" foo" string-index 0 ?s
  max-strings @ 2 ?s
  0 nth-string s" foo" ?str
  1 nth-string s" bar" ?str
}t

t{ ." raw-token" cr
  init
  s" FOO" raw-token
  dup tk-type TK-NOOP ?s
  dup tk-string s" FOO" ?str
  dup tk-operand1 0 ?s
      tk-operand2 0 ?s
}t

t{ ." set-tk-type : LIT " cr
  s" 4807" raw-token set-tk-type
  dup tk-type TK-LIT ?s
      tk-operand1 4807 ?s
}t

t{ ." set-tk-type : VAR " cr
  s" foo" raw-token set-tk-type
  dup tk-type TK-VAR ?s
      tk-assigned? ?false
}t

t{ ." set-tk-type : NOT " cr
  s" NOT" raw-token set-tk-type
  tk-type TK-NOT ?s
}t

t{ ." set-tk-type : -> " cr
  s" ->" raw-token set-tk-type
  tk-type TK-ASSIGN ?s
}t

t{ ." set-tk-type : AND " cr
  s" AND" raw-token set-tk-type
  tk-type TK-AND ?s
}t

t{ ." set-tk-type : OR " cr
  s" OR" raw-token set-tk-type
  tk-type TK-OR ?s
}t

t{ ." set-tk-type : LSHIFT " cr
  s" LSHIFT" raw-token set-tk-type
  tk-type TK-LSHIFT ?s
}t

t{ ." set-tk-type : RSHIFT " cr
  s" RSHIFT" raw-token set-tk-type
  tk-type TK-RSHIFT ?s
}t

t{ ." next-entry" cr
  s"   foo bar    qux-baz" entry swap cmove
  entry entry@ !
  next-entry step count s" foo" ?str
  next-entry step count s" bar" ?str
  next-entry step count s" qux-baz" ?str
  next-entry step count 0 ?s drop 
}t

t{ ." instruction" cr
  init
  s" 123 -> x" instruction drop
  s" 456 -> y" instruction drop
  s" x AND y -> d" instruction drop
  s" x OR y -> e" instruction drop
  s" x LSHIFT 2 -> f" instruction drop
  s" y RSHIFT 2 -> g" instruction drop
  s" NOT x -> h" instruction drop
  s" NOT y -> i" instruction drop
  swap-operators
  chain-tokens
  .tokens
  s" h" string-index 15 ?s
}t

t{ ." eval" cr
  s" x" eval-output 123 ?s
  s" d" eval-output 72 ?s
  s" h" eval-output 65412 ?s
}t
bye

  s" d" eval-output 72 ?s
  s" e" eval-output 507 ?s
  s" f" eval-output 492 ?s
  s" g" eval-output 114 ?s
  s" h" eval-output 65412 ?s
  s" i" eval-output 65079 ?s
  s" x" eval-output 123 ?s
  s" y" eval-output 456 ?s
}t
bye

t{ ." solve-it-1" cr
  solve-it-1
  0 ?s
}t

bye
