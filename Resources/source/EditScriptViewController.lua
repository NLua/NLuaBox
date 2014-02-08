

local EditScriptViewController = {}


EditScriptViewController.mt = { __call = function(self, ...)
													return self.new (...)
											end
							  }

setmetatable (EditScriptViewController,EditScriptViewController.mt)

function EditScriptViewController.new (onDone, filename,...)
		
		local title = "Edit Script";
		if (String.IsNullOrEmpty (filename)) then
			title = "New Script";
		end

		EditScriptViewController.m = {}; -- create a table members to store Lua fields.
		EditScriptViewController.m.doneAction = onDone;
		
		luanet.make_object (EditScriptViewController, 'MonoTouch.Dialog.DialogViewController');

		EditScriptViewController.m.nameEntry = EntryElement ("Name", "Enter script name", filename);
		EditScriptViewController.Root = RootElement(title);
		local section = Section("script");
		section:Add(EditScriptViewController.m.nameEntry);
		EditScriptViewController.Root:Add(section);

		return EditScriptViewController;
end

function EditScriptViewController:ViewDidLoad ()
	self.base:ViewDidLoad();

	self.NavigationItem:SetLeftBarButtonItem(UIBarButtonItem (UIBarButtonSystemItem.Cancel, function () self:OnCancel() end), false);
	self.NavigationItem.RightBarButtonItem = UIBarButtonItem (UIBarButtonSystemItem.Done, function () self:OnDone() end);
	self.m.nameEntry:BecomeFirstResponder (false);
end

function EditScriptViewController:OnCancel ()
	self:DismissViewController(true, nil);
end

function EditScriptViewController:OnDone ()
	if (self.m.doneAction ~= nil) then
		self.m.doneAction (self.m.nameEntry.Value, function () self:DismissViewController (true, nil) end);
	end
end

return EditScriptViewController;