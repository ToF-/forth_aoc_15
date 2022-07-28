
( Advent of Code 2015 Day 07 )
( ~~~~~~~~~~~~~~~~~~~~~~~~~~ )
( ~ Some Assembly Required ~ )

5  constant max-steps
10 constant step-size
step-size max-steps * constant steps-size
create steps steps-size allot
create sar-output 10 allot
create sar-signals 30 allot
variable steps#

: NOT ( n - n )
  -1 xor 65535 and ;

create sar-code 80 allot
create sar-temp 80 allot

: sar-forth ( addr, l )
  sar-code count ;
: forth|
  sar-code 80 erase ;

create operators 80 allot
operators 80 erase

: init-operators
  s\" \3NOT\3AND\2OR\6LSHIFT\6RSHIFT\2->"
  0 do
    dup i + c@
    operators i + c!
  loop drop ;

init-operators

: #string ( n,addr -- addr )
  swap 0 ?do count + loop ;

: operator ( n -- )
  operators #string ;

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

: s>empty ( addr )
  dup c@ erase ;

: s>copy ( addr,l,dest -- )
  over over c!
  1+ swap cmove ;

: s>append ( addr,l,dest -- )
  dup c@               \ addr,l,dest,k
  rot 2dup +           \ addr,dest,k,l,k+l
  2swap over           \ addr,l,k+l,dest,k,dest
  2swap c!             \ addr,l,k,dest
  + 1+ swap cmove ;

: s>prepend ( addr,l,dest -- )
  dup count pad s>copy
  dup >r s>copy 
  pad count r> s>append ;

: init-steps
    steps step-size erase ;

: #step-ref ( n, addr )
    step-size * steps + ;

: #step ( n -- addr,l )
    #step-ref count ;

: #step-cmove ( addr,l,n -- )
  #step-ref s>copy ;

: #step>copy ( addr,l,n -- )
  over if #step-cmove else drop 2drop then ;

: parse-names ( n -- n )
  0 swap
  0 do
    parse-name 
    dup if rot 1+ -rot then
    i #step>copy
  loop 
  steps# ! ;

: instruction>steps ( addr,l -- )
  init-steps
  s" 5 parse-names " pad place
  pad +place
  s\" \n" pad +place
  pad count 
  evaluate ;

: step>output ( n -- )
  sar-output s>copy ;


: #sar-signal-ref ( n -- addr )
  sar-signals #string ;

: #signal ( n -- addr,l )
  #sar-signal-ref count ;

: output ( -- addrl,l )
  sar-output count ;

: arrange-steps ( -- )
  steps# @
  dup 3 = if drop
    0 #step 0 #sar-signal-ref s>copy
    2 #step sar-output s>copy
  else 4 = if
    1 #step 0 #sar-signal-ref s>copy
    0 #step 1 #sar-signal-ref s>copy
    3 #step sar-output s>copy
  else 
    0 #step 0 #sar-signal-ref s>copy
    2 #step 1 #sar-signal-ref s>copy
    1 #step 2 #sar-signal-ref s>copy
    4 #step sar-output s>copy
  then then ;


: forth> ( addr,l ) 
  sar-code s>copy ;
: forth>> ( addr,l )
  sar-code s>append ;
: >forth ( addr,l )
  sar-code s>prepend ;

: forth_> ( addr,l )
  s" _" forth>> ;

: forth>:
  s" : _" forth> ;

: forth>:_
  s" : __" forth> ;

: forth>>bl
  s"  " forth>> ;

: forth>>sep
  forth>> forth>>bl ;

: forth>defer ( addr,l -- )
  sar-temp 80 erase
  s" DEFER _" sar-temp s>copy
  sar-temp s>append
  s"  " sar-temp s>append
  sar-temp count >forth ;

: already-defined ( addr,l -- )
  sar-temp 80 erase
  sar-temp s>copy
  s" _" sar-temp s>prepend
  sar-temp count find-name ;


: forth>>word ( addr,l )
  2dup is-symbol? if
    2dup already-defined 0= if 2dup forth>defer then
    forth_> forth>>
  else
    forth>>
  then
  forth>>bl ;

variable 1st-declaration
create temp 10 allot

: forth>>;
  1st-declaration @ if
    s" ;" forth>>
  else
    s" ; ' __" forth>> output forth>> s"  is _" forth>> output forth>>
  then ;

: forth>>:output ( addr,l )
  temp 10 erase
  s" _" temp s>copy
  output temp s>append
  temp count
  find-name 0= dup 1st-declaration !
  if forth>: else forth>:_ then output forth>>sep ;
  

: 3-points
  forth| forth>>:output 
  0 #signal forth>>word 
  forth>>; ;

: 4-points
  forth| forth>>:output 
  0 #signal forth>>word 
  1 #signal forth>>word 
  forth>>; ;

: 5-points
  forth| forth>>:output 
  0 #signal forth>>word 
  1 #signal forth>>word 
  2 #signal forth>>word 
  forth>>; ;

: forth>>words ( n -- )
  forth| forth>>:output
  steps# @ 2 - 0 do
    i #signal forth>>word
  loop forth>>; ;

: sar>forth ( addr,l -- )
  instruction>steps 
  arrange-steps 
  forth>>words 
  sar-forth type cr ;

: sar-value ( addr,l -- n )
  forth|  forth_> forth>>
  sar-forth evaluate ;


