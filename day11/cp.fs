\ Corporate Policy 

: (increment) ( addr,addr' -- )
  2dup <= if
    dup
    c@ dup [char] z < if
      1+ swap c! drop
    else
      drop [char] a
      over c!
      1- recurse
    then
  else
    2drop
  then ;

: increment ( addr,l -- )
  1- over + (increment) ;
