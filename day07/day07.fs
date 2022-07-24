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

: add-lit-token ( addr,l -- )
  add-token-string
  TK-LIT 48 lshift
  add-token ;

: add-var-token ( addr,l -- )
  2dup find-token 0= if
    add-token-string
    TK-VAR 48 lshift
    add-token 
  else
    drop 2drop
  then ;

: add-unary-token ( t,op -- )
  s" (un)" add-token-string
  48 lshift or add-token ;

: add-binary-token ( t,u,op -- )
  s" (bin)" add-token-string
  -rot 
  16 lshift or
  swap 48 lshift or 
  add-token ;

: add-assignment-token ( t,u -- )
  s" (->)" add-token-string
  16 lshift or
  TK-ASSIGN 48 lshift or
  add-token ;

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


: eval ( addr -- n )
  tk->string s>number? drop d>s ;

: .token-strings
  #tokens @ 0 do
    i token@ tk->string type cr
  loop ;
