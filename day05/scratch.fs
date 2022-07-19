
: vowel? ( c -- f )
  dup [char] a = swap
  dup [char] e = swap
  dup [char] i = swap
  dup [char] o = swap
      [char] u =
    or or or or ;

: couple ( c1,c2 -- n )
  swap 256 * + ;

: same ( n -- f )
  256 /mod = ;

char a char b couple constant ab
char c char d couple constant cd
char p char q couple constant pq
char x char y couple constant xy

variable vowels
variable has-same
variable has-forbidden

: track-vowels ( c -- )
  vowel? if 1 vowels +! then ;

: forbidden? ( n -- f )
  dup ab = swap
  dup cd = swap
  dup pq = swap
      xy = 
    or or or ;

variable h 
: nice? ( addr,l -- f )
  vowels off
  has-same off
  has-forbidden off
  bl -rot
  over + swap do
    i c@
    tuck
    dup track-vowels
    couple 
    dup same if has-same on then
    forbidden? if has-forbidden on then
  loop drop
  vowels @ 3 >= 
  has-same @ and
  has-forbidden @ 0= and ;
  
80 constant maxline
create puzzle-line maxline allot

variable nices
0 value fd-in
: solve-it
  0
  next-arg r/o open-file throw to fd-in
  begin
    puzzle-line maxline fd-in read-line throw while
    puzzle-line swap 2dup type nice? if 1+ ."  Y" else ."  N" then cr 
  repeat drop
  fd-in close-file throw ;

s" ugknbfddgicrmopn" nice? . 
s" aaa" nice? . 
s" jchzalrnumimnmhp" nice? . 
s" haegwjzuvuyypxyu" nice? . 
s" dvszwmarrgswjxmb" nice? . 
cr

solve-it . cr

bye

