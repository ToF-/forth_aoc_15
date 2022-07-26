
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

