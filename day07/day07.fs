
0 constant NULL-OP
1 constant LITT-OP
2 constant VAR-OP
3 constant NOT-OP
4 constant AND-OP
5 constant OR-OP
6 constant LS-OP
7 constant RS-OP
8 constant ARROW-OP

65535 constant 16bits-mask

1000 constant max-tokens
variable #tokens

create tokens max-tokens cells allot
0 #tokens !

create terms max-tokens cells allot
variable #terms
0 #terms !

create term-strings max-tokens cells 10 * allot
variable current-string
term-strings current-string !

: init
  #tokens off
  #terms off
  term-strings current-string ! ;

: last-token# ( -- n )
  #tokens @ 1- ;

: add-token ( t -- )
  tokens #tokens @ cells + !
  1 #tokens +! ;

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
  VAR-OP <<op-kind or add-token ;

: make-constant ( t -- )
  LITT-OP <<op-kind or add-token ;

: eval-litt ( v -- n )
  16bits-mask and ;

: eval-var  ( v -- n )
  16bits-mask and cells tokens + @ ;
  
: not ( n -- n )
  -1 xor 16bits-mask and ;

: eval-token ( v -- )
  dup >>op-kind
  dup LITT-OP = if
    drop 16bits-mask and 
  else 
    dup VAR-OP = if
      drop operand1 token recurse
    else
      dup NOT-OP = if
        drop operand1 token recurse not
      else 
        swap
        dup operand1 token recurse
        swap operand2 token recurse
        rot dup AND-OP = if
          drop and
        else 
          dup OR-OP = if
            drop or
          else
            dup LS-OP = if
              drop lshift
            else
              dup RS-OP = if
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

: advance-current-string
  current-string @ count +
  current-string ! ;

: new-term-string
  current-string @ 
  terms #terms @ cells + !
  1 #terms +! ;

: add-term-string ( addr,l -- )
  dup -rot 
  current-string @ 1+ swap
  cmove 
  current-string @ c!
  new-term-string
  advance-current-string ;

: find-term ( addr,l -- n,T|F )
  2>r
  0 term-strings
  begin
    dup current-string @ < 
    over count 2r@ compare and while
      count +
      swap 1+ swap
  repeat
  2r> 2drop
  current-string @ < if
    true
  else
    drop false
  then ;
  
: find-operator ( addr,l -- op )
  2dup s" NOT" compare 0= if
    NOT-OP
  else
    2dup s" AND" compare 0= if
      AND-OP
    else
      2dup s" OR" compare 0= if
        OR-OP
      else
        2dup s" LSHIFT" compare 0= if
          LS-OP
        else
          2dup s" RSHIFT" compare 0= if
            RS-OP
          else
            2dup s" ->" compare 0= if
              ARROW-OP
            else
              NULL-OP
            then
          then
        then
      then
    then
  then -rot 2drop ;

: assign-variable ( t,addr,l -- )
  2dup find-term if s" already assigned" exception throw then
  add-term-string
  make-variable ;

: find-value ( addr,l -- t )
  2dup find-term 0= if
    2dup s>number? if
      d>s make-constant
      add-term-string
      last-token#
    then
  else
    -rot 2drop
  then ;
