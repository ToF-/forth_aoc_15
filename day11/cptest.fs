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

t{ ." has-straight" cr
  s" aaaaaabc" has-straight ?true
  s" aaaaabca" has-straight ?true
  s" abcdazta" has-straight ?true
  s" xyzdazta" has-straight ?true
  s" aacaaaaa" has-straight ?false
}t

t{ ." has-letters-iol" cr
  s" aaaaaaaa" has-letters-iol ?false
  s" aaaoaaaa" has-letters-iol ?true
  s" aaaaaaia" has-letters-iol ?true
  s" laaaaaaa" has-letters-iol ?true
}t

t{ ." has-two-pairs" cr
  s" aaaaaaaa" has-two-pairs ?false
  s" aabbaaaa" has-two-pairs ?true
  s" abbdefhh" has-two-pairs ?true
  s" abbdefbb" has-two-pairs ?false
  s" abbbefhh" has-two-pairs ?true
}t

t{ ." meet-requirements" cr
  s" hijklmmn" meet-requirements ?false
  s" abbceffg" meet-requirements ?false
  s" abbcegjk" meet-requirements ?false
  s" abcdffaa" meet-requirements ?true
  s" ghjaabcc" meet-requirements ?true
}t

t{ ." next-password" cr
  s" abcdefgh" 2dup next-password s" abcdffaa" ?str
  s" ghijklmn" 2dup next-password s" ghjaabcc" ?str
  s" hxbxwxba" 2dup next-password s" hxbxxyzz" ?str
  s" hxbxxyzz" 2dup next-password s" hxcaabcc" ?str
}t

bye
