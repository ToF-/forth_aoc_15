
0 constant NULL-TOKEN
1 constant LITT-TOKEN
2 constant VAR-TOKEN
3 constant NOT-TOKEN
4 constant AND-TOKEN
5 constant OR-TOKEN
6 constant LS-TOKEN
7 constant RS-TOKEN

65535 constant 16bits-mask

1000 constant max-tokens
variable last-token

create tokens max-tokens cells allot
0 last-token !

create terms max-tokens 10 cells * allot


: add-token ( t -- )
  tokens last-token @ cells + !
  1 last-token +! ;

: >>op-kind ( v -- op )
  32 rshift ;

: <<op-kind ( op -- v )
  32 lshift ;

: operand1 ( v -- opd )
  16bits-mask and ;

: operand2 ( v -- opd )
  16 rshift 16bits-mask and ;

: token ( opd -- v )
  cells tokens + @ ;

: make-binany-op ( t1,t2,op -- )
  <<op-kind
  swap 16 lshift
  or or add-token ;

: make-unary-op ( t,op -- )
  <<op-kind or add-token ; 

: make-variable ( t -- )
  VAR-TOKEN <<op-kind or add-token ;

: make-constant ( t -- )
  LITT-TOKEN <<op-kind or add-token ;

: eval-litt ( v -- n )
  16bits-mask and ;

: eval-var  ( v -- n )
  16bits-mask and cells tokens + @ ;
  
: not ( n -- n )
  -1 xor 16bits-mask and ;

: eval-token ( v -- )
  dup >>op-kind
  dup LITT-TOKEN = if
    drop 16bits-mask and 
  else 
    dup VAR-TOKEN = if
      drop operand1 token recurse
    else
      dup NOT-TOKEN = if
        drop operand1 token recurse not
      else 
        swap
        dup operand1 token recurse
        swap operand2 token recurse
        rot dup AND-TOKEN = if
          drop and
        else 
          dup OR-TOKEN = if
            drop or
          else
            dup LS-TOKEN = if
              drop lshift
            else
              dup RS-TOKEN = if
                drop rshift
              else 
                s" unknown operator" exception throw
              then
            then
          then
        then
      then
    then
  then ;
  
: find-term ( addr,l -- n,T|F )
  false -rot
  last-token @ 0 ?do
    2dup i 10 cells * terms + equals? if
      rot drop i true 2swap leave
    then
  loop 2drop ;
  
