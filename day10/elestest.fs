require ffl/tst.fs
require eles.fs

page 
t{ ." 1 becomes 11" cr
  s" 1" look-and-say 
  output output-size @ s" 11" ?str
}t

t{ ." 11 becomes 21" cr
  s" 11" look-and-say
  output output-size @ s" 21" ?str
}t

t{ ." 21 becomes 1211" cr
  s" 21" look-and-say
  output output-size @ s" 1211" ?str
}t

t{ ." 1211 becomes 111221" cr
  s" 1211" look-and-say
  output output-size @ s" 111221" ?str
}t

t{ ." iterate" cr
  s" 1" 1 iterate
  output output-size @ s" 11" ?str
  s" 1" 2 iterate
  output output-size @ s" 21" ?str
  s" 1" 3 iterate
  output output-size @ s" 1211" ?str
  s" 1" 4 iterate
  output output-size @ s" 111221" ?str
  s" 1" 5 iterate
  output output-size @ s" 312211" ?str
  s" 1321131112" 40 iterate
  output-size @ 492982 ?s
  s" 1321131112" 50 iterate
  output-size @ 492982 ?s
}t
bye
