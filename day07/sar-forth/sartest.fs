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

t{ ." s>prepend" cr
  
  temp 20 erase
  s" bar" temp s>copy
  s" foo " temp s>prepend
  temp count s" foo bar" ?str
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

t{ ." #step>copy" cr

  s" 123" 0 #step>copy 
  0 #step s" 123" ?str

  s" AND" 1 #step>copy  
  1 #step s" AND" ?str 

  s" a" 2 #step>copy
  2 #step s" a" ?str

  s" " 3 #step>copy
  3 #step s" " ?str

}t

t{ ." wires" cr
  init-wires
  s" foo" wire 0 ?s
}t

t{ ." instruction>steps" cr
  s" a AND 4807 -> baz" instruction>steps
  steps# @ 5 ?s

  0 #step s" a" ?str
  1 #step s" AND" ?str
  2 #step s" 4807" ?str
  3 #step s" ->" ?str
  4 #step s" baz" ?str

  s" a bar quux" instruction>steps 
  steps# @ 3 ?s
}t

t{ ." arrange-steps" cr

  s" 123 -> x" instruction>steps arrange-steps
  output s" x" ?str
  0 #signal s" 123" ?str

  s" 123 AND 456 -> a" instruction>steps arrange-steps
  output s" a" ?str
  0 #signal s" 123" ?str
  1 #signal s" 456" ?str
  2 #signal s" AND" ?str

  s" NOT 123 -> a" instruction>steps arrange-steps
  output s" a" ?str
  0 #signal s" 123" ?str
  1 #signal s" NOT" ?str
}t

bye
123 constant _a
t{ ." sar>forth" cr

  s" 123 -> x" sar>forth
  sar-forth s" : _x 123 ;" ?str

  s" NOT 456 -> y" sar>forth
  sar-forth s" : _y 456 NOT ;" ?str

  s" 123 AND 456 -> z" sar>forth
  sar-forth s" : _z 123 456 AND ;" ?str

  s" 123 -> a" sar>forth
  sar-forth s" : __a 123 ; ' __a is _a" ?str

  s" b -> x" sar>forth
  sar-forth s" DEFER _b : _x _b ;" ?str

  s" b AND c -> x" sar>forth
  sar-forth s" DEFER _c DEFER _b : _x _b _c AND ;" ?str

  s" a LSHIFT c -> x" sar>forth
  sar-forth s" DEFER _c : _x _a _c LSHIFT ;" ?str
}t

t{ ." example" cr
s" NOT xav -> hal" sar>forth sar-forth evaluate
s" NOT yon -> ian" sar>forth sar-forth evaluate
s" 123 -> xav" sar>forth sar-forth evaluate
s" 456 -> yon" sar>forth sar-forth evaluate
s" xav AND yon -> dug" sar>forth sar-forth evaluate
s" xav LSHIFT 2 -> far" sar>forth sar-forth evaluate
s" xav OR yon -> elf" sar>forth sar-forth evaluate
s" yon RSHIFT 2 -> gus" sar>forth sar-forth evaluate
s" xav" sar-value 123 ?s
s" ian" sar-value 65079 ?s
s" hal" sar-value 65412 ?s
s" yon" sar-value 456 ?s
s" dug" sar-value 72 ?s
s" far" sar-value 492 ?s
s" elf" sar-value 507 ?s
s" gus" sar-value 114 ?s

}t
bye
