require ffl/tst.fs
require matchsticks.fs

page

t{ ." mem-length tells how many chars in code a string occupy" cr
  s\" \"\"" 2dup dump 2dup code-length 2 ?s mem-length 0 ?s

  s\" \"abc\"" 2dup dump 2dup code-length 5 ?s mem-length 3 ?s

  s\" \"aaa\\\"aaa\"" 2dup dump 2dup code-length 10 ?s mem-length 7 ?s

  s\" \"A\\\\BCD\"" 2dup dump 2dup code-length 8 ?s mem-length 5 ?s

  s\" \"\\x27\"" 2dup dump 2dup code-length 6 ?s mem-length 1 ?s
}t

t{ ." overhead-length is code-length minus mem-length" cr

  s\" \"\"" code-length
  s\" \"abc\"" code-length
  s\" \"aaa\\\"aaa\"" code-length
  s\" \"\\x27\"" code-length 
  + + +
  s\" \"\"" mem-length
  s\" \"abc\"" mem-length
  s\" \"aaa\\\"aaa\"" mem-length
  s\" \"\\x27\"" mem-length 
  + + + - 12 ?s
}t

t{ ." read string from the sample and compute total overhead length" cr
  s" sample.txt" overhead 12 ?s
}t

t{ ." read string from the puzzle and compute total overhead length" cr
  s" puzzle08.txt" overhead 1342 ?s
}t

t{ ." encoded-length tells how many chars it takes to encode a string" cr
  s\" \"\"" encoded-length 6 ?s
  s\" \"abc\"" encoded-length 9 ?s
  s\" \"aaa\\\"aaa\"" encoded-length 16 ?s
  s\" \"\\x27\"" encoded-length 11 ?s
}t

t{ ." read string from the sample and compute total encoded overhead length" cr
  s" sample.txt" encoded 19 ?s
}t
t{ ." read string from the puzzle and compute total encoded overhead length" cr
  s" puzzle08.txt" encoded 2074 ?s
}t
bye
