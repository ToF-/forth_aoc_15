
1000 constant max-tokens

create strings max-tokens cells allot
create string-data max-tokens 10 * allot

variable max-strings
variable next-string

: init
  max-strings off
  string-data next-string ! ;

: nth-string-ref ( si -- addr )
  cells strings + ;

: nth-string ( si -- addr,l )
  nth-string-ref @ count ;  
  
: find-string-index ( addr,l -- n,T|F )
  false -rot
  max-strings @ 0 ?do
    i nth-string 2over compare 0= if
      rot drop i true 2swap
    then
  loop 2drop ;

: last-string-index ( -- si )
  max-strings @ 1- ;
  
: add-string-address ( addr -- )
  next-string @ max-strings @ nth-string-ref ! 
  1 max-strings +! ;
  
: advance-next-string
  next-string @ count + next-string ! ;

: copy-string ( addr,l,dest -- )
  2dup c! 1+ swap cmove ;

: add-string ( addr,l -- si )
  next-string @ copy-string
  add-string-address
  advance-next-string
  last-string-index ;
  
: string-index ( addr,l -- si )
  2dup find-string-index 0= if
    add-string
  else
    -rot 2drop
  then ;

1 16 lshift 1- constant operand-mask
32 constant type-offset
16 constant opd2-offset

: make-token ( opd1,opd2,type -- t )
  type-offset lshift -rot
  operand-mask and opd2-offset lshift swap
  operand-mask and or or ;

: tk-type ( t -- type )
  type-offset rshift ;

: tk-operand1 ( t -- opd )
  operand-mask and ;

: tk-operand2 ( t -- opd )
  opd2-offset rshift
  operand-mask and ;




