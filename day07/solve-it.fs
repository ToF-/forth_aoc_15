require ffl/tst.fs
require sar.fs

80 constant max-length
create puzzle-line max-length allot

variable file-name variable file-name-size
next-arg file-name-size ! file-name !

0 value fd-in

variable #line
: solve-it-1
  #line off
  0 file-name @ file-name-size @ r/o open-file throw to fd-in
  begin
    #line ? unused . cr 
    puzzle-line max-length erase
    puzzle-line max-length fd-in read-line throw while
    puzzle-line swap sar>forth 
    sar-forth evaluate
    1 #line +!
  repeat drop
  fd-in close-file throw  ;


