

local NLuaBoxAppDelegate = {}

ui = require ('ui');

NLuaBoxAppDelegate.mt = {__call = function(self, ...)
													return self.new (...)
											end
								  }
setmetatable (NLuaBoxAppDelegate,NLuaBoxAppDelegate.mt)

function NLuaBoxAppDelegate.new (AppDelegate)
	NLuaBoxAppDelegate.m = {}; -- create a table members to store Lua fields
	NLuaBoxAppDelegate.AppDelegate = AppDelegate;
	return NLuaBoxAppDelegate
end

function NLuaBoxAppDelegate:FinishLaunching ()
			
			

			return true;
end



return NLuaBoxAppDelegate
