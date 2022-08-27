\ JSabacusFramework.io

1   constant JS-LIST
2   constant JS-OBJECT
4   constant JS-STRING
128 constant RED
3 constant str
4 constant nbr

80 constant string-max
create current-string-value string-max allot
: openJS-OBJECTect ( -- O,0 )
  JS-OBJECT 0 ;

: closeJS-OBJECTect ( C,n -- n|0 )
  swap red = if
    drop 0
  then ;

: openList ( -- C,0 )
  JS-LIST 0 ;

: closeList ( -- n )
  nip ;

: comma ( C,n -- C,n' )
  + ;

: quote ( C,n -- C,n )
;

: in-a-list? ( ... -- f )
  over JS-LIST = ;

: is-digit? ( c -- f )
  dup [char] 0 >=
  swap [char] 9 <= and ;

: is-minus? ( c -- f )
  [char] - = ;

: (js-number) ( addr -- n,addr' )
  0 swap
  begin
    dup c@ dup is-digit? while
      [char] 0 - 
      rot 10 * + swap
    1+
  repeat drop ;

: js-number ( addr -- n,l )
  dup 
  dup c@ is-minus? dup if swap 1+ swap then >r
  (js-number)
  r> if swap negate swap then
  swap -rot swap - ;

: is-quote? ( c -- f )
  [char] " = ;

: in-a-string? ( ... -- f )
  over JS-STRING = ;

  
variable last-string
variable last-string-len
variable expect-value?
variable exclude-red

exclude-red off

: last-string-is-red? ( -- f )
  last-string @ last-string-len @ 
  s" red" compare 0= ;

1 constant next-byte

: parse-json ( addr,l -- ... )
  bounds do
    i c@ dup [char] { = if drop 
      JS-OBJECT 0
      expect-value? off
      next-byte
    else dup [char] } = if drop
      swap dup assert( JS-OBJECT or )
      RED and exclude-red @ and if drop 0 then
      +
      next-byte
    else dup [char] [ = if drop
      JS-LIST 0 
      expect-value? off
      next-byte
    else dup [char] ] = if drop 
      swap assert( JS-LIST = )
      +
      next-byte
    else dup is-digit? over is-minus? or if drop
      i js-number 
      >r
      +
      r>
    else dup [char] " = if drop
      in-a-string? 0= if
        JS-STRING 0
        i 1+ last-string !
        last-string-len off
        next-byte
      else ( ending a string )
        i last-string @ - last-string-len !
        drop assert( JS-STRING = )
        over JS-OBJECT and 
        expect-value? @ and
        last-string-is-red? and if
          swap RED or swap 
        then
        next-byte
      then
    else dup [char] : = if drop 
      expect-value? on
      next-byte
    else
      drop
      next-byte
    else dup [char] , = if drop
      next-byte
    then then then then then then then then
  +loop ;

: init-parser ( addr,l -- 0,addr,l )
  0 -rot ;

80 constant line-size
create line line-size allot

0 value fd-in

: sum ( addr,l -- n )
  r/o open-file throw to fd-in
  0
  begin
    line line-size erase
    line line-size fd-in read-line throw while
    line swap 
    2dup type ."   :" 
    parse-json
    dup . cr
  repeat
  fd-in close-file throw drop ;

