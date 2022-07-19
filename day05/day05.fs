
: include? ( c,addr,l -- f )
  rot 0 swap 2swap
  over + swap do
    i c@ over = if
      nip i swap leave
    then
  loop drop ;

: include-3-vowels? ( addr,l -- f)
  0 -rot
  s" aeiou" over + swap do
    i c@ >r 2dup r> -rot include? if
      rot 1+ -rot
    then
  loop 2drop 3 >= ;

: include-pair? ( a,b,addr,l -- f)
  2>r 0 2r>
  over + swap do           \ a,b,n
    dup 0= if              \ a,b,n
      rot dup i c@         \ b,0,a,a,c
      = if                 \ b,0,a
        nip 1              \ b,a,1
      else 
        -rot               \ a,b,0
      then
    else                   \ b,a,1
      rot dup i c@         \ a,1,b,b,c
      = if                 \ a,1,b
        nip 2 leave        \ a,b,2
      else                 \ a,1,b
        nip 0              \ a,b,0
      then
    then
  loop -rot 2drop 2 = ;

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
      
: nice1? ( addr,l -- f )
  2dup include-3-vowels? -rot
  2dup include-doubled? -rot
  include-forbidden? 0= and and ;


80 constant maxline
create puzzle-line maxline allot

variable nices
variable file-name variable file-name-size

next-arg file-name-size ! file-name !

0 value fd-in

: solve-it1
  0 file-name @ file-name-size @ r/o open-file throw to fd-in
  begin
    puzzle-line maxline fd-in read-line throw while
    puzzle-line swap 2dup type nice1? if 1+ ."  Y" else ."  N" then cr 
  repeat drop
  fd-in close-file throw ;
  
page
solve-it1 .
cr
s" kpvwblrizaabmnhz" include-3-vowels? . cr
s" pygmy" include-3-vowels? . cr
s" dog" include-3-vowels? . cr
s" pygmalion" include-3-vowels? . cr
cr
bye
s" pygmalion" char y -rot include? . cr
s" pygmalion" char z -rot include? . cr
s" pygmalion" char n -rot include? . cr
cr
s" pygmalion" char g char u 2swap include-pair? . cr
s" pygmalion" char g char a 2swap include-pair? . cr
s" pygmalion" char l char a 2swap include-pair? . cr
s" domino"    char o char o 2swap include-pair? . cr
s" pygmalion" char g char m 2swap include-pair? . cr
s" pygmalion" char o char n 2swap include-pair? . cr
s" pygmalion" char p char y 2swap include-pair? . cr

cr
s" domino" include-doubled? . cr
s" ardvaark" include-doubled? . cr
s" waterloo" include-doubled? . cr
cr
s" domino" include-forbidden? . cr
s" abcde" include-forbidden? . cr
s" macdonald" include-forbidden? . cr
s" cupqake" include-forbidden? . cr
s" maxymum" include-forbidden? . cr

cr
s" ugknbfddgicrmopn" nice1? . cr
s" aaa" nice1? . cr
s" jchzalrnumimnmhp" nice1? . cr
s" haegwjzuvuyypxyu" nice1? . cr
s" dvszwmarrgswjxmb" nice1? . cr

bye

