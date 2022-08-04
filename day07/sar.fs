500 constant max-connections
create connections max-connections cells allot
variable next-connection
connections next-connection !

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
6   constant lshift-gate
7   constant rshift-gate

: not ( u64 -- u64 )
  -1 xor ;

: mask ( size -- mask )
  1 swap lshift 1- ;

: clear ( cell,offset,size -- cell' )
  mask swap lshift not and ;

: <-! ( cell,value,offset,size -- cell' )
  2>r swap 2r@ clear swap
  r> mask and
  r> lshift or ;

: -> ( cell,offset,size -- value )
  mask -rot rshift swap and ;

 0 16 2constant cnx-input1
16 16 2constant cnx-input2
32  2 2constant cnx-size
34  1 2constant cnx-input1-type
35  1 2constant cnx-input2-type
36  3 2constant cnx-gate
48 16 2constant cnx-output

create gates 
  ' noop , ' not  , ' and    , ' or     , 
  ' noop , ' noop , ' lshift , ' rshift ,

: gate ( u3 -- xt )
  cells gates + @ ;


: string>output ( addr,l -- u16 )
  over c@
  8 lshift -rot
  1 > if 1+ c@ else drop 0 then
  or ;

: #connections ( -- n )
  next-connection @ connections - cell / ;

: add-connection ( cnx -- )
  next-connection @ !
  cell next-connection +! ;

: find-connection ( u16 -- addr,T,F )
  #connections if 
    false swap
    next-connection @ connections do
      i @ cnx-output -> over = if
        nip i true rot
      then
    cell +loop
    drop
  else
    drop false
  then ;

: connection! ( cnx -- )
  dup cnx-output -> find-connection if
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
  dup cnx-input1 ->
  swap cnx-input1-type -> wired = if
    connection eval-rec
  then ;

: eval-simple ( cnx -- n )
  dup input1
  swap cnx-gate -> 
  gate execute ;

: eval-double ( cnx - n )
  dup input1                                \ cnx,in1
  over cnx-input2 ->                    \ cnx,in1,u16
  rot swap over                             \ in1,cnx,u16,cnx
  cnx-input2-type -> wired = if         
    connection eval-rec
  then                                      \ in1,cnx,in2
  swap cnx-gate gate execute ;

: eval ( cnx -- n )
  dup cnx-size -> 3 < 
  if eval-simple else eval-double then ;

' eval is eval-rec
