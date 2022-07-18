require ffl/md5.fs

md5-create md5hash


: has-prefix? ( addr,n -- f )
  dup >r
  over + swap 
  0 -rot
  do 
    i c@ [char] 0 = if
      1+
    then
  loop r> = ;

: to-string ( n -- addr c )  s>d <# #s #> ;


: md5hashstring ( addr,l,n -- f )
  -rot md5hash md5-reset 
  md5hash md5-update
  to-string md5hash md5-update
  md5hash md5-finish md5+to-string ;
\    >r
\    r@ md5-reset
\    -rot r@ md5-update
\    to-string r@ md5-update
\    r@ md5-finish md5+to-string
\    r> drop
\    has-5z-prefix? ;


variable prefix-size

: solve-it
  10000000 0 do
    i 10000 mod 0 = if i . then
    2dup i md5hashstring drop prefix-size @ has-prefix? if i leave then
  loop -rot 2drop ;

5 prefix-size !
s" abcdef" solve-it cr .
cr
s" pqrstuv" solve-it cr .
cr
s" bgvyzdsv" solve-it cr .
6 prefix-size !
s" bgvyzdsv" solve-it cr .
bye
