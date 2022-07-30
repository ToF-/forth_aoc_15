500 constant max-connections
create connections max-connections cells allot
variable #connections

0 constant conn-empty
#connections off
65535 constant u16mask 

: string>pname ( addr,l -- u16)
  over c@        
  8 lshift -rot  \ encode 1st char
  1 > if 
    1+ c@        \ encode 2nd char
  else 
    drop 0 
  then or ;

: s>empty ( addr -- )
  0 swap c! ;

: s>cons ( c,addr -- )
    dup c@ 1+            \ increment length
    over over
    swap c!              \ write length
    + c! ;               \ append c at end of addr
  
: pname>string ( u16,addr )
  dup s>empty
  swap 256 /mod          \ decode c1 and c2
  rot swap over          \ c2,addr,c1,addr
  s>cons s>cons ;

: connection>pname ( cnx -- pname )
  48 rshift u16mask and ;

: connection<pname! ( cnx,pname -- cnx )
  u16mask 48 lshift
  -1 xor rot and
  swap 48 lshift or ;

: (find-connection) ( pname -- cnx,T,F )
  false swap
  connections #connections @ cells
  bounds do
    dup i @ connection>pname = if
      swap drop i @ true
    then
  cell +loop drop ;

: find-connection ( pname -- cnx,T|F )
  #connections @ if
    (find-connection)
  else
    drop false
  then ;

: add-connection ( pname -- cnx )
  assert( #connections @ max-connections < )
  0 swap connection<pname!
  connections #connections @ + !
  1 #connections +! ;
    
: connection ( pname -- cnx )
  dup find-connection if
    drop
  else
    dup add-connection
    find-connection
  then ;
