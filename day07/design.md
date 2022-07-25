For example, here is a simple circuit:

123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f
y RSHIFT 2 -> g
NOT x -> h
NOT y -> i
After it is run, these are the signals on the wires:

d: 72
e: 507
f: 492
g: 114
h: 65412
i: 65079
x: 123
y: 456

----
tokenizing the elements in the instructions

lit(123)	op-assign	var(x)
lit(456)	op-assign	var(y)
var(x)	op-AND	var(y)	op-assign	var(d)
var(x)	op-OR	var(y)	op-assign	var(e)
var(x)	op-LS	lit(2)	op-assign	var(f)
var(y)	op-RS	lit(2)	op-assign	var(g)
op-NOT	var(x)	op-assign	var(h)
op-NOT	var(y)	op-assign	var(i)

reordering the tokens in the instructions makes it almost like a forth program

lit(123)	var(x)  op-assign
lit(456)	var(y)  op-assign
var(x)	var(y)	op-AND	var(d)  op-assign
var(x)	var(y)	op-OR	var(e)  op-assign
var(x)	lit(2)	op-LS	var(f)  op-assign
var(y)	lit(2)	op-RS	var(g)  op-assign
var(x)	op-NOT	var(h)  op-assign
var(y)	op-NOT	var(i)  op-assign

the one and only rule of reordering is : reverse any operator (including op-assign) with its successor 



lit ( s -- ti )
if not found add lit token with operand1 = number conversion of s
return token index

var ( s -- ti )
look for a token with operand1 == string index for a string == s
if found return token index
if not found add string s, returning string index, then create a var token with operand1 = string index
return token index

op-assign ( ti1,ti2 -- ti )
add op-assign token with operands ti1 and ti2
return token index

op-not ( ti -- ti )
add not token with operand ti
return token index

op-and, op-or, op-ls, op-rs ( ti1,ti2 -- ti )
add binary op token with operands ti1 and ti2
return token index

then evaluation is a recursive process:

eval ( ti -- n )
depending on token type of token ti:
lit: operand1 is the value
var : if operand2 <> 0 then operand1 else exception "unassigned var"
op-assign : eval operand1, update token operand2 with operand2 = true, operand1 = value
op-not : eval operand1, not that
op-and,op-or,op-ls,op-rs : eval operand1, eval operand2, operation

eval-output (s -- n )
look for var token with operand1 = string index of a string == s, returning ti
look for an op-assign token with operand2 = ti and eval this token
then eval token ti


----
we need basic words to
- given a token, return its type, operand1 operand2
- extract token strings from the instruction string
- append tokens to a sequence of tokens
- find a token with a given set of characteristics


