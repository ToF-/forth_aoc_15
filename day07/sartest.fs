require ffl/tst.fs
require sar.fs

page


: .cnx 
  dup 2 base ! u. decimal cr
  hex u. decimal cr ;

t{ ." storing several informations in a cell" cr
  2 base !  1001011000111 decimal
  (         ==  ----       )
  5 6 4 <-!
  3 11 3 <-!
  dup
  2 base !  1100101000111 decimal
  (         == ----       )
  ?s 
  dup 6 4 -> 5 ?s
     11 3 -> 3 ?s
}t
  

t{ ." building a connection" cr
  ."   for a simple signal" cr
  new-connection
  s" az" string>output 
         cnx-output      <-!
  signal cnx-input1-type <-!
  1      cnx-size        <-!
  4807   cnx-input1      <-!

  dup connection!
  eval 4807 ?s

  ." for a not gate with a signal" cr
  new-connection
  s" by" string>output
         cnx-output      <-!
  signal cnx-input1-type <-!
  not-gate cnx-gate      <-!
  2      cnx-size        <-!
  4807   cnx-input1      <-!

  dup connection!

  eval 4807 not ?s

  ." for a gate with two input signal" cr
  new-connection
  s" cx" string>output
         cnx-output      <-!
  signal cnx-input1-type <-!
  signal cnx-input2-type <-!
  and-gate cnx-gate      <-!
  3      cnx-size        <-!
  4807   cnx-input1      <-!
  4217   cnx-input2      <-!
  dup connection!

  eval 4161 ?s

  ." for a simple wire" cr
  new-connection
  s" dw" string>output   
         cnx-output      <-!
  wired  cnx-input1-type <-!
  1      cnx-size        <-!
  #connections 3 ?s
s" az" string>output find-connection ?true drop
  s" az" string>output
         cnx-input1      <-!

  eval 4807 ?s

  ." for two wires" cr
  s" az" string>output connection eval 4807 ?s
  s" cx" string>output connection eval 4217 ?s
  new-connection
  3      cnx-size        <-!
  s" ev" string>output
         cnx-output      <-!
  wired  cnx-input1-type <-!
  s" az" string>output
         cnx-input1      <-!
  wired  cnx-input2-type <-!
  s" cx" string>output
         cnx-input2      <-!
  or-gate cnx-gate       <-!
  .s cr
  
  dbg eval 4261 ?s


}t


bye
