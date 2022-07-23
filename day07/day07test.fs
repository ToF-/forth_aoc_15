require ffl/tst.fs
require day07.fs

page
t{ ." an expression" cr
  123 make-constant           \ 0: 123
  0 make-variable             \ 123 -> 1: x
  456 make-constant           \ 2: 456
  2 make-variable             \ 456 -> 3: y
  1 3 AND-TOKEN make-binany-op  \ x AND y -> 4: d
  1 3 OR-TOKEN make-binany-op   \ x OR y -> 5: e
  2 make-constant             \ 6: 2
  1 6 LS-TOKEN make-binany-op  \ x LSHIFT 2 -> 7: f
  3 6 RS-TOKEN make-binany-op  \ y RSHIFT 2 -> 8: g
  1 NOT-TOKEN make-unary-op    \ NOT x -> 9: h
  3 NOT-TOKEN make-unary-op    \ NOT y -> 10: i
  last-token @ 11 ?s
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
  0 last-token !
  s" 123" find-term 0 ?s
}t
bye
