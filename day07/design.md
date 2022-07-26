
there are INSTRUCTIONS to connect SIGNALS to OUTPUT, through GATES.
OUTPUTS are identified by SYMBOLS



123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f
y RSHIFT 2 -> g
NOT x -> h
NOT y -> i

in forth:
123 signal s" x" symbol connect
456 signal s" y" symbol connect
s" x" symbol s" y" symbol ' and gate connect




we first need to collect symbols and their value.

predefined symbols are :

NOT
AND
OR
LSHIFT
RSHIFT
->


parsing the instructions : 

create instruction 80 allot
: (( 5 0 do parse-name loop ;

the instruction are read in a zone stored in line-buffer with length stored in line-buffer-length
instruction 80 erase
s" ((" instruction swap cmove
line-buffer @ line-buffer-length @ instruction 3 + swap cmove
line-buffer-length @ 3 + instruction + 13 swap c!
instruction line-buffer-length @ 4 + evaluate

and then we have 5 strings on the stack


PARSE-NAME parses the flow of entry

