500 constant max-connections
create connections max-connections cells allot

0 constant conn-empty

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

