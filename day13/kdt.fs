( Knights of the Dinner Table )

10 constant max-guests
variable #guests

create guests max-guests dup * cells allot

: happiness! ( i,j,n -- )
  rot max-guests * rot + cells guests + ! ;

: happiness ( i,j -- n )
  swap max-guests * + cells guests + @ ;
