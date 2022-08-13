require ffl/tst.fs
require cp.fs

page
  
t{ ." incrementing string" cr
  s" aaaaaaaa" 2dup increment
  s" aaaaaaab" ?str
  s" aaaaaaaz" 2dup increment
  s" aaaaaaba" ?str
  s" aaaaaazz" 2dup increment
  s" aaaaabaa" ?str
  s" zzzzzzzz" 2dup increment
  s" aaaaaaaa" ?str
}t

bye
