require ffl/tst.fs
require day06.fs

page
t{ ." turn-on" cr
  0 0 turn-on 
  0 0 light 1 ?s
}t

t{ ." turn-off" cr
  0 0 turn-on
  1 0 turn-on
  0 0 turn-off
  0 0 light 0 ?s
}t

t{ ." turn-on-through" cr
  init-grid
  2 2 8 8 turn-on-through
  2 2 light 1 ?s
  4 4 light 1 ?s
  1 1 light 0 ?s
  9 9 light 0 ?s
}t

t{ ." turn-off-through" cr
  init-grid
  2 2 8 8 turn-on-through
  2 2 light 1 ?s
  4 4 light 1 ?s
  1 1 light 0 ?s
  9 9 light 0 ?s
  3 3 4 4 turn-off-through
  4 4 light 0 ?s
  3 3 light 0 ?s
}t

t{ ." toggle-through" cr
  init-grid
  2 2 8 8 toggle-through
  2 2 light 1 ?s
  4 4 light 1 ?s
  1 1 light 0 ?s
  9 9 light 0 ?s
  3 3 4 4 toggle-through
  4 4 light 0 ?s
  3 3 light 0 ?s
}t
t{ ." count-on" cr
  init-grid
  count-on 0 ?s
  0 0 2 2 turn-on-through 
  count-on 3 3 * ?s
  10 10 12 12 turn-on-through \ 3 * 3
  count-on 3 3 * 3 3 * + ?s
  2 2 5 5 turn-on-through \ 4 * 4
  count-on 3 3 * 3 3 * + 4 4 * + 1 - ?s
  0 0 2 2 turn-off-through
  count-on 3 3 * 4 4 * + 1 - ?s
}t
t{ ." solve it" cr
  init-grid
  require puzzle06.fs
  count-on 0 ?s
}t

bye
