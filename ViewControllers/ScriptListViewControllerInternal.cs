using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;
using System.Threading;

namespace NLuaBox
{
	public class ScriptListViewControllerInternal : UITableViewController
	{
		ScriptsDataSource dataSource;

		public ScriptListViewControllerInternal ()
			: base ()
		{
			dataSource = new ScriptsDataSource (this);

			TableView.Source = dataSource;

			Title = NSBundle.MainBundle.LocalizedString ("Scripts", "Scripts");

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				PreferredContentSize = new SizeF (320f, 600f);
				ClearsSelectionOnViewWillAppear = false;
			}
		}

		public ScriptsStore ScriptsStore {
			get {
				return dataSource.ScriptsStore;
			}
		}

		public ScriptViewControllerInternal ScriptViewController {
			get;
			set;
		}

        string FixupName(string file)
        {
            if (file.EndsWith(".lua"))
                return file;
            return file + ".lua";
        }


		void AddNewFile (string file, Action onSuccess)
		{
            file = FixupName(file);

			Action actionAddFile = () => {
				bool exists = dataSource.Exists (file);
				dataSource.AddFile (file);

				if (!exists) {
					using (var indexPath = NSIndexPath.FromRowSection (TableView.NumberOfRowsInSection (0), 0))
						TableView.InsertRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
				}
				onSuccess ();
			};

			ValidateFileName (file, actionAddFile);
		}

		void ValidateFileName (string name, Action onValidName)
		{
			if (!dataSource.IsValidName (name)) {

				UIAlertView alert = new UIAlertView ();
				alert.Title = "Invalid name";

				if (string.IsNullOrEmpty (name))
					alert.Message = "The name can't be empty";
				else
					alert.Message = string.Format ("The script name {0} is not valid", name);
				alert.AddButton ("OK");
				alert.Show ();
				return;
			}

			if (dataSource.Exists (name)) {
				UIAlertView alert = new UIAlertView ();
				alert.Title = "Replace file";
				alert.Message = string.Format ("The script {0} already exists, replace it?", name);
				alert.AddButton ("OK");
				alert.AddButton ("Cancel");
				alert.Dismissed += (sender, args) => {
					if (args.ButtonIndex == 0)
						onValidName ();
				};
				alert.Show ();
			} else {
				onValidName ();
			}
		}

		void RenameFile (NSIndexPath indexPath,string newName, Action onSuccess)
		{
			Action actioRenameFile = () => {
				bool exists = dataSource.Exists (newName);
				dataSource.RenameFile (indexPath, newName);
				if (!exists)
					TableView.ReloadRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
				else
					TableView.ReloadData ();
				onSuccess ();
			};

            newName = FixupName (newName);

			string oldName = dataSource.GetScriptName (indexPath);

			if (oldName == newName) {
				onSuccess ();
				return;
			}

			ValidateFileName (newName, actioRenameFile);
		}

		public void OnAccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
		{
			string fileName = dataSource.GetScriptName (indexPath);

			var editFile = new EditScriptViewControllerInternal ((name, action) => {
				RenameFile (indexPath, name, action);
			}, fileName);

			var nav = new UINavigationController (editFile);
			nav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

			PresentViewController (nav, true, null);
		}

		void AddNewItem (object sender, EventArgs args)
		{
			var editFile = new EditScriptViewControllerInternal ((name, action) => {
				AddNewFile (name, action);
			});
			var nav = new UINavigationController (editFile);
			nav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

			PresentViewController (nav, true, null);
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			NavigationItem.LeftBarButtonItem = EditButtonItem;

			var addButton = new UIBarButtonItem (UIBarButtonSystemItem.Compose, AddNewItem);
			NavigationItem.RightBarButtonItem = addButton;

			dataSource.Reload ();
		}
	}
}

