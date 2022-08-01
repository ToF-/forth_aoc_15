require ffl/tst.fs
require sar.fs

page

t{ ." storing several informations in a cell" cr
  2 base !  1001011000111 decimal
  (         ==  ----       )
  5 6 4 <field!
  3 11 3 <field!
  dup
  2 base !  1100101000111 decimal
  (         == ----       )
  ?s 
  dup 6 4 >field 5 ?s
     11 3 >field 3 ?s
}t
  

t{ ." building a connection" cr
  new-connection
  s" az" string>output cnx-output <field!
  signal cnx-input1-type <field!
  1 cnx-size <field!
  4807 cnx-input1 <field!
  dup 2 base ! u. decimal cr
  dup hex u. decimal cr
  eval 4807 ?s
}t

bye
