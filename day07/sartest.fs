require ffl/tst.fs
require sar.fs

page

t{ ." conn<name" cr
  
  0 s" ab" conn<name

  pad conn>name 
  pad count s" ab" ?str

  0 s" x" conn<name

  pad conn>name
  pad count s" x" ?str
}t
bye
