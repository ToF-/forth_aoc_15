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

t{ ." token" cr
  4807 42 2 make-token
  dup tk-type 2 ?s
  dup tk-operand1 4807 ?s
      tk-operand2 42 ?s
}t

bye
