--
-- ScriptListViewController.lua : Main View Controller of NLuaBox implemented using Lua + Xamarin.iOS . 
-- UITableViewControler implemented using NLua + Xamarin.iOS.
--
-- Authors:
--	Vinicius Jarina (vinicius.jarina@xamarin.com)
-- Copyright 2013-2014 Xamarin Inc.
-- 
-- Licensed under MIT License
--

local ScriptListViewController = {}

ScriptsDataSource = require ('ScriptsDataSource')
EditScriptViewController = require ('EditScriptViewController')

require ('string_extra')

ScriptListViewController.mt = { __call = function(self, ...)
													return self.new (...)
											end
							  }

setmetatable (ScriptListViewController,ScriptListViewController.mt)

function ScriptListViewController.new (...)

		ScriptListViewController.m = {}; -- create a table members to store Lua fields
		ScriptListViewController.m.dataSource = nil;
		ScriptListViewController.m.ScriptViewController = nil;

		luanet.make_object (ScriptListViewController, 'MonoTouch.UIKit.UITableViewController');

		ScriptListViewController.m.dataSource = ScriptsDataSource (ScriptListViewController);
		ScriptListViewController.TableView.Source = ScriptListViewController.m.dataSource;

		ScriptListViewController.Title = NSBundle.MainBundle:LocalizedString ("Scripts", "Scripts");

		if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) then
			ScriptListViewController.PreferredContentSize = SizeF (320, 600);
			ScriptListViewController.ClearsSelectionOnViewWillAppear = false;
		end
		return ScriptListViewController;
end

function ScriptListViewController:GetScriptsStore ()
		return self.m.dataSource:GetScriptsStore();
end

function FixupName (file)
	if (file:EndsWith(".lua")) then
		return file;
	end
	return file .. ".lua";
end
function using (disposable, block)
	block (disposable);
	disposable:Dispose();
end
function ScriptListViewController:AddNewFile (file, onSuccess)

	file = FixupName (file);
	Console.WriteLine (" Inside AddNewFile {0}" , file);

	local actionAddFile = function ()
		Console.WriteLine (" Inside actionAddFile {0}" , file);
		
		local exists = self.m.dataSource:Exists (file);
		self.m.dataSource:AddFile (file);

		if (exists == false) then
			Console.WriteLine (" Calling using" );
			--self.TableView:BeginUpdates ();
			--using(NSIndexPath.FromRowSection (self.TableView:NumberOfRowsInSection (0), 0),
			--function (indexPath)
			--	self.TableView:InsertRows (luanet.make_array(NSIndexPath,{ indexPath }), UITableViewRowAnimation.Automatic);
			--end
		--	)
		--	self.TableView:EndUpdates ();
			self.m.dataSource:Reload ();
			self.TableView:ReloadData ();
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

				self.m.dataSource:Reload ();
				self.TableView:ReloadData ();
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

			local editFile = EditScriptViewController.new ( function (name, action) 
				self:RenameFile (indexPath, name, action);
			end, fileName);

			local nav = UINavigationController (editFile);
			nav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

			self:PresentViewController (nav, true, null);
end

function ScriptListViewController:AddNewItem ()

			local editFile = EditScriptViewController.new (function (name, action) 
				Console.WriteLine (" Calling AddNewFile" );
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

			local addButton = UIBarButtonItem (UIBarButtonSystemItem.Compose, function () self:AddNewItem() end);
			self.NavigationItem.RightBarButtonItem = addButton;
			self.m.dataSource:Reload ();
end

return ScriptListViewController;
