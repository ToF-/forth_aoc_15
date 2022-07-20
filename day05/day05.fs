
require ffl/tst.fs

: include? ( c,addr,l -- f )
  rot 0 swap 2swap
  over + swap do
    dup i c@ = if nip i swap leave then
  loop drop ;

: include-3-vowels? ( addr,l -- f)
  0 -rot
  over + swap do
    i c@ s" aeiou" include? if 1+ then
  loop 3 >= ;

: cmp-first ( c1,c2,c1,c -- c2,c1,1 | c1,c2,0 )
  = if swap 1 else 0 then ;

: include-pair? ( c1,c2,addr,l -- f )
  0 -rot
  over + swap do
    0= if over i c@ cmp-first
    else over i c@ = if 2 leave
    else swap over i c@ cmp-first
    then then
  loop
  -rot 2drop 2 = ;

: include-doubled? ( addr,l -- f)
  0 -rot 2dup
  over + swap do
    i c@ dup 2over include-pair? if
      rot drop -1 -rot leave
    then
  loop 2drop ;

\ ab, cd, pq, or xy
: include-forbidden? ( addr,l -- f)
  [char] a [char] b 2over include-pair? >r
  [char] c [char] d 2over include-pair? >r
  [char] p [char] q 2over include-pair? >r
  [char] x [char] y 2swap include-pair? 
  r> or r> or r> or ;
      
: nice-1? ( addr,l -- f )
  2dup include-3-vowels? -rot
  2dup include-doubled? -rot
  include-forbidden? 0= and and ;


80 constant maxline
create puzzle-line maxline allot

variable nices
variable file-name variable file-name-size

next-arg file-name-size ! file-name !

0 value fd-in

: solve-it-1
  0 file-name @ file-name-size @ r/o open-file throw to fd-in
  begin
    puzzle-line maxline fd-in read-line throw while
    puzzle-line swap nice-1? if 1+ then
  repeat drop
  fd-in close-file throw ;
  
page

t{ ." include?" cr
  s" pygmalion" char y -rot include? ?true
  s" pygmalion" char z -rot include? ?false
  s" pygmalion" char n -rot include? ?true
}t
t{ ." include-pair?" cr
  s" pygmalion" char g char u 2swap include-pair? ?false
  s" pygmalion" char g char a 2swap include-pair? ?false
  s" pygmalion" char l char a 2swap include-pair? ?false
  s" domino"    char o char o 2swap include-pair? ?false
  s" pygmalion" char p char y 2swap include-pair? ?true
  s" pygmalion" char g char m 2swap include-pair? ?true
  s" pygmalion" char o char n 2swap include-pair? ?true
  s" aabmnh" char a char b 2swap include-pair? ?true
}t
t{ ." include-3-vowels?" cr
  s" kpvwblrizaabmnhz" include-3-vowels? ?true
  s" pygmy" include-3-vowels? ?false
  s" dog" include-3-vowels? ?false
  s" pygmalion" include-3-vowels? ?true
}t
t{ ." include-doubled?" cr
  s" domino" include-doubled? ?false
  s" ardvaark" include-doubled? ?true
  s" waterloo" include-doubled? ?true
}t
t{ ." include-forbidden?" cr
  s" domino" include-forbidden? ?false
  s" abhorrent" include-forbidden? ?true
  s" cabdriver" include-forbidden? ?true
  s" abcde" include-forbidden? ?true
  s" macdonald" include-forbidden? ?true
  s" cupqake" include-forbidden? ?true
  s" maxymum" include-forbidden? ?true
  s" aabmnhz" include-forbidden? ?true
  s" kpvwblrizaabmnhz" include-forbidden? ?true
}t
t{ ." nice-1?" cr
  s" ugknbfddgicrmopn" nice-1? ?true
  s" aaa" nice-1? ?true
  s" jchzalrnumimnmhp" nice-1? ?false
  s" haegwjzuvuyypxyu" nice-1? ?false
  s" dvszwmarrgswjxmb" nice-1? ?false
}t
t{ ." solve-it-1" cr
  solve-it-1 255 ?s
}t

bye

