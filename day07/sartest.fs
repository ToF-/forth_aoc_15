require ffl/tst.fs
require sar.fs
page
t{ ." nth-step-cmove" cr

  s" 123" 0 nth-step-cmove 
  0 nth-step s" 123" ?str

  s" AND" 1 nth-step-cmove  
  1 nth-step s" AND" ?str 

  s" foo" 2 nth-step-cmove
  2 nth-step s" _foo_" ?str
}t

t{ ." instruction>steps" cr
  s" foo AND 4807 -> baz" instruction>steps
  5 ?s

  0 nth-step s" _foo_" ?str
  1 nth-step s" AND" ?str
  2 nth-step s" 4807" ?str
  3 nth-step s" ->" ?str
  4 nth-step s" _baz_" ?str

  s" foo bar quux" instruction>steps 3 ?s
}t

t{ ." is-operator?" cr
  s" AND" is-operator? ?true 1 ?s
  s" ->" is-operator? ?true 5 ?s
  s" 123" is-operator? ?false
}t

t{ ." is-number" cr
  s" 123" is-number? ?true 123 ?s
  s" foo" is-number? ?false 
}t

t{ ." arrange-steps" cr

  s" 123 -> x" instruction>steps step>signals
  output s" _x_" ?str
  0 signal s" 123" ?str

  s" 123 AND 456 -> foo" instruction>steps step>signals
  output s" _foo_" ?str
  0 signal s" 123" ?str
  1 signal s" 456" ?str
  2 signal s" AND" ?str

  s" NOT 123 -> foo" instruction>steps step>signals
  output s" _foo_" ?str
  0 signal s" 123" ?str
  1 signal s" NOT" ?str
}t

bye
