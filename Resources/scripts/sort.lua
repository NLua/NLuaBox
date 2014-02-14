-- temperature conversion table (celsius to farenheit)
-- Part of Lua tests (lua.org)
-- Copyright © 1994–2014 Lua.org, PUC-Rio.
-- Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
--
--The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
--
-- THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
-- FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
-- DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
-- SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

-- two implementations of a sort function
-- this is an example only. Lua has now a built-in function "sort"

quicksort_x = {"Apr","Aug","Dec","Feb","Jan","Jul","Jun","Mar","May","Nov","Oct","Sep"}
reverse_x = {"Sep","Oct","Nov","May","Mar","Jun","Jul","Jan","Feb","Dec","Aug","Apr"}

-- extracted from Programming Pearls, page 110
function qsort(x,l,u,f)
 if l<u then
  local m=math.random(u-(l-1))+l-1	-- choose a random pivot in range l..u
  x[l],x[m]=x[m],x[l]			-- swap pivot to first position
  local t=x[l]				-- pivot value
  m=l
  local i=l+1
  while i<=u do
    -- invariant: x[l+1..m] < t <= x[m+1..i-1]
    if f(x[i],t) then
      m=m+1
      x[m],x[i]=x[i],x[m]		-- swap x[i] and x[m]
    end
    i=i+1
  end
  x[l],x[m]=x[m],x[l]			-- swap pivot to a valid place
  -- x[l+1..m-1] < x[m] <= x[m+1..u]
  qsort(x,l,m-1,f)
  qsort(x,m+1,u,f)
 end
end

function selectionsort(x,n,f)
 local i=1
 while i<=n do
  local m,j=i,i+1
  while j<=n do
   if f(x[j],x[m]) then m=j end
   j=j+1
  end
 x[i],x[m]=x[m],x[i]			-- swap x[i] and x[m]
 i=i+1
 end
end

function show(m,x, t)
 print(m.." ".."\n\t")
 local i=1
 while x[i] do
  assert (x[i] == t[i])
  print(x[i])
  i=i+1
  if x[i] then print(",") end
 end
 print("\n")
end

function testsorts(x)
 local n=1
 while x[n] do n=n+1 end; n=n-1		-- count elements
 show("original",x , x)
 qsort(x,1,n,function (x,y) return x<y end)
 show("after quicksort",x, quicksort_x)
 selectionsort(x,n,function (x,y) return x>y end)
 show("after reverse selection sort",x, reverse_x)
 qsort(x,1,n,function (x,y) return x<y end)
 show("after quicksort again",x, quicksort_x)
end

-- array to be sorted
x={"Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"}

testsorts(x)
