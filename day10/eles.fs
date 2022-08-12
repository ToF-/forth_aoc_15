\ elves look elves say

65536 65536 * constant max-output
variable output-ref
variable input-ref

max-output allocate throw output-ref !
max-output allocate throw input-ref !

: output ( -- addr )
  output-ref @ ;

: input ( -- addr )
  input-ref @ ;

: c>output ( c -- )
  output dup c@ 1+ 
  2dup swap c!
  + c! ;

: say>ouput ( c,n -- )
  ?dup if
    [char] 0 + c>output
    c>output 
  else
    drop
  then ;

: look-and-say ( addr,l -- )
  output max-output erase
  [char] * 0 2swap
  bounds do
    over i c@ <> if
      say>ouput 
      i c@ 1
    else
      1+
    then
  loop say>ouput ;

: iterate ( addr,l,n -- )
  -rot
  dup input c!
  input 1+ swap cmove
  0 do
    input count look-and-say
    output count dup input c! 
    input 1+ swap cmove
  loop ;

    
  
