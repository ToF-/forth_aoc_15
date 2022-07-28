
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

: f|
  forth-instruction-ref 80 erase ;

: forth-instruction ( -- addr,l )
  forth-instruction-ref count ;

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

: is-operator? ( addr,l -- f )
  false -rot 6 0 do 
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

: s>append ( addr,l,dest -- )
  dup c@               \ addr,l,dest,k
  rot 2dup +           \ addr,dest,k,l,k+l
  2swap over           \ addr,l,k+l,dest,k,dest
  2swap c!             \ addr,l,k,dest
  + 1+ swap cmove ;

: init-steps
    steps step-size erase ;

: nth-step-ref ( n, addr )
    step-size * steps + ;

: nth-step ( n -- addr,l )
    nth-step-ref count ;

: nth-step-cmove ( addr,l,n -- )
  nth-step-ref s>copy ;

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


: f> ( addr,l ) 
  forth-instruction-ref s>copy ;
: f>> ( addr,l )
  forth-instruction-ref s>append ;

: f>:
  s" : _" f> ;

: f>:_
  s" : __" f> ;

: f>>bl
  s"  " f>> ;

: f>>sep
  f>> f>>bl ;

: f>>word ( addr,l )
  2dup is-symbol? if
  else
    f>> f>>bl
  then ;

variable 1st-declaration
create temp 10 allot

: f>>;
  1st-declaration @ if
    s" ;" f>>
  else
    s" ; ' __" f>> output f>> s"  is _" f>> output f>>
  then ;

: f>>:output ( addr,l )
  temp 10 erase
  s" _" temp s>copy
  output temp s>append
  temp count
  find-name 0= dup 1st-declaration !
  if f>: else f>:_ then output f>>sep ;
  
: 3-terms-instruction
  f| f>>:output 0 signal f>>word f>>; ;

: 4-terms-instruction
  f| f>>:output 0 signal f>>word 1 signal f>>word f>>; ;

: 5-terms-instruction
  f| f>>:output 0 signal f>>word 1 signal f>>word 2 signal f>>word f>>; ;

: instruction>forth ( addr,l -- )
  instruction>steps
  arrange-steps
  steps# @ dup 3 = if drop 3-terms-instruction
  else 4 = if 4-terms-instruction
  else 5-terms-instruction
  then then
;
