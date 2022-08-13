8 constant set-max-size
create set set-max-size cells allot
variable set-size

: initial ( n -- )
  dup set-size !
  0 do
     i dup cells set + ! 
  loop ;

defer fold-set-xt

: set-ref ( i -- addr )
  cells set + ;

: fold-set
  set-size @ 0 do
    i set-ref @ fold-set-xt
  loop ;

: reversed-pair? ( i -- b )
  dup set-ref @ 
  swap 1+ set-ref @ > ;

: swap-pair ( i,j -- )
  set-ref swap set-ref
  2dup @ swap @ 
  rot ! swap ! ;

: first-non-reversed ( i -- i )
  set-size @ 2 - 
  begin
    dup 0 >= over reversed-pair? and while
    1-
  repeat nip ;
    
: reverse-suffix ( i -- n )
  1+ set-size @ 1-
  begin 
    2dup < while
    2dup swap-pair
    1- swap 1+ swap
  repeat 2drop ;

: first-greater ( i -- j )
  dup 1+
  begin
    2dup set-ref @ swap set-ref @ < while
    1+
  repeat nip ;

defer use-permutation

: next-permutation ( -- pos )
  set-size @ 2 -
  first-non-reversed
  dup reverse-suffix
  dup 0 >= if
    dup first-greater
    over swap-pair
  then ;

: permute-set
  begin
    use-permutation
    next-permutation 
    0 >= while
  repeat ;

8 constant max-strings
80 constant string-size
create strings string-size max-strings * allot
variable #strings

: string-ref ( n -- addr )
  string-size * strings + ;

: string# ( n -- addr,l )
  string-ref count ;

: add-string ( addr,l -- )
  #strings @ string-ref over swap c!
  #strings @ string-ref 1+ swap cmove
  1 #strings +! ;

: init-strings
  strings string-size max-strings * erase
  #strings off ;

: find-string ( addr,l -- n )
  -1 -rot
  #strings @ 0 do
    i string# 2over compare 0= if
      rot drop i -rot leave
    then
  loop 2drop ;

init-strings
s" AlphaCentauri" add-string
s" Snowdin" add-string 
s" Tambi" add-string
s" Faerun" add-string
s" Norrath" add-string
s" Straylight" add-string
s" Tristram" add-string
s" Arbre" add-string

create distances max-strings max-strings * cells allot

: distance-ref ( i,j -- addr )
  max-strings cells * swap cells + distances + ;

: distance ( i,j -- n )
  distance-ref @ ;

: add-distance ( addr,l,addr,l,d -- )
  >r
  find-string -rot find-string
  2dup distance-ref r@ swap !
  swap distance-ref r> swap ! ;

s" AlphaCentauri" s" Snowdin" 66 add-distance
s" AlphaCentauri" s" Tambi" 28 add-distance
s" AlphaCentauri" s" Faerun" 60 add-distance
s" AlphaCentauri" s" Norrath" 34 add-distance
s" AlphaCentauri" s" Straylight" 34 add-distance
s" AlphaCentauri" s" Tristram" 3 add-distance
s" AlphaCentauri" s" Arbre" 108 add-distance
s" Snowdin"       s" Tambi" 22 add-distance
s" Snowdin"       s" Faerun" 12 add-distance
s" Snowdin"       s" Norrath" 91 add-distance
s" Snowdin"       s" Straylight" 121 add-distance
s" Snowdin"       s" Tristram" 111 add-distance
s" Snowdin"       s" Arbre" 71 add-distance
s" Tambi"         s" Faerun" 39 add-distance
s" Tambi"         s" Norrath" 113 add-distance
s" Tambi"         s" Straylight" 130 add-distance
s" Tambi"         s" Tristram" 35 add-distance
s" Tambi"         s" Arbre" 40 add-distance
s" Faerun"        s" Norrath" 63 add-distance
s" Faerun"        s" Straylight" 21 add-distance
s" Faerun"        s" Tristram" 57 add-distance
s" Faerun"        s" Arbre" 83 add-distance
s" Norrath"       s" Straylight" 9 add-distance
s" Norrath"       s" Tristram" 50 add-distance
s" Norrath"       s" Arbre" 60 add-distance
s" Straylight"    s" Tristram" 27 add-distance
s" Straylight"    s" Arbre" 81 add-distance
s" Tristram"      s" Arbre" 90 add-distance

: route-length ( -- n )
  0 set-size @ 1 do
    i 1- set-ref @ 
    i    set-ref @ 
    distance +
  loop ;

variable min-route-length
variable max-route-length

: update-min-route-length ( n -- )
  min-route-length dup @
  rot min swap ! ;

: update-max-route-length ( n -- )
  max-route-length dup @
  rot max swap ! ;

: find-min-max-route-length ( -- imin,max ) 
  8 initial
  100000 min-route-length !
  -100000 max-route-length !
  begin
    route-length
    dup update-min-route-length
    update-max-route-length
    next-permutation
    0 >= while
  repeat
  min-route-length @ 
  max-route-length @ ;

