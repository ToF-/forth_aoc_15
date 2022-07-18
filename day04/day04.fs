require ffl/md5.fs

md5-create md5hash


: has-5z-prefix? ( addr,l -- f )
  drop 5 over + swap 
  0 -rot
  do 
    i c@ [char] 0 = if
      1+
    then
  loop 5 = ;

: to-string ( n -- addr c )  s>d <# #s #> ;


: valid-answer? ( addr,l,n,md5 -- f )
  .s key drop
  >r r> drop
;
\    >r
\    r@ md5-reset
\    -rot r@ md5-update
\    to-string r@ md5-update
\    r@ md5-finish md5+to-string
\    r> drop
\    has-5z-prefix? ;

s" pqrstuv" 1058970 md5hash dbg valid-answer? .
s" abcdef"  609043  valid-answer? .
s" abcdef"  4807    valid-answer? .
bye
