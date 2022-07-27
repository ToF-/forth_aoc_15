
( Advent of Code 2015 Day 07 )
( ~~~~~~~~~~~~~~~~~~~~~~~~~~ )
( ~ Some Assembly Required ~ )

5 constant max-steps
10 constant step-size
step-size max-steps * constant steps-size
create steps steps-size allot
create output-ref 10 allot
create signals 30 allot
variable steps#

create forth-instruction-ref 80 allot

: init-forth-instruction
  forth-instruction-ref 80 erase ;

: forth-instruction ( -- addr,l )
  forth-instruction-ref count ;

create operators 80 allot
operators 80 erase

: init-operators
  s\" \3NOT\3AND\2OR\6LSHIFT\6RSHIFT\2->\5 NOT \5 AND \4 OR \8 LSHIFT \8 RSHIFT \4 -> "
  0 do
    dup i + c@
    operators i + c!
  loop drop ;

init-operators

: nth-string ( n,addr -- addr )
  swap 0 ?do count + loop ;

: operator ( n -- )
  operators nth-string ;

: is-operator? ( addr,l -- f )
  false -rot 12 0 do 
    i operator count 2over compare 0= if
      rot drop true -rot leave
    then
  loop 
  2drop ;

: is-digit? ( c -- f )
  dup [char] 0 >= swap [char] 9 <= and ;

: is-number? ( addr,l -- f )
  true -rot
  over + swap do
    i c@ dup is-digit? 0= if
      bl <> if drop false then
    else 
      drop
    then
  loop ;

: is-symbol? ( addr,l -- f )
  2dup is-number? -rot is-operator? or 0= ;

: s>copy ( addr,l,dest -- )
  over over c!
  1+ swap cmove ;

: init-steps
    steps step-size erase ;

: nth-step-ref ( n, addr )
    step-size * steps + ;

: nth-step ( n -- addr,l )
    nth-step-ref count ;

: nth-step-cmove ( addr,l,n -- )
  >r 
  2dup 
  2dup is-number?   
  -rot is-operator? or 
  r> swap if
    -rot
    s"  " pad place
    pad +place
    s"  " pad +place
    pad count
    rot nth-step-ref s>copy
  else
    -rot
    s" _" pad place
    pad +place
    s" _ " pad +place
    pad count
    rot nth-step-ref s>copy
  then ;

: nth-step>copy ( addr,l,n -- )
  over if nth-step-cmove else drop 2drop then ;

: parse-names ( n -- n )
  0 swap
  0 do
    parse-name 
    dup if rot 1+ -rot then
    i nth-step>copy
  loop 
  dup steps# ! ;

: instruction>steps ( addr,l -- )
  init-steps
  s" 5 parse-names " pad place
  pad +place
  s\" \n" pad +place
  pad count 
  evaluate ;

: step>output ( n -- )
  output-ref s>copy ;


: signal-ref ( n -- addr )
  signals nth-string ;

: signal ( n -- addr,l )
  signal-ref count ;

: output ( -- addrl,l )
  output-ref count ;

: arrange-steps ( n -- )
  dup 3 = if drop
    0 nth-step 0 signal-ref s>copy
    2 nth-step output-ref s>copy
  else 4 = if
    1 nth-step 0 signal-ref s>copy
    0 nth-step 1 signal-ref s>copy
    3 nth-step output-ref s>copy
  else 
    0 nth-step 0 signal-ref s>copy
    2 nth-step 1 signal-ref s>copy
    1 nth-step 2 signal-ref s>copy
    4 nth-step output-ref s>copy
  then then ;

: instruction>forth ( addr,l -- )
  instruction>steps
  arrange-steps
  init-forth-instruction
  s" : " pad place
  output pad +place
  0 signal pad +place
  s" ;" pad +place
  pad count forth-instruction-ref s>copy
  ;

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

