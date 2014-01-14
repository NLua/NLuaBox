-- make global variables readonly

function string.ends(String,End)
   return End=='' or string.sub(String,-string.len(End))==End
end

local f=function (t,i) error("cannot redefine global variable `"..i.."'",2) end
local g={}
local G=getfenv()
setmetatable(g,{__index=G,__newindex=f})
setfenv(1,g)

-- an example
rawset(g,"x",3)
x=2

local function test_y()
	y=1	-- cannot redefine `y'
end

local code, msg = pcall (test_y)
print (tostring(code).." ".. msg)
assert (code == false)
assert (string.ends(msg, "readonly.lua:18: cannot redefine global variable `y'"))
return 0
