require ffl/tst.fs
require day05.fs

page

t{ ." include?" cr
  s" pygmalion" char y -rot include? ?true
  s" pygmalion" char z -rot include? ?false
  s" pygmalion" char n -rot include? ?true
}t
t{ ." include-pair?" cr
  s" pygmalion" char g char u 2swap include-pair? ?false
  s" pygmalion" char g char a 2swap include-pair? ?false
  s" pygmalion" char l char a 2swap include-pair? ?false
  s" domino"    char o char o 2swap include-pair? ?false
  s" pygmalion" char p char y 2swap include-pair? ?true
  s" pygmalion" char g char m 2swap include-pair? ?true
  s" pygmalion" char o char n 2swap include-pair? ?true
  s" aabmnh" char a char b 2swap include-pair? ?true
}t
t{ ." include-3-vowels?" cr
  s" kpvwblrizaabmnhz" include-3-vowels? ?true
  s" pygmy" include-3-vowels? ?false
  s" dog" include-3-vowels? ?false
  s" pygmalion" include-3-vowels? ?true
}t
t{ ." include-doubled?" cr
  s" domino" include-doubled? ?false
  s" ardvaark" include-doubled? ?true
  s" waterloo" include-doubled? ?true
}t
t{ ." include-forbidden?" cr
  s" domino" include-forbidden? ?false
  s" abhorrent" include-forbidden? ?true
  s" cabdriver" include-forbidden? ?true
  s" abcde" include-forbidden? ?true
  s" macdonald" include-forbidden? ?true
  s" cupqake" include-forbidden? ?true
  s" maxymum" include-forbidden? ?true
  s" aabmnhz" include-forbidden? ?true
  s" kpvwblrizaabmnhz" include-forbidden? ?true
}t
t{ ." nice-1?" cr
  s" ugknbfddgicrmopn" nice-1? ?true
  s" aaa" nice-1? ?true
  s" jchzalrnumimnmhp" nice-1? ?false
  s" haegwjzuvuyypxyu" nice-1? ?false
  s" dvszwmarrgswjxmb" nice-1? ?false
}t
t{ ." solve-it-1" cr
  solve-it-1 255 ?s
}t
t{ ." has-repeated-pair?" cr
  s" hello magellan" has-repeated-pair? ?true
  s" cow" has-repeated-pair? ?false
  s" magical mystery tour" has-repeated-pair? ?false
  s" phenomenology" has-repeated-pair? ?true
}t
t{ ." has-in-between?" cr
  s" abc" has-in-between? ?false
}t
t{ ." nice-2?" cr
  s" qjhvhtzxzqqjkmpb" nice-2? ?true
  s" xxyxx" nice-2? ?true
  s" uurcxstgmygtbstg" nice-2? ?false
  s" ieodomkazucvgmuy" nice-2? ?false
}t
t{ ." solve-it2" cr
  solve-it-2 55 ?s
}t
bye

