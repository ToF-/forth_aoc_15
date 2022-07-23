1024 constant max-y
1024 8 / constant max-x

create grid max-y max-x * allot

: init-grid
  grid max-y max-x * erase ;

: bit-value ( b -- v )
  1 swap lshift ;

: offset ( x,y -- addr,v )
  max-x * over 8 / + grid +
  swap 8 mod bit-value ;

: inverse ( v -- v )
  -1 xor 255 and ;

: turn-on ( x,y -- )
  offset over c@ or swap c! ;

: turn-off ( x,y -- )
  offset inverse
  over c@ and swap c! ;

: light ( x,y -- n )
  offset swap c@ and 0 > 1 and ;

: turn-on-through ( x0,y0,x1,y1 -- )
  1+ rot                 \ x0,x1,y1+1,y0
  2swap 1+ swap          \ y1+1,y0,x1+1,x0
  do 2dup                \ y1+1,y0,y+1,y0
    do j i turn-on loop
  loop 2drop ;

: turn-off-through ( x0,y0,x1,y1 -- )
  1+ rot                 \ x0,x1,y1+1,y0
  2swap 1+ swap          \ y1+1,y0,x1+1,x0
  do 2dup                \ y1+1,y0,y+1,y0
    do j i turn-off loop
  loop 2drop ;

: toggle-through ( x0,y0,x1,y1 -- )
  1+ rot                 \ x0,x1,y1+1,y0
  2swap 1+ swap          \ y1+1,y0,x1+1,x0
  do 2dup                \ y1+1,y0,y+1,y0
    do j i light if
        j i turn-off
      else 
        j i turn-on
      then
    loop
  loop 2drop ;

: count-on ( -- n )
  0 
  max-x 0 do
    max-y 0 do
      j i light if
        1+
      then
    loop 
  loop ;
