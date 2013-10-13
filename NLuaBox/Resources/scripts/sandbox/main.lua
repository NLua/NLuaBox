Console.WriteLine ('Aqui 1')

NLuaBoxAppDelegate = require ('NLuaBoxAppDelegate')

function Init (AppDelegate)
	local nluaBoxAppDelegate = NLuaBoxAppDelegate(AppDelegate)
	return nluaBoxAppDelegate:FinishLaunching()
end