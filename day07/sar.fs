500 constant max-connections
create connections max-connections cells allot
variable #connections

0  constant signal 
1  constant wired

16 constant u16-offset
u16-offset 3 * constant output-offset
u16-offset 2 * constant descriptor-offset
0 constant new-connection
#connections off
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

: <field! ( cell,value,offset,size -- cell' )
  2>r swap 2r@ clear swap
  r> mask and
  r> lshift or ;

: >field ( cell,offset,size -- value )
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


: eval ( cnx -- value )
  cnx-input1 >field ;

