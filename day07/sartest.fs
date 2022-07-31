require ffl/tst.fs
require sar.fs

page

t{ ." a connection is made of either" cr

   ."     one signal input" cr
   s" az" 4807 make-simple-connection
   dup connection>input-1 4807 ?s
   dup connection>input-type-1 signal ?s
   dup connection>gate-type noop-gate ?s
   dup connection>output 256 /mod char a ?s char z ?s
   eval 4807 ?u

   ."     not gate" cr
  s" by" 4807 not-gate make-unary-connection
  dup connection>input-1 4807 ?s
  dup connection>input-type-1 signal ?s
  dup connection>gate-type not-gate ?s
  dup connection>output 256 /mod char b ?s char y ?s
  eval 4807 not ?u
}t

bye


