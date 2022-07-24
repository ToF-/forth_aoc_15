require ffl/tst.fs
require day07.fs

page
t{ ." an expression" cr
  init
  last-token# -1 ?s
  123 make-constant           \ 0: 123
  0 make-variable             \ 123 -> 1: x
  456 make-constant           \ 2: 456
  2 make-variable             \ 456 -> 3: y
  1 3 AND-OP make-binany-op  \ x AND y -> 4: d
  1 3 OR-OP make-binany-op   \ x OR y -> 5: e
  2 make-constant             \ 6: 2
  1 6 LS-OP make-binany-op  \ x LSHIFT 2 -> 7: f
  3 6 RS-OP make-binany-op  \ y RSHIFT 2 -> 8: g
  1 NOT-OP make-unary-op    \ NOT x -> 9: h
  3 NOT-OP make-unary-op    \ NOT y -> 10: i
  #tokens @ 11 ?s
  last-token# 10 ?s
  0 token eval-token 123 ?s
  1 token eval-token 123 ?s
  2 token eval-token 456 ?s
  3 token eval-token 456 ?s
  4 token eval-token 72 ?s
  5 token eval-token 507 ?s
  6 token eval-token 2 ?s
  7 token eval-token 492 ?s
  8 token eval-token 114 ?s
  9 token eval-token 65412 ?s
  10 token eval-token 65079 ?s
}t

t{ ." find-term" cr
  init
  s" foo" find-term ?false
  s" foo" add-term-string
  s" foo" find-term ?true 0 ?s
  s" bar" add-term-string
  s" qux" add-term-string
  s" bar" find-term ?true 1 ?s
  s" qux" find-term ?true 2 ?s
  s" baz" find-term ?false
}t

t{ ." find-operator" cr
  s" FOO" find-operator NULL-OP ?s
  s" AND" find-operator AND-OP ?s
  s" ->"  find-operator ARROW-OP ?s
}t

t{ ." assign-variable" cr
  init
  123 make-constant 
  #tokens @ 1 ?s
  last-token# s" x" assign-variable
  #tokens @ 2 ?s
  1 token eval-token 123 ?s
  last-token# s" y" assign-variable
  #tokens @ 3 ?s
  last-token# token eval-token 123 ?s
}t

t{ ." find-value" cr
  init
  s" 123" find-value token eval-token 123 ?s
  s" 456" find-value token eval-token 456 ?s
  s" 123" find-value token eval-token 123 ?s
  s" 456" find-value token eval-token 456 ?s
  #tokens @ 2 ?s
}t

t{ ." find-variable" cr
  init
  s" foo" find-variable 0 ?s
  s" bar" find-variable 1 ?s
  s" foo" find-variable 0 ?s
  #tokens @ 2 ?s
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

t{ ." interpret-instruction NOT 123 -> a" cr
  init
  #tokens @ 0 ?s
  s" NOT 123 -> a" get-instruction
  interpret-instruction
  #tokens @ 3 ?s
  0 token >>op-kind LITT-OP ?s
  1 token >>op-kind NOT-OP ?s
  1 token operand1 0 ?s
  2 token >>op-kind VAR-OP ?s
  2 token operand1 1 ?s
  s" a" find-value token eval-token 65412 ?s
}t
t{ ." interpret-instruction NOT b -> a" cr
  init
  #tokens @ 0 ?s
  s" NOT b -> a" get-instruction
  interpret-instruction
  #tokens @ 3 ?s
  0 token >>op-kind VAR-OP ?s
  1 token >>op-kind NOT-OP ?s
  1 token operand1 0 ?s
  2 token >>op-kind VAR-OP ?s
  2 token operand1 1 ?s
  s" a" find-value token dbg eval-token 65412 ?s

}t

bye
