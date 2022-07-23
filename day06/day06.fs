1024 constant max-y
1024 constant max-x

create grid max-y max-x * allot

: init-grid
  grid max-y max-x * erase ;

: offset ( x,y -- addr )
  max-y * + grid + ;

: turn-on-1 ( x,y -- )
  offset 1 swap c! ;

: turn-off-1 ( x,y -- )
  offset 0 swap c! ;

: toggle-1 ( x,y -- )
  offset dup c@ 1 swap - swap c! ;

: light ( x,y -- n )
  offset c@ ;

: turn-on-2 ( x,y -- )
  offset dup c@ 
  dup 255 = if s" too bright" exception throw then
  1+ swap c! ;

: turn-off-2 ( x,y -- )
  offset dup c@ 1- 0 max
  swap c! ;

: toggle-2 ( x,y -- )
  offset dup c@
  dup 254 = if s" too bright" exception throw then
  2 + swap c! ;

defer turn-on
defer turn-off
defer toggle

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
    do j i toggle loop
  loop 2drop ;

: count-on ( -- n )
  0 max-x 0 do max-y 0 do j i light + loop loop ;

' turn-on-1 is turn-on
' turn-off-1 is turn-off
' toggle-1 is toggle

