500 constant max-connections
create connections max-connections cells allot
variable #connections

0  constant signal 
1  constant wired

16 constant u16-offset
u16-offset 3 * constant output-offset
u16-offset 2 * constant descriptor-offset
0 constant empty-connection
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

create gates 
  ' noop , ' not  , ' and    , ' or     , 
  ' noop , ' noop , ' lshift , ' rshift ,

: gate ( u3 -- xt )
  cells gates + @ ;

: connection>descriptor ( cnx -- desc )
  descriptor-offset rshift 
  u7mask and ;

: connection>input-type-1 ( cnx -- f )
  connection>descriptor
  1 and ;
  
: connection>input-1 ( cnx -- u16 )
  u16mask and ;

: connection>output ( cnx -- u16 )
  output-offset rshift u16mask and ;

: connection>gate-type ( cnx -- u3 )
  connection>descriptor 
  3 rshift u3mask and ;

: connection>input1! ( cnx,u16 -- cnx )
  or ;
  
: connection>output! ( cnx,u16 -- cnx )
  swap u16mask output-offset lshift not and
  swap output-offset lshift or ;

: string>output ( addr,l -- u16 )
  over c@
  8 lshift -rot
  1 > if 1+ c@ else drop 0 then
  or ;

: connection>descriptor ( cnx -- u7 )
  descriptor-offset rshift 127 and ;

: connection>descriptor! ( cnx,desc -- cnx)
  127 descriptor-offset lshift not
  rot and 
  swap descriptor-offset lshift or ;

: make-simple-connection ( addr,l,u16 -- cnx )
  empty-connection 
  swap connection>input1!
  -rot string>output connection>output! ;

: make-unary-connection ( addr,l,u16,g -- cnx )
  3 lshift 
  empty-connection swap
  connection>descriptor!
  swap connection>input1!
  -rot string>output connection>output! ;

: eval ( cnx -- u16 )
  dup  connection>input-1 
  swap connection>gate-type gate execute ;
