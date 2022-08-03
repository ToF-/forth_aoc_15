require ffl/tst.fs
require sar.fs

page


: .cnx 
  dup 2 base ! u. decimal cr
  hex u. decimal cr ;

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
  ."   for a simple signal" cr
  new-connection
  s" az" string>output 
         cnx-output      <field!
  signal cnx-input1-type <field!
  1      cnx-size        <field!
  4807   cnx-input1      <field!

  dup connection!
  dup .cnx
  eval 4807 ?s

  ." for a not gate with a signal" cr
  new-connection
  s" by" string>output
         cnx-output      <field!
  signal cnx-input1-type <field!
  not-gate cnx-gate      <field!
  2      cnx-size        <field!
  4807   cnx-input1      <field!

  dup .cnx
  eval 4807 not ?s

  ." for a gate with two input signal" cr
  new-connection
  s" cx" string>output
         cnx-output      <field!
  signal cnx-input1-type <field!
  signal cnx-input2-type <field!
  and-gate cnx-gate      <field!
  3      cnx-size        <field!
  4807   cnx-input1      <field!
  4217   cnx-input2      <field!

  dup .cnx
  eval 4161 ?s

  ." for a simple wire" cr
  new-connection
  s" dw" string>output   
         cnx-output      <field!
  wired  cnx-input1-type <field!
  1      cnx-size        <field!
#connections 1 ?s
s" az" string>output find-connection ?true drop
  s" az" string>output
         cnx-input1      <field!

  dup .cnx
  dbg eval 4807 ?s


}t


bye
