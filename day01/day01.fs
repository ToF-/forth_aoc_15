10000 constant maxpuzzle
create puzzle maxpuzzle allot
variable track
-1 constant basement

: track-basement ( floor,n -- )
  over basement = track @ 0= and if
    track !
  else
    drop
  then
  drop ;

: solve-first-half ( l -- n )
  0 puzzle
  rot 0 do
    dup c@
    dup [char] ( = if
      drop 
      swap 1+ swap
    else [char] ) = if
      swap 1- 
      dup i track-basement
      swap
    then then
    1+
  loop drop ;

: solve-second-half 
  track @ 1+ ;

0 track !
puzzle maxpuzzle accept
solve-first-half 
cr . cr
solve-second-half 
cr . cr
