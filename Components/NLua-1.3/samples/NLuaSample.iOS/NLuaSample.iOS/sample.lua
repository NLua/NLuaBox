local s = Scriptable ("My String Parameter")
s:DoSomething ()

print (s.Param1)

local ret = s:SumOfLengths ("Name", 10);

print (tostring(ret))

Scriptable.Print("Hello NLua")

s.Param3 = 0.5;

local p2 = tostring(s.Param3)

print (p2)



















