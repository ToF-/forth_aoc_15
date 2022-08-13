require ffl/tst.fs
require asn.fs

page
t{ ." initial set" cr
  8 initial
  set 0 cells + @ 0 ?s
  set 3 cells + @ 3 ?s
  set 7 cells + @ 7 ?s
}t

t{ ." fold the set with a word" cr
  8 initial
  ' + is fold-set-xt
  0 fold-set
  28 ?s
}t

t{ ." first non reversed pair" cr
  4 initial
  2 first-non-reversed 2 ?s
}t

t{ ." swap set items" cr
  4 initial
  0 3 swap-pair
  0 set-ref @ 3 ?s
  3 set-ref @ 0 ?s
}t

t{ ." swap pairs" cr
  4 initial
  -1 reverse-suffix
  0 set-ref @ 3 ?s
  1 set-ref @ 2 ?s
  2 set-ref @ 1 ?s
  3 set-ref @ 0 ?s
}t

t{ ." first greater" cr
  4 initial
  2 first-greater 3 ?s
}t

: emit-item
  [char] A + emit ;

' emit-item is fold-set-xt

: .set
  fold-set space ;

' .set is use-permutation

t{ ." permute set" cr
  4 initial
  permute-set cr
}t
  

t{ ." distances" cr
  s" AlphaCentauri" s" Snowdin" find-string -rot find-string distance 66 ?s
  s" Tambi" s" AlphaCentauri" find-string -rot find-string distance 28 ?s
}t

t{ ." route-length" cr
  8 initial
  route-length 
  66 22 + 39 + 63 + 9 + 27 + 90 + ?s
}t

: .route-length 
  .set
  route-length . space ;

' .route-length is use-permutation

t{ ." permute route length" cr
  3 initial
  permute-set cr
}t

t{ ." find min route length" cr
  find-min-max-route-length swap 141 ?s 736 ?s
}t

bye
