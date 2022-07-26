
 0 constant TK-NOOP
 1 constant TK-LIT
 2 constant TK-VAR
 3 constant TK-ASSIGNED-VAR
 4 constant TK-NOT
16 constant TK-ASSIGN
17 constant TK-AND
18 constant TK-OR
19 constant TK-LSHIFT
20 constant TK-RSHIFT

10000 constant max-tokens

create strings max-tokens cells allot
create string-data max-tokens 10 * allot

variable max-strings
variable next-string

create tokens max-tokens cells allot
variable token-max 

variable assignments

: init
  max-strings off
  string-data next-string !
  token-max off
  assignments off ;

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

: not ( n -- n )
  -1 xor operand-mask and ;

: raw-token ( addr,l - t )
  string-index string-offset lshift ;

: make-token ( opd1,opd2,type -- t )
  type-offset lshift -rot
  operand-mask and opd2-offset lshift swap
  operand-mask and or or ;

: tk-type ( t -- type )
  type-offset rshift ;

: tk-string-index ( t - si )
  string-offset rshift operand-mask and ;

: tk-string ( t -- addr,l )
  tk-string-index nth-string ;

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

: mk-assigned-var-token ( value,addr,l -- t )
  string-index string-offset lshift
  swap opd2-offset lshift or
  TK-ASSIGNED-VAR type-offset lshift or ;

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

80 constant max-entry 
create entry max-entry allot
create step  max-entry allot
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
  entry max-entry erase
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


: chain-not-token ( t,i -- t )
  1- or ;

: chain-var-token ( t -- )
  dup tk-string-index or ;

: chain-assign-token ( t,i -- t )
  over tk-type type-offset lshift -rot
  swap tk-string-index string-offset lshift -rot
  dup 2 - 
  swap 1 - opd2-offset lshift
  or or or ;

: chain-binary-token ( t,i -- t )
  over tk-type type-offset lshift -rot
  swap tk-string-index string-offset lshift swap
  dup 2 - 
  swap 1 - opd2-offset lshift 
  or or or ;

: chain-tokens
  token-max @ 0 do
    i nth-token 
         dup tk-type TK-NOT = if i chain-not-token
    else dup tk-type TK-VAR = if chain-var-token
    else dup tk-type TK-ASSIGN = if i chain-assign-token
    else dup tk-type TK-NOT > IF i chain-binary-token
    then then then then
    i nth-token-ref !
  loop ;

: find-var-for-string ( addr,l -- ti )
  2dup ." looking for var: " type cr
  operand-mask -rot
  token-max @ 0 do
    i nth-token tk-string 2over compare 0= if
      rot drop i -rot leave
    then
  loop 2drop
  dup operand-mask = if
    s" unknown string" exception throw
  then ;

: find-assign-for-token-index ( ti -- ti )
  dup ." looking for assignment for token index: " . cr
  operand-mask swap
  token-max @ 0 do
    dup
    i nth-token dup tk-type TK-ASSIGN = 
               swap tk-operand2 rot = and if
        swap drop i swap leave
      then
    loop drop
  dup operand-mask = if
    s" unassigned variable" exception throw
  then ;

      
: find-assign-for-string ( addr,l -- ti )
  find-var-for-string 
  find-assign-for-token-index ;

: find-assign ( si -- ti )
  false swap token-max @ 0 do
    i nth-token dup tk-type TK-ASSIGN = >r
    tk-operand2 over = r> and if
      nip i true rot leave
    then
  loop drop ;
       
: .tk-type ( tt -- )
  dup TK-LIT = if drop ." LIT"
  else dup TK-VAR = if drop ." VAR"
  else dup TK-ASSIGNED-VAR = if drop ." ASSIGNED VAR"
  else dup TK-ASSIGN = if drop ." ->"
  else dup TK-NOT = if drop ." NOT"
  else dup TK-AND = if drop ." AND"
  else dup TK-OR  = if drop ." OR"
  else dup TK-LSHIFT = if drop ." LSHIFT"
  else dup TK-RSHIFT = if drop ." RSHIFT"
  else drop
  then then then then then then then then then ;

: .token ( t -- )
  dup tk-type .tk-type space
  dup tk-type dup TK-VAR = swap TK-ASSIGNED-VAR = or if dup tk-string type then
  space 
  dup tk-operand1 .
  tk-operand2 . ;

: .tokens
  token-max @ 0 do
    i . i nth-token .token cr
  loop ;

: is-binary-op ( tt - f )
  16 > ;

: binary-op-eval ( n,m,tt - n )
       dup TK-AND = if drop and 
  else dup TK-OR  = if drop or
  else dup TK-LSHIFT = if drop lshift
  else dup TK-RSHIFT = if drop rshift
  else drop s" unknown binary op" exception throw
  then then then then ;

: eval ( t -- n )
  dup ." eval " .token cr
  dup tk-type dup TK-LIT = if 
    drop tk-operand1
  else dup TK-ASSIGNED-VAR = if
    tk-operand2
  else dup TK-VAR = if
    drop 
    dup ." unassigned var: " .token cr
    tk-string find-assign-for-string
    dup ." found assignment: " .token cr
    nth-token recurse
  else dup TK-ASSIGN = if
    drop
    ." assigning token index: " dup tk-operand1 . ." to token index: " dup tk-operand2 . cr
    dup tk-operand1 nth-token recurse
    swap tk-operand2 >r
    r@ ." writing at token index: " . cr
    dup r@ nth-token tk-string mk-assigned-var-token
    r> nth-token-ref !

  else dup TK-NOT = if
    drop tk-operand1 nth-token recurse not
  else dup is-binary-op if
    drop
    dup tk-type
    swap dup tk-operand1 nth-token recurse 
    swap tk-operand2 nth-token recurse
    rot binary-op-eval
  else drop s" unknown tk-type" exception throw
  then then then then then then 
  ." returning " dup . cr ;

: eval-output ( addr,l -- n )
  find-assign-for-string nth-token eval 
  ." done" cr cr ;


80 constant maxline
create puzzle-line maxline allot

variable file-name variable file-name-size

0 value fd-in
next-arg file-name-size ! file-name !

: get-instructions
  0 file-name @ file-name-size @ r/o open-file throw to fd-in
  begin
    puzzle-line maxline fd-in read-line throw while
    puzzle-line swap 
    2dup type cr
    instruction drop
  repeat drop drop
  fd-in close-file throw ;

: solve-it-1 ( -- n )
  init
  get-instructions 
  swap-operators
  chain-tokens
  s" a" eval-output ;
