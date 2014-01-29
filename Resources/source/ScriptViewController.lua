
local ScriptViewController = {}


ScriptViewController.mt = { __call = function(self, store,...)
													return self.new (store, ...)
											end
							  }

setmetatable (ScriptViewController,ScriptViewController.mt)

function ScriptViewController.new (store,...)

		ScriptViewController.m = {}; -- create a table members to store Lua fields
		ScriptViewController.m.masterPopoverController = nil;
		ScriptViewController.m.ScriptName = "";
		ScriptViewController.m.IsSource = false;
		ScriptViewController.m.store = store;
		ScriptViewController.m.timer = nil;

		luanet.make_object (ScriptViewController, 'Chormatism.JLTextViewController');

		return ScriptViewController;
end

function ScriptViewController:LoadScript (name, source)
		
		if (scriptName ~= name) then
			self.m.ScriptName = name;
			self.m.IsSource = source;
			-- Update the view
			self:ConfigureView ();
		end
			
		if (masterPopoverController ~= nil) then
			masterPopoverController:Dismiss (true);
		end
end

function ScriptViewController:EnablePlayButton ()

			local playButton = UIBarButtonItem (UIBarButtonSystemItem.Play, self.OnRun);
			self.NavigationItem.RightBarButtonItem = playButton;
end

function ScriptViewController: DisablePlayButton ()
		self.NavigationItem.RightBarButtonItem = null;
end

function ScriptViewController: ConfigureView ()
		
	-- Update the user interface for the detail item
	if (not self.IsViewLoaded) then
		return;
	end
	if (self.m.ScriptName ~= nil and self.m.ScriptName ~= "") then
		if (self.m.IsSource) then
			self.TextView.Text = self.m.store:GetSourceContent (self.m.ScriptName);
			self:DisablePlayButton ();

		else 
			self.TextView.Text = self.m.store:GetScriptContent (self.m.ScriptName);
			self:EnablePlayButton ();
		end
					
		self.TextView.Editable = true;
		self.Title = self.m.ScriptName;
	else 
		self.TextView.Text = " -- <no script selected>";
		self.TextView.Editable = false;
		self.Title = "Script";
		self:DisablePlayButton ();
	end
end

function ScriptViewController:ViewDidLoad ()

		self.base:ViewDidLoad ();

		self.View.Frame = UIScreen.MainScreen.Bounds;
		self.View.AutoresizingMask = luanet.enum(UIViewAutoresizing,'FlexibleWidth,FlexibleHeight');
			
		self.TextView.Changed:Add (self.OnChanged);
		self:ConfigureView ();
end

function ScriptViewController:OnRun (sender, args)

	local script = self.TextView.Text;

	local output = OutputViewController (script);
	self.ModalPresentationStyle =  UIModalPresentationStyle.CurrentContext;
	local nav =  UINavigationController (output);
	self:PresentViewController (nav, true, null);
end

function ScriptViewController:OnChanged (sender, e)
		
			if (self.m.timer ~= null) then
				self.m.timer:Invalidate ();
				self.m.timer:Dispose ();
				self.m.timer = nil;
			end

			self.m.timer = NSTimer.CreateScheduledTimer (0.7, NSAction (function ()
				self:SaveFileContent ();
				end
			));
end

function ScriptViewController:SaveFileContent ()
	self.m.store:SaveScriptContent (self.m.ScriptName, self.TextView.Text);
end

function ScriptViewController:WillHideViewControllerZeugma (splitController, viewController, barButtonItem, popoverController)
			barButtonItem.Title = NSBundle.MainBundle.LocalizedString ("Scripts", "Scripts");
			self.NavigationItem:SetLeftBarButtonItem (barButtonItem, true);
			self.m.masterPopoverController = popoverController;
end

function  ScriptViewController:WillShowViewController ( svc,  vc, button)
			-- Called when the view is shown again in the split view, invalidating the button and popover controller.
			self.NavigationItem:SetLeftBarButtonItem (nil, true);
			self.m.masterPopoverController = null;
end

return ScriptViewController;