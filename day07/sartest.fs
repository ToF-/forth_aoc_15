require ffl/tst.fs
require sar.fs

page

t{ ." >pname" cr
  
  s" az" string>pname
  pad    pname>string
  pad count s" az" ?str
}t

t{ ." string>connection" cr

  s" a" string>connection 
  conection>pname 
  pad pname>string
  pad count s" az" ?str
}

bye
