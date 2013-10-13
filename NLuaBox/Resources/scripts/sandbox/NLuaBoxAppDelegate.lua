
NLuaBoxViewController = require ('NLuaBoxViewController')

local NLuaBoxAppDelegate = {}

NLuaBoxAppDelegate.mt = {__call = function(self, ...)
													return self.new (...)
											end
								  }
setmetatable (NLuaBoxAppDelegate,NLuaBoxAppDelegate.mt)

function NLuaBoxAppDelegate.new (AppDelegate)
	NLuaBoxAppDelegate.AppDelegate = AppDelegate;
	return NLuaBoxAppDelegate
end

function NLuaBoxAppDelegate:FinishLaunching ()
			
			
			self.AppDelegate.Window = UIWindow (UIScreen.MainScreen.Bounds)
			
			self.AppDelegate.ViewController = NLuaBoxViewController.new ()--NLuaBoxViewController.new ();
			
			self.AppDelegate.Window.RootViewController = self.AppDelegate.ViewController;
			Console.WriteLine ('aqui 4');
			self.AppDelegate.Window:MakeKeyAndVisible();
			Console.WriteLine ('aqui 5');
			return true;
end

return NLuaBoxAppDelegate





