-- trace assigments to global variables

msgs = {
    "a is now 1 (was nil)",
    "b is now 2 (was nil)",
    "a is now 10 (was 1)",
    "b is now 20 (was 2)",
    "b is now nil (was 20)",
    "b is now 200 (was nil)",
}

idx_msg = 1

do
 -- a tostring that quotes strings. note the use of the original tostring.
 local _tostring=tostring
 local tostring=function(a)
  if type(a)=="string" then
   return string.format("%q",a)
  else
   return _tostring(a)
  end
 end

 local log=function (name,old,new)
  local t=debug.getinfo(3,"Sl")
  local line=t.currentline
  io.write(t.short_src)
  if line>=0 then io.write(":",line) end
  local message = name .. " is now " .. tostring(new) .. " (was " .. tostring(old) .. ")"
  io.write(": ",message,"\n")
  assert (message == msgs [idx_msg])
  idx_msg = idx_msg + 1
 end

 local g={}
 local set=function (t,name,value)
  log(name,g[name],value)
  g[name]=value
 end
 setmetatable(getfenv(),{__index=g,__newindex=set})
end

-- an example

a=1
b=2
a=10
b=20
b=nil
b=200
print(tostring(a).." "..tostring(b).." "..tostring(c))
