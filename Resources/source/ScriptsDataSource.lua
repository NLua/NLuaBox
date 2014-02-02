ScriptStore = require ('ScriptStore')
ScriptViewController = require ('ScriptViewController')

local ScriptsDataSource = {}

ScriptsDataSource.mt = {__call = function(self, ...)
													return self.new (...)
											end
								  }
setmetatable (ScriptsDataSource,ScriptsDataSource.mt)


g_CellIdentifier = NSString ("Cell");


function ScriptsDataSource.new (controller)
	
	ScriptsDataSource.m = {}; -- create a table members to store Lua fields
	ScriptsDataSource.m.scripts = CreateListString();
	ScriptsDataSource.m.sources = CreateListString();
	if (controller == nil) then
		error ("ArgumentError: controller cannot be null");
	end
		
	ScriptsDataSource.m.controller = controller;

	local scritpPath = LocalPathPrepare.ScriptsPath;
	local sourcePath = LocalPathPrepare.SourcePath;

	Console.WriteLine ("Construindo ScriptStore");
	ScriptsDataSource.m.store = ScriptStore (scritpPath, sourcePath);

	Console.WriteLine ("ScriptsDataSource ctor");


	luanet.make_object (ScriptsDataSource, 'MonoTouch.UIKit.UITableViewSource');


	return ScriptsDataSource;
end



function ScriptsDataSource:IsSourceCodeNumber(number)
	return number == 1;
end

function ScriptsDataSource:IsSourceCode(indexPath)
	return self:IsSourceCodeNumber(indexPath.Section);
end

function ScriptsDataSource:GetScriptsStore ()
		return self.m.store;
end

-- Customize the number of sections in the table view.
function ScriptsDataSource:NumberOfSections (tableView)
	return 2;
end

function ScriptsDataSource:TitleForHeader (tableView, section)
	Console.WriteLine ("TitleForHeader {0}", section == nil);
	if (self:IsSourceCodeNumber (section))then
		return "NLuaBox Source";
	end
	return "Lua Scripts";
end


function ScriptsDataSource:RowsInSection ( tableview, section)

		Console.WriteLine ("ScriptsDataSource: RowsInSection");

		if (self:IsSourceCodeNumber (section))then
			return self.m.sources.Count;
		end
		return self.m.scripts.Count;
end

-- Customize the appearance of table view cells.
function ScriptsDataSource:GetCell ( tableView,  indexPath)
	
	local cell = tableView:DequeueReusableCell (g_CellIdentifier);
	if (cell == nil) then
		cell =  UITableViewCell (UITableViewCellStyle.Default, g_CellIdentifier);
	end

	if (self:IsSourceCode (indexPath)) then
		cell.TextLabel.Text = self.m.sources[indexPath.Row];
		cell.Accessory = UITableViewCellAccessory.None;
	else 
		cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
		cell.TextLabel.Text = self.m.scripts[indexPath.Row];
	end
	return cell;
end


function ScriptsDataSource:CanEditRow (tableView, indexPath)
	return not self:IsSourceCode (indexPath);
	-- Return false if you do not want the specified item to be editable.
end


function ScriptsDataSource:CommitEditingStyle (tableView,  editingStyle,  indexPath)
		
	if (editingStyle == UITableViewCellEditingStyle.Delete) then
		-- Delete the row from the data source.
		local controller = self.m.controller;
		if (controller.m.ScriptViewController ~= nil and scripts[indexPath.Row] == controller.m.ScriptViewController.ScriptName) then
			controller.m.ScriptViewController:LoadScript (null, false);
		end
		self:RemoveFile (indexPath.Row);
		controller.TableView:DeleteRows (luanet.make_array(NSIndexPath, { indexPath }), UITableViewRowAnimation.Fade);
	elseif (editingStyle == UITableViewCellEditingStyle.Insert) then
		-- Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
	end
end

function ScriptsDataSource:RowSelected (tableView, indexPath)
		
	local name = nil;
	local isSource = self:IsSourceCode (indexPath);
	local controller = self.m.controller;
	Console.WriteLine ("RowSelected");

	if (isSource)then
		name = self.m.sources[indexPath.Row];
	else
		name = self.m.scripts[indexPath.Row];
	end

	if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) then
		if (controller.m.ScriptViewController == nil) then
			controller.m.ScriptViewController =  ScriptViewController (self:GetScriptsStore());
		end

		controller.m.ScriptViewController:LoadScript (name, isSource);

		-- Pass the selected object to the new view controller.
		controller.NavigationController:PushViewController (controller.m.ScriptViewController, true);
	else
		controller.m.ScriptViewController:LoadScript (name, isSource);
	end
end

function ScriptsDataSource:AccessoryButtonTapped ( tableView,  indexPath)
	self.m.controller:OnAccessoryButtonTapped (tableView, indexPath);
end

function ScriptsDataSource:Reload ()
	
	Console.WriteLine ("Store is null (3): {0} ", self.m.store == nil);
	local store = self:GetScriptsStore ();
	local scripts = store:GetScripts();
	local sources = store:GetSources();

	for script in luanet.each(scripts) do
		local name = Path.GetFileName (script);
		self.m.scripts:Add (name);
	end

	self.m.scripts:Sort();

	for script in luanet.each(sources) do
		local name = Path.GetFileName (script);
		self.m.sources:Add (name);
	end

	self.m.sources:Sort();
end

function ScriptsDataSource:Exists (file)
	return self.m.store:Exists (file);
end

function ScriptsDataSource:IsValidName (file)
	return not String.IsNullOrWhiteSpace (file);
end

function ScriptsDataSource:RenameFile (indexPath, newName)
	local removeIndex = -1;

	if (self:Exists (newName)) then
		removeIndex = self.m.scripts:IndexOf (newName);
	end

	local row = indexPath.Row;
	local oldName = self.m.scripts [row];
	self.m.scripts [row] = newName;
	self.m.store:RenameFile (oldName, newName);

	if (removeIndex ~= -1) then
		self.m.scripts:RemoveAt (removeIndex);
	end

	if (self.m.controller.m.ScriptViewController ~= null) then
		self.m.controller.m.ScriptViewController:LoadScript (newName, false);
	end
end

function ScriptsDataSource:AddFile (file)
		
	if (not self:Exists (file)) then
		self.m.scripts:Add (file);
	end

	self.m.store:SaveScriptContent (file, "");

	if (self.m.controller.m.ScriptViewController ~= nil) then
		self.m.controller.m.ScriptViewController:LoadScript (file, false);
	end
end

function ScriptsDataSource:RemoveFile (row)
	local file = self.m.scripts [row];
	self.m.scripts:RemoveAt (row);
	self.m.store:RemoveFile (file);
end

function ScriptsDataSource:GetScriptName (index)
	
	if (self:IsSourceCode (index)) then
		return self.m.sources [index.Row];
	end

	return self.m.scripts [index.Row];
end

return ScriptsDataSource;
