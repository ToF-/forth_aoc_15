require ffl/tst.fs
require kdt.fs

page
t{ ." sample happiness relation" cr
  4 #guests !
  0   1  54 happiness!
  0   2 -59 happiness!
  0   3  -2 happiness!
  1   0  83 happiness!
  1   2  -7 happiness!
  1   3 -63 happiness!
  2   0 -62 happiness!
  2   1  60 happiness!
  2   3  55 happiness!
  3   0  46 happiness!
  3   1  -7 happiness!
  3   2  41 happiness!

  0 0 happiness 0 ?s
  0 1 happiness 54 ?s
  2 3 happiness 55 ?s
}t
t{ ." arrangement global happiness" cr
  2  arrangement 0 cells + !  \ 60+55
  3  arrangement 1 cells + !  \ 41+46
  0  arrangement 2 cells + !  \ -2+54
  1  arrangement 3 cells + !  \ 83-7
  global-happiness 330 ?s
  2  arrangement 0 cells + !  \ 60-62
  0  arrangement 1 cells + !  \ -59-2
  3  arrangement 2 cells + !  \ 46-7
  1  arrangement 3 cells + !  \ -63-7
  global-happiness 23 ?s
}t
bye

