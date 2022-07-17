variable paper
variable ribbon

: surfaces ( l,w,h -- s )
  over over * >r     \ l,w,h  [w*h]
  rot                \ w,h,l
  over over * >r     \ w,h,l  [w*h,h*l]
  swap -rot          \ h,w,l
  over over * >r     \ h,w,l  [w*h,h*l,w*l]
  drop drop drop
  r> r> r> ;

: perimeters ( l,w,h -- s )
  over over + 2* >r   \ l,w,h [2(w+h)]
  rot                 \ w,h,l
  over over + 2* >r   \ w,h,l [2(w+h),2(h+l)]
  swap -rot           \ h,w,l
  over over + 2* >r   \ h,w,r [2(w+h),2(h+l),2(w+r)]
  drop drop drop
  r> r> r> ;

: volume ( l,w,h -- v )
  * * ;

: smaller ( a,b,c -- x )
  min min ;

: 3dup ( a,b,c -- a,b,c,a,b,c )
  >r over over    \ a,b,a,b [c]
  r@ -rot r> ;    \ a,b,c,a,b,c

: wrapping ( l,w,h -- n )
  surfaces 
  3dup smaller >r
  2* swap 2* + swap 2* +
  r> + ;

: attaching ( l,w,h -- n )
  3dup perimeters smaller >r
  volume r> + ;
  
: compute-order
  3dup wrapping paper +!
       attaching ribbon +! ;
0 paper !  
0 ribbon !

: co compute-order ;  
require puzzle02.fs

page
." paper: " paper ? cr
." rippon: " ribbon ? cr
bye


