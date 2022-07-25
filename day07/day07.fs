
 0 constant TK-NOOP
 1 constant TK-LIT
 2 constant TK-VAR
 3 constant TK-NOT
16 constant TK-ASSIGN
17 constant TK-AND
18 constant TK-OR
19 constant TK-LSHIFT
20 constant TK-RSHIFT

1000 constant max-tokens

create strings max-tokens cells allot
create string-data max-tokens 10 * allot

variable max-strings
variable next-string

create tokens max-tokens cells allot
variable token-max 

: init
  max-strings off
  string-data next-string !
  token-max off ;

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
48 constant type-offset
32 constant string-offset
16 constant opd2-offset

: raw-token ( addr,l - t )
  string-index string-offset lshift ;

: make-token ( opd1,opd2,type -- t )
  type-offset lshift -rot
  operand-mask and opd2-offset lshift swap
  operand-mask and or or ;

: tk-type ( t -- type )
  type-offset rshift ;

: tk-string ( t -- addr,l )
  string-offset rshift operand-mask and
  nth-string ;
  

: tk-operand1 ( t -- opd )
  operand-mask and ;

: tk-operand2 ( t -- opd )
  opd2-offset rshift
  operand-mask and ;

: mk-lit-token ( n -- t )
  TK-LIT type-offset lshift or ;

: mk-var-token ( addr,l -- t )
  string-index string-offset lshift 
  TK-VAR type-offset lshift or ;

: mk-token ( type -- t )
  type-offset lshift ;

: tk-assigned? ( t -- f )
  tk-operand2 ;

: set-tk-type ( t -- t )
  tk-string 
  2dup s>number? if d>s mk-lit-token
  else 2drop 2dup s" NOT" compare 0= if TK-not mk-token
  else 2dup s" ->" compare 0= if TK-ASSIGN mk-token
  else 2dup s" AND" compare 0= if TK-AND mk-token
  else 2dup s" OR" compare 0= if TK-OR mk-token
  else 2dup s" LSHIFT" compare 0= if TK-LSHIFT mk-token
  else 2dup s" RSHIFT" compare 0= if TK-RSHIFT mk-token
  else 2dup mk-var-token 
  then then then then then then then -rot 2drop ;

: next-non-space ( addr -- addr )
  begin
    dup c@ bl = while
    1+
  repeat ;

: non-space-length ( addr - l )
  dup
  begin
    dup c@ dup bl <> swap and while
    1+
  repeat swap - ;

create entry 80 allot
create step  80 allot
variable entry@

: next-entry 
  entry@ @ next-non-space
  dup non-space-length 
  dup step c!
  2dup step 1+ swap cmove
  + entry@ ! ;

: add-token ( t -- )
  token-max @ cells tokens + !
  1 token-max +! ;

: instruction ( addr,l -- n )
  entry 80 erase
  entry swap cmove
  entry entry@ !
  0 begin
    next-entry step count dup while
    raw-token set-tk-type add-token
    1+
  repeat 2drop ;

: nth-token-ref ( i -- addr )
  cells tokens + ;

: nth-token ( i -- t )
    nth-token-ref @ ;

: swap-operators
  token-max @ 0 do
    i nth-token tk-type TK-NOT >= if
      i nth-token
      i 1+ nth-token
      swap
      i 1+ nth-token-ref !
      i nth-token-ref !
      2
    else
      1
    then
  +loop ;

: chain-operators
  token-max @ 0 do
    i nth-token dup tk-type TK-NOT = if
      i 1- or i nth-token-ref !
    else dup tk-type 3 > IF
      i 2 - or
      i 1 - opd2-offset lshift or
      i nth-token-ref ! 
    then then
  loop ;

: .tk-type ( tt -- )
  dup TK-LIT = if drop ." LIT"
  else dup TK-VAR = if drop ." VAR"
  else dup TK-ASSIGN = if drop ." ->"
  else dup TK-NOT = if drop ." NOT"
  else dup TK-AND = if drop ." AND"
  else dup TK-OR  = if drop ." OR"
  else dup TK-LSHIFT = if drop ." LSHIFT"
  else dup TK-RSHIFT = if drop ." RSHIFT"
  else drop
  then then then then then then then then ;



: .token ( t -- )
  dup tk-type .tk-type space
  dup tk-type TK-VAR = if dup tk-string type then
  space 
  dup tk-operand1 .
  tk-operand2 .
  cr ;

: .tokens
  token-max @ 0 do
    i . i nth-token .token 
  loop cr ;
