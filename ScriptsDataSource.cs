using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NLuaBox
{
	class ScriptsDataSource : UITableViewSource
	{
		static readonly NSString CellIdentifier = new NSString ("Cell");

		readonly List<string> scripts = new List<string> ();
		readonly List<string> sources = new List<string> ();

		ScriptsStore store;

		readonly ScriptListViewControllerInternal controller;

		bool IsSourceCode (NSIndexPath index)
		{
			return IsSourceCode (index.Section);
		}

		bool IsSourceCode (int section)
		{
			return section == 1;
		}

		public ScriptsDataSource (ScriptListViewControllerInternal controller)
		{
			this.controller = controller;

			string basePath = LocalPathPrepare.LocalPath;

            string scritpPath = LocalPathPrepare.ScriptsPath;
            string sourcePath = LocalPathPrepare.SourcePath;

			store = new ScriptsStore(scritpPath, sourcePath);
		}

		public ScriptsStore ScriptsStore {
			get {
				return store;
			}
		}

		// Customize the number of sections in the table view.
		public override int NumberOfSections (UITableView tableView)
		{
			return 2;
		}

		public override string TitleForHeader (UITableView tableView, int section)
		{
			if (IsSourceCode (section))
				return "NLuaBox Source";
			return "Lua Scripts";
		}


		public override int RowsInSection (UITableView tableview, int section)
		{
			if (IsSourceCode (section))
				return sources.Count;
			return scripts.Count;
		}

		// Customize the appearance of table view cells.
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (CellIdentifier);
			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Default, CellIdentifier);

			if (IsSourceCode (indexPath)) {
				cell.TextLabel.Text = sources [indexPath.Row].ToString ();
				cell.Accessory = UITableViewCellAccessory.None;
			}
			else {
				cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
				cell.TextLabel.Text = scripts [indexPath.Row].ToString ();
			}
			return cell;
		}


		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			return !IsSourceCode (indexPath);
			// Return false if you do not want the specified item to be editable.
		}


		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			if (editingStyle == UITableViewCellEditingStyle.Delete) {
				// Delete the row from the data source.
				if (controller.ScriptViewController != null && scripts [indexPath.Row] == controller.ScriptViewController.ScriptName)
					controller.ScriptViewController.LoadScript (null, false);
				RemoveFile (indexPath.Row);
				controller.TableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
			} else if (editingStyle == UITableViewCellEditingStyle.Insert) {
				// Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
			}
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			string name;
			bool isSource = IsSourceCode (indexPath);

			if (isSource)
				name = sources [indexPath.Row];
			else
				name = scripts [indexPath.Row];

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				if (controller.ScriptViewController == null)
					controller.ScriptViewController = new ScriptViewControllerInternal (ScriptsStore);

				controller.ScriptViewController.LoadScript (name, isSource);

				// Pass the selected object to the new view controller.
				controller.NavigationController.PushViewController (controller.ScriptViewController, true);
			} else {
				controller.ScriptViewController.LoadScript (name, isSource);
			}
		}

		public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
		{
			controller.OnAccessoryButtonTapped (tableView, indexPath);
		}

		public void Reload ()
		{
			foreach (string script in store.Scripts) {
				string name = Path.GetFileName (script);
				scripts.Add (name);
			}

            scripts.Sort();

			foreach (string script in store.Sources) {
				string name = Path.GetFileName (script);
				sources.Add (name);
			}

            sources.Sort();
		}

		public bool Exists (string file)
		{
			return store.Exists (file);
		}

		public bool IsValidName (string file)
		{
			return !string.IsNullOrWhiteSpace (file);
		}

		public void RenameFile (NSIndexPath indexPath, string newName)
		{
			int removeIndex = -1;
			if (Exists (newName))
				removeIndex = scripts.IndexOf (newName);

			int row = indexPath.Row;
			string oldName = scripts [row];
			scripts [row] = newName;
			store.RenameFile (oldName, newName);

			if (removeIndex != -1)
				scripts.RemoveAt (removeIndex);
            if (controller.ScriptViewController != null)
			    controller.ScriptViewController.LoadScript (newName, false);
		}

		public void AddFile (string file)
		{
			if (!Exists (file))
				scripts.Add (file);

			store.SaveScriptContent (file, "");

            if (controller.ScriptViewController != null)
			    controller.ScriptViewController.LoadScript (file, false);
		}

		public void RemoveFile (int row)
		{
			string file = (string)scripts [row];
			scripts.RemoveAt (row);
			store.RemoveFile (file);
		}

		public string GetScriptName (NSIndexPath index)
		{
			if (IsSourceCode (index))
				return sources [index.Row];
			return (string)scripts [index.Row];
		}
	}

}

