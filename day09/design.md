LP LP LP

LR LP LP
LM LP LP

AZ BZ LP

LR LP LP
LM LP LP

AY BZ LP

LR LP LP
LM LP LP

AX BZ LP

LR LP LP
LM LP LP

swap(0, 0,1) swap(0, 0,1) swap(0, 0,1) 

swap(-1, 0,2) swap(0, 0,1) swap(0, 0,1)
swap(-1, 0,1) swap(0, 0,1) swap(0, 0,1)

swap(-2, 0,3) swap(-1, 0,2) swap(0, 0,1)

swap(-1, 0,2) swap(0, 0,1) swap(0, 0,1)
swap(-1, 0,1) swap(0, 0,1) swap(0, 0,1)

swap(-2, 0,2) swap(-1, 0,2) swap(0, 0,1)
swap(-1, 0,2) swap(0, 0,1) swap(0, 0,1)

swap(-1, 0,1) swap(0, 0,1) swap(0, 0,1)

swap(-2, 0,1) swap(-1, 0,2) swap(0, 0,1)

swap(-1, 0,2) swap(0, 0,1) swap(0, 0,1)
swap(-1, 0,1) swap(0, 0,1) swap(0, 0,1)

ab -> ba -> ab -> ba

abc -> acb -> abc -> acb
acb -> bca -> bac -> bca
bca -> cba -> cab -> cba

abcd -> abdc -> abcd -> abdc
abdc -> acdb -> acbd -> acdb
acdb -> adcb -> adbc -> adcb
adcb -> bdca -> bacd -> badc
badc -> bcda -> bcad -> bcda
bcda -> bdca -> bdac -> bdca
bdca -> cdba -> cabd -> cadb
cadb -> cbda -> cbad -> cbda
cbda -> cdba -> cdab -> cdba
cdba -> dcba -> dabc -> dacb
dacb -> dbca -> dbac -> dbca
dbca -> dcba -> dcab -> dcba




permutations
abcd         *
swap 2,3
abcd -> abdc * 

swap 1,3 abdc -> acdb swap 2,3 acdb -> acbd * swap 2,3 acbd -> acdb *

swap 1,2 acdb -> adcb swap 2,3 adcb -> adbc * swap 2,3 adbc -> adcb *

swap 0,3 adcb -> bdca swap 1,3 bdca -> bacd * swap 2,3 bacd -> badc *

swap 1,3 badc -> bcda swap 2,3 bcda -> bcad * swap 2,3 bcad -> bcda *

swap 1,2 bcda -> bdca swap 2,3 bdca -> bdac * swap 2,3 bdca -> bdca *

swap 0,2 bdca -> cdba swap 1,3 cdba -> cabd * swap 2,3 cabd -> cadb *

swap 1,3 cadb -> cbda swap 2,3 cbda -> cbad * swap 2,3 cbad -> cbda *

swap 1,2 cbda -> cdba swap 2,3 cdba -> cdab * swap 2,3 cdab -> cdba *

swap 0,1 cdba -> dcba swap 1,3 dcba -> dabc * swap 2,3 dabc -> dacb *

swap 1,3 dacb -> dbca swap 2,3 dbca -> dbac * swap 2,3 dbac -> dbca *

swap 1,2 dbca -> dcba swap 2,3 dcba -> dcab * swap 2,3 dcab -> dcba *

