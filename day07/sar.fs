5 constant max-steps
10 constant step-size
step-size max-steps * constant steps-size
create steps steps-size allot

: init-steps
    steps step-size erase ;

: nth-step-ref ( n, addr )
    step-size * steps + ;

: nth-step ( n -- addr,l )
    nth-step-ref count ;

: nth-step-cmove ( addr,l,n -- )
    nth-step-ref
    over over c!
    1+ swap cmove ;

: parse-names ( n -- n )
  0 swap
  0 do
    parse-name 
    dup if rot 1+ -rot then
    i nth-step-cmove
  loop ;

: instruction>steps ( addr,l -- )
  init-steps
  s" 5 parse-names " pad place
  pad +place
  s\" \n" pad +place
  pad count evaluate ;


: symbol ( addr,l -- addr,l )
  s" _" pad place
  pad +place 
  pad count ;

: assign ( addr,l,addr,l -- addr,l )
  2>r
  s" : ->" pad place
  2r@ pad +place
  s" " pad +place
  pad +place
  s"  ; '->" pad +place
  2r@ pad +place
  s"  is "
  2r> pad +place
  pad count ;

