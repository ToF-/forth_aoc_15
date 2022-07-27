require ffl/tst.fs
require sar.fs
page
t{ ." is-operator?" cr
  s" AND" is-operator? ?true
  s" ->" is-operator? ?true
  s" 123" is-operator? ?false
}t

t{ ." is-number?" cr
  s" 123" is-number? ?true 
  s" foo" is-number? ?false 
}t

t{ ." is-symbol?" cr
  s"  123 " is-symbol? ?false
  s"  AND " is-symbol? ?false
  s" _foo_ " is-symbol? ?true
}t

t{ ." nth-step>copy" cr

  s" 123" 0 nth-step>copy 
  0 nth-step s"  123 " ?str

  s" AND" 1 nth-step>copy  
  1 nth-step s"  AND " ?str 

  s" foo" 2 nth-step>copy
  2 nth-step s" _foo_ " ?str

  s" " 3 nth-step>copy
  3 nth-step s" " ?str

}t

t{ ." instruction>steps" cr
  s" foo AND 4807 -> baz" instruction>steps
  5 ?s

  0 nth-step s" _foo_ " ?str
  1 nth-step s"  AND " ?str
  2 nth-step s"  4807 " ?str
  3 nth-step s"  -> " ?str
  4 nth-step s" _baz_ " ?str

  s" foo bar quux" instruction>steps 3 ?s
}t

t{ ." arrange-steps" cr

  s" 123 -> x" instruction>steps arrange-steps
  output s" _x_ " ?str
  0 signal s"  123 " ?str

  s" 123 AND 456 -> foo" instruction>steps arrange-steps
  output s" _foo_ " ?str
  0 signal s"  123 " ?str
  1 signal s"  456 " ?str
  2 signal s"  AND " ?str

  s" NOT 123 -> foo" instruction>steps arrange-steps
  output s" _foo_ " ?str
  0 signal s"  123 " ?str
  1 signal s"  NOT " ?str
}t

t{ ." instruction>forth" 
  s" 123 -> x" instruction>forth 
  forth-instruction s" : _x_  123 ;" ?str \ because _x_ can't be found
  \ if _x_ can be found it has been deferred
  \ so we should have s" : >_x_ 123 ; ' >_x_ is _x_ "
}t
bye
