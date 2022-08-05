require ffl/tst.fs
require sar.fs

page


: .cnx 
  dup 2 base ! u. decimal cr
  hex u. decimal cr ;

t{ ." storing several informations in a cell" cr
  2 base !  1001011000111 decimal
  (         ==  ----       )
  5  6 4 bf!
  3 11 3 bf!
  dup
  2 base !  1100101000111 decimal
  (         == ----       )
  ?s 
  dup 6 4 bf@ 5 ?s
     11 3 bf@ 3 ?s
}t
  

t{ ." building a connection" cr
  ."   for a simple signal" cr
  new-connection
  s" az" s>key cnx-output      bf!
  signal       cnx-input1-type bf!
  1            cnx-size        bf!
  4807         cnx-input1      bf!
  dup connection!
  eval 4807 ?s

  ."   for a not gate with a signal" cr
  new-connection
  s" by" s>key cnx-output      bf!
  signal       cnx-input1-type bf!
  not-gate     cnx-gate        bf!
  2            cnx-size        bf!
  4807         cnx-input1      bf!
  dup connection!
  eval 4807 u16not ?s

  ."   for a gate with two input signals" cr
  new-connection
  s" cx" s>key cnx-output      bf!
  signal       cnx-input1-type bf!
  signal       cnx-input2-type bf!
  and-gate     cnx-gate        bf!
  3            cnx-size        bf!
  4807         cnx-input1      bf!
  4217         cnx-input2      bf!
  dup connection!
  eval 4161 ?s

  ."   for a simple wire" cr
  new-connection
  s" dw" s>key cnx-output      bf!
  wired        cnx-input1-type bf!
  1            cnx-size        bf!
  #connections 3 ?s
s" az" s>key find-connection ?true drop
  s" az" s>key
         cnx-input1      bf!

  eval 4807 ?s

  ."   for two wires" cr
  new-connection
  3      cnx-size        bf!
  s" ev" s>key
         cnx-output      bf!
  wired  cnx-input1-type bf!
  s" az" s>key
         cnx-input1      bf!
  wired  cnx-input2-type bf!
  s" by" s>key
         cnx-input2      bf!
  or-gate cnx-gate       bf!
  eval 65535 ?s

}t

t{ ." finding connections" cr
  s" az" s>key connection eval 4807 ?u
  s" by" s>key connection eval 4807 u16not ?u
  s" cx" s>key connection eval 4161 ?u
}t

t{ ." parsing instruction steps " cr
  s"   4807 cx AND -> az" s>steps
  steps# @ 5 ?s
  0 #step s" 4807" ?str
  1 #step s" cx" ?str
  2 #step s" AND" ?str
  3 #step s" ->" ?str
  4 #step s" az" ?str
}t

t{ ." parsing steps" cr
  s" 4807" parse-step number-step ?s 4807 ?s
  s" az" parse-step wire-step ?s s" az" s>key ?s
  s" NOT" parse-step gate-step ?s not-gate ?s
}t

t{ ." interpreting steps to connection" cr
  ."    for a simple signal" cr
    s" 4807 -> ax"
    steps>cnx
    dup cnx-size bf@ 1 ?s
    dup cnx-output bf@ s" ax" s>key ?str
    dup cnx-input1 bf@ 4807 ?s
    dup cnx-input1-type bf@ signal ?s
    dup cnx-gate bf@ 0 ?s
    drop


}t

bye
