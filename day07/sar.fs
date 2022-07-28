500 constant max-connections
create connections max-connections cells allot

: conn<name ( cnx,addr,l -- )
  dup 2 > if s" name is too long" exception throw then
  over c@ 8 lshift -rot
  1 > if 1+ c@ else drop 0 then 
  or 48 lshift or ;

: s>empty ( addr -- )
  0 swap c! ;

: s>cons ( c,addr -- )
    dup c@           \ c,addr,l
    1+               \ c,addr,l'
    2dup swap c!     \ c,addr,l'
    + c! ;
  

: conn>name ( cnx,dest -- )
  dup s>empty
  swap 48 rshift 256 /mod      \ dest,c2,c1
  rot swap over s>cons         \ dest,c2
  swap ?dup if 
    swap s>cons 
  else drop then ;
    

