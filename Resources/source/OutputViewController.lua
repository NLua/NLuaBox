--
-- OutputViewController.lua : View Controller to eval the Script and display the output inside a TextView.
--
-- Authors:
--	Vinicius Jarina (vinicius.jarina@xamarin.com)
-- Copyright 2013-2014 Xamarin Inc.
-- 
-- Licensed under MIT License
--

local OutputViewController = {}

import ('System','System.Text')

OutputViewController.mt = { __call = function(self, code,...)
							return self.new (code, ...)
					end
		}

setmetatable (OutputViewController,OutputViewController.mt)

function OutputViewController.new (code,...)

	OutputViewController.m = {}; -- create a table members to store Lua fields
	OutputViewController.m.backView = nil;
	OutputViewController.m.scriptCode = code;
	OutputViewController.m.timer = nil;

	luanet.make_object (OutputViewController, 'MonoTouch.UIKit.UIViewController');

	return OutputViewController;
end


function OutputViewController:ViewDidLoad ()
		
	self.base:ViewDidLoad();

	self.View.BackgroundColor = UIColor.Clear;
	self.View.Frame = UIScreen.MainScreen.Bounds;
	self.View.AutoresizingMask = luanet.enum(UIViewAutoresizing,'FlexibleWidth,FlexibleHeight');

	self.m.backView = UITextView (self.View.Frame);
	self.m.backView.AutocapitalizationType = UITextAutocapitalizationType.None;
	self.m.backView.AutocorrectionType = UITextAutocorrectionType.No;
	self.m.backView.Font = UIFont.FromName ("Menlo", 14.0);
	self.m.backView.Editable = false;
		

	self.m.backView.AutoresizingMask = luanet.enum(UIViewAutoresizing,'FlexibleWidth,FlexibleHeight');
	self.View:AddSubview (self.m.backView);


	local playButton = UIBarButtonItem (UIBarButtonSystemItem.Stop, function() self:OnStop() end);
	self.NavigationItem.RightBarButtonItem = playButton;

	self.View:LayoutIfNeeded ();

	self.m.timer = NSTimer.CreateScheduledTimer (0.1,  function ()
		self:EvalScript ();
	end);

	-- Perform any additional setup after loading the view, typically from a nib.
end

function OutputViewController:EvalScript ()

	local context = NLuaBoxAppDelegate.AppDelegate.Context;
	local printOutputFunc = function (output, ...) self:OutputString(output, ...) end;
	print = printOutputFunc;
	io.write = printOutputFunc;

	local function run()
		local context = NLuaBoxAppDelegate.AppDelegate.Context;

		context:DoString (self.m.scriptCode);
	end

	err,msg = pcall (run)

	if (not err) then
		local error = "Error running... ";
		if (msg ~= nil) then
			error = error .. msg:ToString ();
			if (msg.InnerException ~= nil) then
				error = error .. " " .. msg.InnerException:ToString();
			end
		end
		self:ErrorString (error);
	end
end
			

function OutputViewController:ErrorString (str)
	self.m.backView.TextColor = UIColor.Red;
	self:OutputStringRaw (str);
end

function OutputViewController:OutputString (output, ...)
	self.m.backView.TextColor = UIColor.DarkTextColor;
	self:OutputStringRaw (output, ...);
end

function OutputViewController:OutputStringRaw (output, ...)

	local builder = StringBuilder ();
	builder:Append (self.m.backView.Text);
	builder:Append (output);
	local args = table.pack(...)
	for i=1,args.n do
		local s = tostring (args[i]);
		builder:Append (' ');
		builder:Append (s);
	end
	self.m.backView.Text = builder:ToString ();
end

function OutputViewController:OnStop ()
	self:DismissViewController (true, null);
end

return OutputViewController;
