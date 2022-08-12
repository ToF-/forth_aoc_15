\ elves look elves say
\ gforth -m 1G 
10000000 constant max-output
create output max-output allot
create input  max-output allot
variable output-size
variable input-size

: c>output ( c -- )
  output output-size @ + c!
  1 output-size +! ;

: say>ouput ( c,n -- )
  ?dup if
    [char] 0 + c>output
    c>output 
  else
    drop
  then ;

: look-and-say ( addr,l -- )
  output max-output erase
  output-size off
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
  dup input-size !
  input swap cmove
  0 do
    input input-size @ look-and-say
    output output-size @ dup input-size !
    input swap cmove 
  loop  ;

    
  
