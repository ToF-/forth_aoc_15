 variable debug 
 debug off
 0 constant TK-NOOP
 1 constant TK-LIT
 2 constant TK-VAR
 3 constant TK-NOT
 4 constant TK-AND
 5 constant TK-OR
 6 constant TK-LSHIFT
 7 constant TK-RSHIFT
 8 constant TK-ASSIGN

1000 constant max-strings
create strings max-strings 10 * allot
variable next-string

1000 constant max-token
2 cells constant token-size
create tokens max-token token-size * allot
variable #tokens

65535 constant operand-mask
: init
  #tokens off 
  strings next-string ! ;

: add-string ( addr,l -- )
  next-string @                    \ addr,l
  over over c!                     \ addr,l,ns@
  dup 2swap rot                    \ ns@,addr,l,ns@
  1+ swap cmove                    \ ns@
  dup c@ + 1+ next-string ! ;

: token@ ( n -- addr )
  token-size * tokens + ;

: last-token ( n )
  #tokens @ 1- ;

: tk->type ( addr -- type )
  @ 48 rshift ;

: tk->string ( addr -- addr,l )
  cell+ @ count ;

: tk->string ( addr -- addr,l )
  cell+ @ count ;

: tk->operand1 ( addr -- n )
  @ operand-mask and ;

: tk->operand2 ( addr -- n )
  @ 16 rshift operand-mask and ;

: find-token ( addr,l -- n,T|0 )
  false -rot
  #tokens @ 0 ?do
    2dup i token@ tk->string compare 0= if
      rot drop i true 2swap leave
    then
  loop 2drop ;
    
: add-token-string ( addr,l -- )
  next-string @ -rot
  add-string 
  #tokens @ token@ cell+ ! ;
  
: add-token ( t -- )
  #tokens @ token@ !
  1 #tokens +! ;

: add-lit-token ( addr,l -- t )
  add-token-string
  TK-LIT 48 lshift
  add-token last-token ;

: add-var-token ( addr,l -- t )
  2dup find-token 0= if
    add-token-string
    TK-VAR 48 lshift
    add-token 
    last-token
  else
    -rot 2drop
  then ;

: add-unary-token ( t,op -- t )
  s" " add-token-string
  48 lshift or add-token last-token ;

: add-binary-token ( t,u,op -- t )
  s" " add-token-string
  -rot 
  16 lshift or
  swap 48 lshift or 
  add-token last-token ;

: add-assignment-token ( t,u -- t )
  s" " add-token-string
  16 lshift or
  TK-ASSIGN 48 lshift or
  add-token last-token ;

: find-assignment  ( addr,l -- n,T|F )
  find-token if
    false swap
    #tokens @ 0 do
      dup i token@ 
      dup tk->type TK-ASSIGN = 
      swap tk->operand2 rot = 
      and if
        nip
        i true rot leave
      then
    loop drop
  else
    false
  then ;


: .token-type ( t -- )
  dup TK-NOOP = if drop ." NOOP" else
    dup TK-LIT = if drop ." LIT" else
      dup TK-VAR = if drop ." VAR" else
        dup TK-NOT = if drop ." NOT" else
          dup TK-AND = if drop ." AND" else
            dup TK-OR = if drop ." OR" else
              dup TK-LSHIFT = if drop ." LSHIFT" else
                dup TK-RSHIFT = if drop ." RSHIFT" else
                  dup TK-ASSIGN = if drop ." ->" else
                    drop then then then then then then then then then ;
: .token ( addr -- )
  dup tk->type .token-type space dup tk->string type space
  dup tk->operand1 . tk->operand2 . ;
  
: .tokens
  debug @ if
    #tokens @ 0 do
      i dup . token@ .token cr
    loop
  then ;

create instruction-buffer 80 allot
variable step@
create step-buffer 80 allot
create steps-strings 5 cells 2* allot
variable #steps
create steps-token 5 cells allot
2variable temp-step

: get-instruction ( addr,l -- )
  dup -rot
  instruction-buffer 80 erase
  instruction-buffer swap cmove
  instruction-buffer + 0 swap c!
  instruction-buffer step@ ! ;

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

: next-step ( -- addr,l )
  step@ @ next-non-space
  dup non-space-length 
  2dup + step@ ! ;

: token-type ( addr,l -- t )
  2dup s>number? -rot 2drop if TK-LIT 
  else 2dup s" NOT" compare 0= if TK-NOT
    else 2dup s" AND" compare 0= if TK-AND
      else 2dup s" OR" compare 0= if TK-OR
        else 2dup s" LSHIFT" compare 0= if TK-LSHIFT
          else 2dup s" RSHIFT" compare 0= if TK-RSHIFT
            else 2dup s" ->" compare 0= if TK-ASSIGN
            else TK-VAR
    then then then then then then then -rot 2drop ;

: step-string# ( n -- addr )
  2* cells steps-strings + ;

: step-string ( n -- addr,l )
  step-string# 2@ ;

: record-steps
  #steps off
  begin
    next-step dup while
    steps-strings #steps @ 2* cells + 2!
    1 #steps +!
  repeat 2drop ;

: reverse-steps ( n,m -- )
  2>r 2r@ step-string rot step-string
  2r> swap >r step-string# 2! r> step-string# 2! ;

: reorder-assignment
  #steps @ 1- dup 1- reverse-steps ;

: reorder-steps
  reorder-assignment 
  0 step-string token-type TK-NOT = if
    1 0 reverse-steps 
  else
    1 step-string token-type 
    dup TK-AND =
    swap dup TK-OR = 
    swap dup TK-LSHIFT =
    swap TK-RSHIFT =
    or or or if
      2 1 reverse-steps
    then
  then ;

create step-tokens 5 cells allot
: step-token ( n -- addr )
  cells step-tokens + ;

: is-binary-token ( t -- f )
  dup TK-AND = 
  swap dup TK-OR =
  swap dup TK-LSHIFT =
  swap TK-RSHIFT =
  or or or ;

: interpret-steps
  0 step-string 2dup token-type TK-LIT = if
    add-lit-token 0 step-token ! 
  else
    add-var-token 0 step-token !
  then
  1 step-string token-type TK-NOT = if
    0 TK-NOT add-unary-token 1 step-token !
  else
    1 step-string 2dup token-type TK-LIT = if
      add-lit-token 1 step-token !
    else
      add-var-token 1 step-token !
    then
  then
  debug @ if ." assignment, steps:" #steps ? cr then
  #steps @ 5 = if
      2 step-string token-type dup is-binary-token if
        0 step-token @ 
        1 step-token @ 
        rot add-binary-token 2 step-token !
        3 step-string add-var-token 3 step-token !
      else
        drop
        2 step-string add-var-token 2 step-token !
      then
    else #steps @ 4 = if
      2 step-string add-var-token 2 step-token !
      3 step-string add-var-token 3 step-token ! 
    then
  then
  debug @ if ." steps: " #steps ? cr then
  debug @ if ." assignement:" #steps @ 3 - . #steps @ 2 - . cr then
  #steps @ 3 - step-token @ 
  #steps @ 2 - step-token @ 
  add-assignment-token 
  #steps @ 1- step-token ! 
  debug @ if
    #steps @ 0 do
      i step-token @ token@ .token cr
    loop cr
  then ;

: .step-strings 
  #steps @ 0 do
    i step-string type space
  loop cr ;

: get-instructions ( addr,l -- )
  get-instruction
  record-steps
  reorder-steps
  debug @ if .step-strings then
  interpret-steps ;
  

: execute-binary ( n,m,tt -- v )
  dup TK-AND = if drop and
    else dup TK-OR = if drop or
      else dup TK-LSHIFT = if drop lshift
        else TK-RSHIFT = if rshift
          else s" unknow tk-type" exception throw
            then then then then ;

: eval ( addr -- n )
  dup tk->type dup TK-LIT = if drop tk->string s>number? drop d>s
    else dup TK-VAR = if drop tk->string find-assignment if
      token@ recurse
      else s" unassigned variable" exception throw then
    else dup TK-ASSIGN = if drop  tk->operand1 token@ recurse
      else dup is-binary-token if 
          swap 
          dup  tk->operand1 token@ recurse
          swap tk->operand2 token@ recurse 
          rot execute-binary
        else 2drop 0
      then then then then ;
