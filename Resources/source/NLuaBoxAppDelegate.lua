

local NLuaBoxAppDelegate = {}

ScriptListViewController = require ('ScriptListViewController');
ScriptViewController = require ('ScriptViewController')

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
			
			self.MainWindow = UIWindow (UIScreen.MainScreen.Bounds);
			local window = self.MainWindow;
			window.TintColor = UIColor.Purple;
			-- load the appropriate UI, depending on whether the app is running on an iPhone or iPad
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) then
				local controller = ScriptListViewController ();
				local navigationController = UINavigationController (controller);
				window.RootViewController = navigationController;
			else 
				local masterViewController =  ScriptListViewController ();
				local masterNavigationController = UINavigationController (masterViewController);
				local detailViewController = ScriptViewController (masterViewController:GetScriptsStore());
				local detailNavigationController = UINavigationController (detailViewController);

				masterViewController.m.ScriptViewController = detailViewController;
				
				splitViewController = UISplitViewController ();
				splitViewController.WeakDelegate = detailViewController;
				splitViewController.ViewControllers = luanet.make_array (UIViewController, {
					masterNavigationController,
					detailNavigationController
				})
				
				window.RootViewController = splitViewController; 
			end

			-- make the window visible
			window:MakeKeyAndVisible ();
			return true;
end



return NLuaBoxAppDelegate
