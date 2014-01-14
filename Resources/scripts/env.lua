-- read environment variables as if they were global variables

function mock_getenv (var)
	if (var == "PATH") then
		return "/Fake/Path/Test"
	end

	if (var == "USER") then
		return "fake_user"
	end

	return nil
end

local f=function (t,i) return mock_getenv(i) end
setmetatable(getfenv(),{__index=f})

-- an example
print(tostring(a) .." ".. USER .." ".. PATH)

assert (PATH == "/Fake/Path/Test")
assert (USER == "fake_user")
assert (a == nil)