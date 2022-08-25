\ JSabacusFramework.io

1   constant JS-LIST
2   constant JS-OBJECT
4   constant JS-STRING
128 constant RED
3 constant str
4 constant nbr

80 constant string-max
create current-string-value string-max
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

: last-string-is-red? ( -- f )
  last-string @ last-string-len @ 
  s" red" compare 0= ;

: parse-json ( addr,l -- ... )
  bounds do
    i c@ dup [char] { = if drop 
      JS-OBJECT 0
      1
      expect-value? off
    else dup [char] } = if drop
      swap assert( JS-OBJECT or )
      1
    else dup [char] [ = if drop
      JS-LIST 0 
      1
      expect-value? off
    else dup [char] ] = if drop 
      swap assert( JS-LIST = )
      1
    else dup is-digit? over is-minus? or if drop
      i js-number 
      >r + r>
    else dup [char] " = if drop
      in-a-string? 0= if
        JS-STRING 0
        i 1+ last-string !
        last-string-len off
        1
      else ( ending a string )
        i last-string @ - last-string-len !
        drop assert( JS-STRING = )
        over JS-OBJECT and 
        expect-value? @ and
        last-string-is-red? and if
          swap RED or swap 
        then
        1
      then
    else dup [char] : = if drop 
      expect-value? on
      1
    else
      drop
      1
    else dup [char] , = if drop
      1
    then then then then then then then then
  +loop ;

: extract-number ( addr,l -- n )
  false 0 2swap
  ?dup if
    bounds do
      i c@ dup is-digit? if
        [char] 0 - 
        swap 10 * +
      else
        is-minus? if
          nip true swap
        then
      then
    loop
  else
    drop
  then
  swap if negate then ;

80 constant line-size
create line line-size allot

0 value fd-in

: sum ( addr,l -- n )
  r/o open-file throw to fd-in
  0
  begin
    line line-size fd-in read-line throw while
    line swap extract-number +
  repeat
  fd-in close-file throw drop ;
