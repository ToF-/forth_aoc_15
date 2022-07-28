
there are INSTRUCTIONS to connect SIGNALS to OUTPUT, through GATES.
OUTPUTS are identified by SYMBOLS

we have an array of connections
each connection has characteristics

byte 
0    : name length (1 or 2)
1..2 : name
3    : connection type
4..5 : data1 signal/wire 
6..7 : Data2 signal/wire 

connection type:
bit 
7    : unused
6..4 : gate operation 000=NOOP 001=NOT 010=AND 011=OR 100=LSHIFT 110=RSHIFT
3    : data 1 type : 0=signal 1=wire
2    : data 2 type : 0=signal 1=wire
1..0 : #elements: (0..3)

how to execute a connection:
read connection type
  read #elements
  if #elements = 0 exception
  read data 1 type : if wire : data 1 = connection name, execute that connection else data 1 = value
  if #elements = 3
  read data 2 type : if wire : data 2 = connection name, execute that connection else data 2 = value
then
  read gate : execute corresponding forth word




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


if a symbol FOO is not known ( find-name )
    evaluate " defer _FOO "
if a symbol FOO is output :
    evaluate " : ->_FOO <signal> ; ' ->_FOO is _FOO "

if a signal is a symbol BAR :
    use " _BAR " as <signal>

if a signal is a value 42 :
    use " 42 " as <signal>



PARSE-NAME parses the flow of entry

: symbol ( addr,l -- addr,l )
    s" _" pad place pad +place pad count ;

123 -> x
    2 nth-item item>symbol find-name if
        empty-string
        s" : ->" <<s
        2 nth-item <<s
        s" " <<s
        0 nth-item 2dup number? 0= if
            s" _" <<s
        then 
        append
        s"  ; '->_" 2 nth-item <<s
        s" _" <<s 2 nth-item <<s

            a
        s" : ->_x 123 ; ' ->_x is _x" evaluate
    else


find-name x if



examine the number of items we have
3 items : a simple connection 
 42 -> a should translate into     42 constant symbol-a
 a -> b should translate into      
 a -> b means that the value of symbol b is symbol a

4 items : a NOT gate connection
NOT 17 -> b means that the value of symbol a is 17 NOT
NOT a -> b means that the value of symbol b is symbol a, NOT-ed

5 items : a binary gate connection
a AND b -> c means that the value of symbol c is symbol a, symbol b


first item :
 if it's a number 
-> : parse the next name, add it to the symbols, 
