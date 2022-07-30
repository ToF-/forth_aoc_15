require ffl/tst.fs
require sar.fs

page

t{ ." s>cons" cr
  
  s" ab" .s drop 1-
  char c swap dbg s>cons
  s" abc" ?str 
}t

t{ ." >pname" cr
  
  s" az" 
  2dup dump string>pname
  pad    dbg pname>string
  pad count s" az" ?str

  s" a" string>pname
  pad    pname>string
  pad count dump
  pad count s" a" ?str 
}t

t{ ." string>connection" cr

  s" a" string>pname dup .
  connection 
  connection>pname  dup .

  pad pname>string
  pad count s" az" ?str
}t

bye
