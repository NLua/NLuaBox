-- NLuaBoxViewController 


local NLuaBoxViewController = {}
setmetatable (NLuaBoxViewController, {__call = function(self, ...)
													return self.new (...)
													end}) 


function NLuaBoxViewController.new (...)
	
	NLuaBoxViewController.m = {}; -- create a table members to store Lua fields
	NLuaBoxViewController.m.numClicks = 0;
	NLuaBoxViewController.m.buttonWidth = 200;
	NLuaBoxViewController.m.buttonHeight = 50;
	NLuaBoxViewController.m.button = nil;
		
	luanet.make_object (NLuaBoxViewController, 'MonoTouch.UIKit.UIViewController');

	return NLuaBoxViewController;
end

function NLuaBoxViewController:ViewDidLoad ()
	self.base:ViewDidLoad();
	Console.WriteLine ('Begin ViewDidLoad: ');

	self.View.Frame = UIScreen.MainScreen.Bounds;
	self.View.BackgroundColor = UIColor.White;

	self.View.AutoresizingMask = luanet.enum(UIViewAutoresizing,'FlexibleWidth,FlexibleHeight');

	self.m.button = UIButton.FromType (UIButtonType.RoundedRect);

	Console.WriteLine ('self.m.button {0}', self.m.button);

	self.m.button.Frame = RectangleF (
			    self.View.Frame.Width / 2 - self.m.buttonWidth / 2,
			    self.View.Frame.Height / 2 - self.m.buttonHeight / 2,
			    self.m.buttonWidth,
			    self.m.buttonHeight);

	Console.WriteLine ('self.m.button.Frame {0}', self.m.button.Frame);

	self.m.button:SetTitle ("Click me", UIControlState.Normal);

	self.m.button.TouchUpInside:Add (function () 
										self.m.numClicks = self.m.numClicks + 1;
										self.m.button:SetTitle (String.Format ("clicked {0} times", self.m.numClicks), UIControlState.Normal);
										local alert = UIAlertView ();
										alert.Message = "Displaying UIAlertView From Lua";
										alert:AddButton ('OK');
										alert:Show ();
									end);

	self.m.button.AutoresizingMask = luanet.enum(UIViewAutoresizing,'FlexibleWidth,FlexibleTopMargin,FlexibleBottomMargin');

	self.View:AddSubview (self.m.button);

	Console.WriteLine ('End ViewDidLoad');
end

return NLuaBoxViewController