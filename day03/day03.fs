8192 constant R

create pos-index R cells allot
variable maxpos

R negate constant north
R constant south
-1 constant west
1 constant east

: track-move ( pos,c -- pos' )
  dup [char] ^ = if north else
  dup [char] v = if south else
  dup [char] < = if west else
  dup [char] > = if east then
  then then then
  nip + ;

: lookup-pos ( pos -- n | -1 )
  -1 swap 
  maxpos @ 0 ?do
    pos-index i cells + @ 
    over = if nip i swap leave then
  loop drop ;

: store-pos ( pos -- )
  pos-index maxpos @ cells + !
  1 maxpos +! ;

: track-course ( addr,l -- )
  0 maxpos !  0 store-pos
  0 -rot
  over + swap do
    i c@ track-move
    dup lookup-pos -1 = if
      dup store-pos
    then
  loop drop ;

s" >"    track-course maxpos ? cr
s" ^>v<" track-course maxpos ? cr
s" ^v^v^v^v^v" track-course maxpos ? cr

10000 constant maxline
create puzzle-line maxline allot

next-arg r/o open-file throw Value fd-in
puzzle-line maxline fd-in read-line throw drop
fd-in close-file throw
puzzle-line swap track-course maxpos ? cr
bye


    






