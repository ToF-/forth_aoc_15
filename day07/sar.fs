500 constant max-connections
create connections max-connections cells allot
variable next-connection

: init-connections
  connections next-connection ! ;


init-connections

0  constant signal 
1  constant wired

16 constant u16-offset
u16-offset 3 * constant output-offset
u16-offset 2 * constant descriptor-offset
0 constant new-connection


65535 constant u16mask 
127   constant u7mask
7     constant u3mask

0   constant noop-gate
1   constant not-gate
2   constant and-gate
3   constant or-gate
4   constant lshift-gate
5   constant rshift-gate
6   constant assign-op

: not ( u64 -- u64 )
  -1 xor ;

: u16not ( u16 -- u16 )
  not u16mask and ;

: mask ( size -- mask )
  1 swap lshift 1- ;

: clear ( cell,offset,size -- cell' )
  mask swap lshift not and ;

: bf! ( cell,value,offset,size -- cell' )
  2>r swap 2r@ clear swap
  r> mask and
  r> lshift or ;

: bf@ ( cell,offset,size -- value )
  mask -rot rshift swap and ;

 0 16 2constant cnx-input1
16 16 2constant cnx-input2
32  2 2constant cnx-size
34  1 2constant cnx-input1-type
35  1 2constant cnx-input2-type
36  3 2constant cnx-gate
48 16 2constant cnx-output

create gates 
  ' noop , ' u16not  , ' and    , ' or     , 
  ' lshift , ' rshift , ' noop ,

: gate ( u3 -- xt )
  cells gates + @ ;


: s>key ( addr,l -- u16 )
  over c@
  8 lshift -rot
  1 > if 1+ c@ else drop 0 then
  or ;

: .key ( u16 -- )
  256 /mod emit emit ;

: #connections ( -- n )
  next-connection @ connections - cell / ;

: add-connection ( cnx -- )
  next-connection @ !
  cell next-connection +! ;

: find-connection ( u16 -- addr,T,F )
  #connections if 
    false swap
    next-connection @ connections do
      i @ cnx-output bf@ over = if
        nip i true rot
      then
    cell +loop
    drop
  else
    drop false
  then ;

: connection! ( cnx -- )
  dup cnx-output bf@ find-connection if
    !
  else
    add-connection
  then ;
  
: connection ( u16 -- cnx )
  find-connection if
    @ 
  else
    s" connection not found" exception throw
  then ;

defer eval-rec

: input1 ( cnx - u16 )
  dup cnx-input1 bf@
  swap cnx-input1-type bf@ wired = if
    connection eval-rec
  then ;

: eval-simple ( cnx -- n )
  dup input1
  swap cnx-gate bf@ 
  gate execute ;

: eval-double ( cnx - n )
  dup input1                                \ cnx,in1
  over cnx-input2 bf@                    \ cnx,in1,u16
  rot swap over                             \ in1,cnx,u16,cnx
  cnx-input2-type bf@ wired = if         
    connection eval-rec
  then                                      \ in1,cnx,in2
  swap cnx-gate bf@ 
  gate execute  ;

: eval ( cnx -- n )
  dup dup cnx-size bf@ 3 < 
  if eval-simple else eval-double then 
  dup -rot
  cnx-input1 bf!
  1 cnx-size bf! 
  signal cnx-input1-type bf!
  noop-gate cnx-gate bf! 
  connection! ;

' eval is eval-rec

5  constant max-steps
10 constant step-size
step-size max-steps * constant steps-size
create steps steps-size allot
variable steps#

: s>copy ( addr,l,dest -- )
  over over c!
  1+ swap cmove ;
    
: init-steps
  steps# off
  steps steps-size erase ;

: #step-ref ( n -- addr )
  step-size * steps + ;

: #step ( n -- addr,l )
  #step-ref count ;

: #step-cmove ( addr,l,n -- )
  #step-ref s>copy  ;

: #step>copy ( addr,l,n -- )
  over if #step-cmove else drop 2drop then ;

: parse-steps 
  0 swap
  5 0 do 
    parse-name
    dup if rot 1+ -rot then
    i #step>copy
  loop steps# ! ;

: prepare-entry ( addr,l -- )
  s" parse-steps " pad place
  pad +place
  s\" \n" pad +place ;

: s>steps ( addr,l -- )
  init-steps
  prepare-entry
  pad count evaluate ;

0 constant number-step
1 constant wire-step
2 constant gate-step
3 constant output-step

create operators 80 allot
operators 80 erase


: init-operators
  s\" \4NOOP\3NOT\3AND\2OR\6LSHIFT\6RSHIFT\2->"
  0 do
    dup i + c@
    operators i + c!
  loop drop ;

init-operators

: #string ( n,addr -- addr )
  swap 0 ?do count + loop ;

: operator ( n -- )
  operators #string ;

: is-operator? ( addr,l -- i,T|F )
  false -rot 7 0 do 
    i operator count 2over compare 0= if
      rot drop i true 2swap leave
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

: parse-step ( addr,l -- n,0|k|1  )
  2dup is-number? if
    s>number d>s number-step
  else 2dup is-operator? if
    -rot 2drop  gate-step
  else
    s>key wire-step
  then then ;

: .cnx 
  dup 2 base ! u. decimal cr
  hex u. decimal cr ;

: cnx-output! ( cnx -- cnx' )
  steps# @ 1- #step
  parse-step assert( wire-step = )
  cnx-output bf! ;

: cnx-signal1! ( cnx,value -- cnx' )
  cnx-input1 bf!
  signal cnx-input1-type bf! ;

: cnx-wired1! ( cnx,value -- cnx' )
  cnx-input1 bf!
  wired cnx-input1-type bf! ;

: cnx-input1! ( cnx,addr,l -- cnx' )
  parse-step number-step = 
  if cnx-signal1!  else cnx-wired1!  then ;
  
: cnx-signal2! ( cnx,value -- cnx' )
  cnx-input2 bf!
  signal cnx-input2-type bf! ;

: cnx-wired2! ( cnx,value -- cnx' )
  cnx-input2 bf!
  wired cnx-input2-type bf! ;

: cnx-input2! ( cnx,addr,l -- cnx' )
  parse-step number-step = 
  if cnx-signal2!  else cnx-wired2!  then ;
  
: cnx-binary-gate! ( cnx,addr,l -- cnx' )
  parse-step assert( gate-step = )
  cnx-gate bf! ;

: steps>cnx ( addr,l -- cnx )
  s>steps
  new-connection
  cnx-output!
  steps# @ 3 = if 
    1 cnx-size bf!
    0 #step cnx-input1!
  else steps# @ 4 = if
    2 cnx-size bf!
    1 #step cnx-input1!
    not-gate cnx-gate bf!
  else
    3 cnx-size bf!
    0 #step cnx-input1!
    2 #step cnx-input2!
    1 #step cnx-binary-gate!
  then then
  dup connection! ;

80 constant instruction-size
create instruction instruction-size allot

0 value fd-in
: read-instructions ( addr,l -- )
  init-connections
  r/o open-file throw to fd-in
  begin
    instruction instruction-size fd-in read-line throw while
    instruction swap
    steps>cnx
    drop
  repeat drop
  fd-in close-file throw ;

