\ Corporate Policy 
char z constant z
char a constant a

: (increment) ( addr,addr' -- )
  2dup <= if
    dup c@ dup z < if
      1+ swap c! drop
    else
      drop a over c!
      1- recurse
    then
  else
    2drop
  then ;

: increment ( addr,l -- )
  1- over + (increment) ;

: straight? ( a,b,c -- f )
  over - 1 = -rot 
  swap - 1 = and ;

: (has-straight) ( addr,addr' -- f )
  2dup swap - 2 >= if
    dup 
    dup 1- dup 1-
    c@ swap c@ rot c@ straight? if
      2drop true
    else
      1- recurse
    then
  else
    2drop false
  then ;

: has-straight ( addr,l -- f )
  1- over + (has-straight) ;

: iol? ( c -- f )
  dup [char] i =
  swap dup [char] o =
  swap [char] l = or or ;


: has-letters-iol ( addr,l -- f)
  false -rot
  bounds do
    i c@ iol? if drop true leave then
  loop ;

: has-one-pair ( addr,l -- c,T|F )
  false -rot
  1- bounds do
    i dup 1+ c@ swap c@ = if
      drop i c@ true leave
    then
  loop ;

: has-one-different-pair ( c,addr,l -- f )
  1- bounds do
    i dup 1+ c@ swap c@ = 
    over i c@ <> and if
      drop true leave
    then
  loop true = ;

: has-two-pairs ( addr,l -- f )
  2dup has-one-pair if
    -rot has-one-different-pair
  else
    2drop false
  then ;

: meet-requirements ( addr,l -- f )
  2dup has-straight -rot
  2dup has-letters-iol 0= -rot
  has-two-pairs and and ;

: next-password ( addr,l -- )
  begin
    2dup increment
    2dup meet-requirements 0= while
  repeat 2drop ;
