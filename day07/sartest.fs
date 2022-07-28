require ffl/tst.fs
require sar.fs
page

create temp 20 allot

t{ ." s>copy" cr

  temp 20 erase
  s" a bar quux" temp s>copy
  temp count s" a bar quux" ?str 
}t

t{ ." s>append" cr

  temp 20 erase
  s" a " temp s>copy
  s" quux" temp s>append
  temp count s" a quux" ?str
}t

t{ ." is-operator?" cr
  s" AND" is-operator? ?true
  s" ->" is-operator? ?true
  s" 123" is-operator? ?false
}t

t{ ." is-number?" cr
  s" 123" is-number? ?true 
  s" a" is-number? ?false 
}t

t{ ." is-symbol?" cr
  s" 123" is-symbol? ?false
  s" AND" is-symbol? ?false
  s" a" is-symbol? ?true
}t

t{ ." nth-step>copy" cr

  s" 123" 0 nth-step>copy 
  0 nth-step s" 123" ?str

  s" AND" 1 nth-step>copy  
  1 nth-step s" AND" ?str 

  s" a" 2 nth-step>copy
  2 nth-step s" a" ?str

  s" " 3 nth-step>copy
  3 nth-step s" " ?str

}t

t{ ." instruction>steps" cr
  s" a AND 4807 -> baz" instruction>steps
  5 ?s

  0 nth-step s" a" ?str
  1 nth-step s" AND" ?str
  2 nth-step s" 4807" ?str
  3 nth-step s" ->" ?str
  4 nth-step s" baz" ?str

  s" a bar quux" instruction>steps 3 ?s
}t

t{ ." arrange-steps" cr

  s" 123 -> x" instruction>steps arrange-steps
  output s" x" ?str
  0 signal s" 123" ?str

  s" 123 AND 456 -> a" instruction>steps arrange-steps
  output s" a" ?str
  0 signal s" 123" ?str
  1 signal s" 456" ?str
  2 signal s" AND" ?str

  s" NOT 123 -> a" instruction>steps arrange-steps
  output s" a" ?str
  0 signal s" 123" ?str
  1 signal s" NOT" ?str
}t

123 constant _a
t{ ." instruction>forth" 

  s" 123 -> x" instruction>forth 
  forth-instruction s" : _x 123 ;" ?str

  s" NOT 456 -> y" instruction>forth
  forth-instruction s" : _y 456 NOT ;" ?str

  s" 123 AND 456 -> z" instruction>forth
  forth-instruction s" : _z 123 456 AND ;" ?str

  s" 123 -> a" instruction>forth
  forth-instruction s" : __a 123 ; ' __a is _a" ?str

  s" b -> x" instruction>forth
  forth-instruction s" defer _b : x _b ;" ?str

  \ if _x_ can be found it has been deferred
  \ so we should have s" : >_x_ 123 ; ' >_x_ is _x_ "
}t
bye
