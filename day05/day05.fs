
require ffl/tst.fs

: include? ( c,addr,l -- f )
  rot false swap 2swap
  over + swap do
    dup i c@ = if nip i swap leave then
  loop drop ;

: include-3-vowels? ( addr,l -- f)
  false -rot
  over + swap do
    i c@ s" aeiou" include? if 1+ then
  loop 3 >= ;

: cmp-first ( c1,c2,c1,c -- c2,c1,1 | c1,c2,0 )
  = if swap true else false then ;

: include-pair? ( c1,c2,addr,l -- f )
  false -rot
  over + swap do
    0= if over i c@ cmp-first
    else over i c@ = if 2 leave
    else swap over i c@ cmp-first
    then then
  loop
  -rot 2drop 2 = ;

: include-doubled? ( addr,l -- f)
  false -rot 2dup
  over + swap do
    i c@ dup 2over include-pair? if
      rot drop true -rot leave
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

: pair@ ( addr -- n )
    dup c@ swap 1+ c@ 8 lshift or ;

: has-repeated-pair? ( addr,l -- f )
  false -rot
  1- over + swap 2dup          \ F,addr,l,addr,l
  do                           \ F,addr,l
    2dup                       \ F,addr,l,addr,l
    do                         \ F,addr,l
      i j - abs 2 >= if
        i pair@ j pair@ = if
          rot drop true -rot   \ T,addr,l
        then
      then
    loop 
  loop 2drop ;

: has-in-between? ( addr,l -- f)
  false -rot over 2 - + swap do
    i dup 2 + c@ swap c@ = if drop true then
  loop ;
    
: nice-2? ( addr,l -- f )
  2dup has-repeated-pair? 
  -rot has-in-between? and ;

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

: solve-it-2
  0 file-name @ file-name-size @ r/o open-file throw to fd-in
  begin
    puzzle-line maxline fd-in read-line throw while
    puzzle-line swap nice-2? if 1+ then
  repeat drop
  fd-in close-file throw ;
  
