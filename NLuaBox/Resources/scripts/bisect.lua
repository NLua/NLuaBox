-- find root of f using bisect method
-- Part of Lua tests (lua.org)
-- Copyright � 1994�2014 Lua.org, PUC-Rio.

delta = 1e-6	-- tolerance

function bisect(f, a, b, fa, fb)

	local c = (a + b)/2
	print(n .. " c = " .. c .. " a = " .. a .. " b = " .. b .. "\n")
	
	if c == a or c == b or math.abs(a - b) < delta then
		return c, b - a 
	end

	n = n + 1
	
	local fc = f(c)
	
	if fa * fc < 0 then 
		return bisect(f, a, c, fa, fc)
	end
	
	return bisect(f, c, b, fc, fb) 
end

--  in the inverval [a, b]. needs f(a) * f(b) < 0
function solve(f, a, b)
	n = 0
	local z, e = bisect(f, a, b, f(a), f(b))

	print(string.format("after %d steps, root is %.17g with error %.1e, f = %.1e\n", n, z, e, f(z)))

	return z
end

-- our function
function f(x)
	return x*x*x - x -1
end

-- find zero in [1,2]

local z = solve (f, 1, 2)

assert (z - 1.32471799850 < 0.00001)