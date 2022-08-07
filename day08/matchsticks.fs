char \ constant ESC
: code-length ( addr,l -- l )
  nip ;

: (mem-length) ( addr,l -- l )
  0 0 2swap
  bounds do
    dup ESC = i c@ [char] x = and if
      drop 0
      2
    else
      i c@ ESC <> over ESC = or if
        swap 1+ swap
      then
      drop i c@
      1
    then +loop
  drop ;

 : mem-length ( addr,l -- l )
  2 - swap 1+ swap
  dup if
    (mem-length)
  else
    nip
  then ;

80 constant line-size
create line line-size allot

: encoded-length ( addr,l -- )
  2dup dump
  0 -rot
  bounds do
    i c@ dup [char] " = 
    swap ESC = or if 
      2 + 
    else 
      1+ 
    then
  loop 2 + ; 

0 value fd-in
: overhead ( addr,l -- )
  r/o open-file throw to fd-in
  0 0
  begin
    line line-size fd-in read-line throw while
    line swap
    2dup code-length -rot
    mem-length 
    rot + 
    -rot + 
    swap
  repeat drop
  fd-in close-file throw 
  - ;

: encoded ( addr,l -- )
  r/o open-file throw to fd-in
  0 0
  begin
    line line-size fd-in read-line throw while
    line swap
    2dup encoded-length -rot
    code-length
    rot + 
    -rot + 
    swap
  repeat drop
  fd-in close-file throw 
  - ;
