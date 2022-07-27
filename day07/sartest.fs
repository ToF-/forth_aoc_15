require ffl/tst.fs
require sar.fs
page
t{ ." instruction>steps" cr

  s" foo bar quux bar baz" instruction>steps
  5 ?s
  0 nth-step s" foo" ?str
  2 nth-step s" quux" ?str
  4 nth-step s" baz" ?str

  s" foo bar quux" instruction>steps 3 ?s
}t

bye
