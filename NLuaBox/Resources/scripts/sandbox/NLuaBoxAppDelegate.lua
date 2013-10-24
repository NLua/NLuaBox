

local NLuaBoxAppDelegate = {}

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
			
			
			-- create a new window instance based on the screen size
			self.Window = UIWindow (UIScreen.MainScreen.Bounds);
			local script = [[
-- this is a Lua script
function myFunc (v1, v2)
	return v1 + v2
end

--msgbox like func

function msgbox(message)
	local view = UIAlertView ();
	view.Message = message;
	view:AddButton ('OK');
	view:Show ();
end

msgbox ("Hello");

print (myFunc(10,20))
]];

			local viewController = JLTextViewController (NSString(script));

			local navigationContorler = UINavigationController (viewController);
			navigationContorler.NavigationBar.BarStyle = UIBarStyle.Black;

			self.m.codeView = viewController.TextView;

			viewController.TextView.KeyboardAppearance = UIKeyboardAppearance.Dark;
			navigationContorler:SetNavigationBarHidden (true, false);

			self:SetupToolbarOnKeyboard (viewController.TextView);

			self.Window.RootViewController = navigationContorler;
			self.Window:MakeKeyAndVisible ();
			
			return true;
end

function NLuaBoxAppDelegate:SetupToolbarOnKeyboard (txt)

			local toolbar = UIToolbar ();
			toolbar.BarStyle = UIBarStyle.Black;
			toolbar.Translucent = true;
			toolbar:SizeToFit ();
			local doneButton = UIBarButtonItem ("Close", UIBarButtonItemStyle.Done,
				function ()
					self.m.codeView:ResignFirstResponder ();
				end);
				
			doneButton.TintColor = UIColor.Gray;

			local goButton =  UIBarButtonItem ("Run", UIBarButtonItemStyle.Done,
				function ()

					self.m.codeView:ResignFirstResponder ();
					self:OnRun ();
				end);

			local itens = 
			toolbar:SetItems (luanet.make_array(UIBarButtonItem, {doneButton, goButton}), true);

			self.m.codeView.InputAccessoryView = toolbar;
end

function NLuaBoxAppDelegate:OnRun ()

	local context = self.AppDelegate.Context;

	context:DoString (self.m.codeView.Text);

end

return NLuaBoxAppDelegate





