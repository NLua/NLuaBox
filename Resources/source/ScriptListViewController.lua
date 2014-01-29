-- ScriptListViewController . 
-- UITableViewControler implemented using NLua + Xamarin.iOS.

local ScriptListViewController = {}

ScriptsDataSource = require ('ScriptsDataSource')

require ('string_extra')

ScriptListViewController.mt = { __call = function(self, ...)
													return self.new (...)
											end
							  }

setmetatable (ScriptListViewController,ScriptListViewController.mt)

function ScriptListViewController.new (...)

		ScriptListViewController.m = {}; -- create a table members to store Lua fields
		ScriptListViewController.m.dataSource = ScriptsDataSource (self);
		ScriptListViewController.m.ScriptViewController = nil;

		luanet.make_object (ScriptListViewController, 'MonoTouch.UIKit.UITableViewController');

		ScriptListViewController.TableView.Source = ScriptsDataSource.m.dataSource;

		ScriptListViewController.Title = NSBundle.MainBundle:LocalizedString ("Scripts", "Scripts");

		if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) then
			ScriptListViewController.PreferredContentSize = SizeF (320, 600);
			ScriptListViewController.ClearsSelectionOnViewWillAppear = false;
		end
		Console.WriteLine (' {0} Dasdas ', ScriptListViewController.m.dataSource.m.store == nil);
		return ScriptListViewController;
end

function FixupName(file)
    if (file:ends(".lua")) then
	return file;
     end
    return file  ".lua";
end

function ScriptListViewController:AddNewFile (file, onSuccess)

	file = FixupName (file);

		local actionAddFile = function ()

			local exists = self.m.dataSource:Exists (file);
			self.m.dataSource:AddFile (file);

			if (exists == false) then
				local indexPath = NSIndexPath.FromRowSection (TableView:NumberOfRowsInSection (0), 0)
				TableView.InsertRows (luanet.make_array(NSIndexPath,{ indexPath }), UITableViewRowAnimation.Automatic);
				indexPath:Dispose()
			end
			onSuccess ();
		end;

		self:ValidateFileName (file, actionAddFile);
end

function ScriptListViewController:ValidateFileName (name, onValidName)

		if (not self.m.dataSource:IsValidName (name)) then

			local alert = UIAlertView ();
			alert.Title = "Invalid name";

			if (String.IsNullOrEmpty (name)) then
				alert.Message = "The name can't be empty";
			else
				alert.Message = String.Format ("The script name {0} is not valid", name);
			end

			alert:AddButton ("OK");
			alert:Show ();
			return;
		end

		if (self.m.dataSource:Exists (name)) then
			local alert = UIAlertView ();
			alert.Title = "Replace file";
			alert.Message = String.Format ("The script {0} already exists, replace it?", name);
			alert:AddButton ("OK");
			alert:AddButton ("Cancel");
			alert.Dismissed:Add( function (sender, args)
				if (args.ButtonIndex == 0) then
					onValidName ();
				end
			end);
			alert:Show ();
		else
			onValidName ();
		end
end

function ScriptListViewController:RenameFile (indexPath, newName, onSuccess)

			local actioRenameFile = function () 
				local exists = self.m.dataSource:Exists (newName);

				self.m.dataSource:RenameFile (indexPath, newName);
				if (exists == false) then
					self.TableView:ReloadRows (luanet.make_array (NSIndexPath,{ indexPath }), UITableViewRowAnimation.Automatic);
				else
					self.TableView:ReloadData ();
				end
				onSuccess ();
			end;

			newName = FixupName (newName);

			local oldName = self.m.dataSource:GetScriptName (indexPath);

			if (oldName == newName) then
				onSuccess ();
				return;
			end

			self:ValidateFileName (newName, actioRenameFile);
end

function  ScriptListViewController:OnAccessoryButtonTapped (tableView, indexPath)
		
			local fileName = self.m.dataSource:GetScriptName (indexPath);

			local editFile = EditScriptViewControllerInternal ( function (name, action) 
				self:RenameFile (indexPath, name, action);
			end, fileName);

			local nav = UINavigationController (editFile);
			nav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

			self:PresentViewController (nav, true, null);
end

function ScriptListViewController:AddNewItem (sender, args)

			local editFile = new EditScriptViewControllerInternal (function (name, action) 
				self:AddNewFile (name, action);
			end);
			local nav = UINavigationController (editFile);
			nav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

			self:PresentViewController (nav, true, null);
end

function ScriptListViewController:ViewDidLoad ()
		
			self.base:ViewDidLoad ();

			-- Perform any additional setup after loading the view, typically from a nib.
			self.NavigationItem.LeftBarButtonItem = EditButtonItem;

			local addButton = UIBarButtonItem (UIBarButtonSystemItem.Compose, AddNewItem);
			self.NavigationItem.RightBarButtonItem = addButton;

			self.m.dataSource:Reload ();
end

return ScriptListViewController;
