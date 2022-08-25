require ffl/tst.fs
require jsaf.fs

page

t{ ." extract-number" cr
  s" " extract-number 0 ?s
  s" 42" extract-number 42 ?s
  s" [[23]]" extract-number 23 ?s
  s" -49" extract-number -49 ?s
}t

t{ ." sum sample" cr
  s" sample.txt" sum 9 ?s
}t

t{ ." sum puzzle" cr
  s" puzzle12.txt" sum 156366 ?s
}t
t{ ." opening an object" cr
  s" {" parse-json swap JS-OBJECT ?s 0 ?s
}t
t{ ." closing an object" cr
  s" { }" parse-json 0 ?s
}t
t{ ." opening a list" cr
  s" [" parse-json swap JS-LIST ?s 0 ?s
}t
t{ ." closing a list" cr
  s" [ ]" parse-json 0 ?s
}t
t{ ." in-a-list?" cr
  s" {[ " parse-json in-a-list? ?true
  s" ]" parse-json in-a-list? ?false
  clearstack
}t
t{ ." numeric value" cr
  s" [ 4807 " parse-json 4807 ?s JS-LIST ?s
  s" [ -2317 " parse-json -2317 ?s JS-LIST ?s
}t
t{ ." string" cr
  s\" [\"foo" parse-json in-a-string? ?true
  clearstack
  s\" [\"foo\"" parse-json in-a-string? ?false
  clearstack
}t
t{ ." attribute value" cr
  s\" {\"foo\"" parse-json expect-value? @ ?false
  clearstack
  s\" {\"foo\":" parse-json expect-value? @ ?true
  clearstack
}t
t{ ." red value detected" cr
  s\" [\"red\"" parse-json swap JS-LIST ?s 0 ?s
  clearstack
  s\" {\"red\":\"green\"" parse-json swap JS-OBJECT ?s 0 ?s
  clearstack
  s\" {\"foo\":\"red\"" parse-json swap JS-OBJECT RED or ?s 0 ?s
  clearstack
}t
t{ ." adding values" cr
  s" [42,17,23]" parse-json 82 ?s
  s\" {\"foo\":42, \"bar\":17, \"quux\":23}" parse-json 82 ?s
  s\" {\"foo\":[42,17,23] \"bar\":17, \"quux\":23}" parse-json 
  .s cr 122 ?s
}t

bye
