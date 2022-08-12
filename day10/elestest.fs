require ffl/tst.fs
require eles.fs

page 
t{ ." 1 becomes 11" cr
  s" 1" look-and-say 
  output count s" 11" ?str
}t

t{ ." 11 becomes 21" cr
  s" 11" look-and-say
  output count s" 21" ?str
}t

t{ ." 21 becomes 1211" cr
  s" 21" look-and-say
  output count s" 1211" ?str
}t

t{ ." 1211 becomes 111221" cr
  s" 1211" look-and-say
  output count s" 111221" ?str
}t

t{ ." iterate" cr
  s" 1" 1 iterate
  output count s" 11" ?str
  s" 1" 2 iterate
  output count s" 21" ?str
  s" 1" 3 iterate
  output count s" 1211" ?str
  s" 1" 4 iterate
  output count s" 111221" ?str
  s" 1" 5 iterate
  output count s" 312211" ?str
  s" 1321131112" 1 iterate
  output count s" 11131221133112" ?str
  s" 1321131112" 20 iterate
  output count s" 11131221133112" ?str
}t
bye
