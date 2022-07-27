
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

create operators 80 allot
operators 80 erase

: init-operators
  s\" \3NOT\3AND\2OR\6LSHIFT\6RSHIFT\2->"
  0 do
    dup i + c@
    operators i + c!
  loop drop ;

init-operators

: nth-string ( n,addr -- addr )
  swap 0 ?do count + loop ;

: operator ( n -- )
  operators nth-string ;

: is-operator? ( addr,l -- n,T|F )
  false -rot 6 0 do 
    i operator count 2over compare 0= if
      rot drop i true 2swap leave
    then
  loop 
  2drop ;

: is-number? ( addr,l -- )
  s>unumber?  if d>s true else 2drop false then ;

: s>copy ( addr,l,dest -- )
  over over c!
  1+ swap cmove ;

: init-steps
    steps step-size erase ;

: nth-step-ref ( n, addr )
    step-size * steps + ;

: nth-step ( n -- addr,l )
    nth-step-ref count ;

: ?nip ( n,T|F -- f )
  dup if nip then ;

: nth-step-cmove ( addr,l,n -- )
  >r 
  2dup 
  2dup is-number?   ?nip
  -rot is-operator? ?nip or 
  r> swap if
    nth-step-ref s>copy
  else
    -rot
    s" _" pad place
    pad +place
    s" _" pad +place
    pad count
    rot nth-step-ref s>copy
  then ;

: parse-names ( n -- n )
  0 swap
  0 do
    parse-name 
    dup if rot 1+ -rot then
    i nth-step-cmove
  loop 
  dup steps# ! ;

: instruction>steps ( addr,l -- )
  init-steps
  s" 5 parse-names " pad place
  pad +place
  s\" \n" pad +place
  pad count evaluate ;

: step>output ( n -- )
  output-ref s>copy ;


: signal-ref ( n -- addr )
  signals nth-string ;

: signal ( n -- addr,l )
  signal-ref count ;

: output ( -- addrl,l )
  output-ref count ;

: step>signals ( n -- )
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

