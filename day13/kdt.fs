( Knights of the Dinner Table )

10 constant max-guests
variable #guests

create guests max-guests dup * cells allot
create arrangement max-guests cells allot

: happiness! ( i,j,n -- )
  rot max-guests * rot + cells guests + ! ;

: happiness ( i,j -- n )
  swap max-guests * + cells guests + @ ;

: global-happiness ( -- n )
  0
  #guests @ 0 do
    i cells arrangement + @ 
    dup 
    dup  1- #guests @ mod cells arrangement + @ happiness
    -rot
    dup 1+ #guests @ mod cells arrangement + @ happiness
    + +
  loop ;
  

