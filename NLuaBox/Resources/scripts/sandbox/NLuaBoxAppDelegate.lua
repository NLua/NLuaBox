
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
			
			self.AppDelegate.ViewController = NLuaBoxViewController ()
			
			self.AppDelegate.Window.RootViewController = self.AppDelegate.ViewController;
			Console.WriteLine ('aqui 4');
			self.AppDelegate.Window:MakeKeyAndVisible();
			Console.WriteLine ('aqui 5');
			self.AppDelegate.Context:DoString ("Console.WriteLine('Inseception')");
			return true;
end

return NLuaBoxAppDelegate





