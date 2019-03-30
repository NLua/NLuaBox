using System;
using System.Collections.Generic;

using UIKit;
using Foundation;
using System.IO;
using System.Linq;

namespace NLuaBox
{
    class ScriptsDataSource : UITableViewSource
    {
        static readonly NSString CellIdentifier = new NSString("Cell");
        readonly List<string> scripts = new List<string>();
        readonly ScriptListViewController controller;

        public ScriptStore Store { get; }

        public ScriptsDataSource(ScriptListViewController controller)
        {
            this.controller = controller;
            Store = new ScriptStore(LocalPathPrepare.ScriptsPath);
        }

        public IList<string> Objects
        {
            get { return scripts; }
        }

        // Customize the number of sections in the table view.
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return scripts.Count;
        }

        // Customize the appearance of table view cells.
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellIdentifier, indexPath);

            string name = scripts[indexPath.Row];
            cell.TextLabel.Text = name;
            cell.DetailTextLabel.Text = Store.ReadFirstLine(name);

            return cell;
        }

        public bool Exists(string name)
        {
            // File.Exists return false comparing the case.
            return Store.Exists(name) || scripts.Any(s => s.Equals(name, StringComparison.OrdinalIgnoreCase));
        }


        public void Reload()
        {
            string[] scriptsFiles = Store.Scripts;

            scripts.Clear();

            foreach (string scriptFile in scriptsFiles)
            {
                string name = Path.GetFileName(scriptFile);
                scripts.Add(name);
            }

            scripts.Sort();
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            if (editingStyle == UITableViewCellEditingStyle.Delete)
            {
                string name = scripts[indexPath.Row];
                // Delete the row from the data source.
                scripts.RemoveAt(indexPath.Row);

                Store.RemoveFile(name);

                if (controller.ScriptViewController?.ScriptName == name)
                    controller.ScriptViewController.SetScript(null);

                controller.TableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Fade);
            }
            else if (editingStyle == UITableViewCellEditingStyle.Insert)
            {
                // Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                controller.ScriptViewController.SetScript(scripts[indexPath.Row]);
        }

        public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
        {
            string fileName = scripts[indexPath.Row];

            var editScriptViewController = EditScriptViewController.Create((edit, newName) => OnRenameScript(edit, fileName, newName), fileName);
            var nav = new UINavigationController(editScriptViewController);
            nav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

            controller.PresentViewController(nav, true, null);
        }

        void OnRenameScript(EditScriptViewController editViewController, string oldName, string newName)
        {
            newName = ScriptStore.FixName(newName);
            // No-op
            if (oldName == newName)
            {
                editViewController.Dismiss();
                return;
            }

            string existingFile = scripts.FirstOrDefault(s => s.Equals(newName, StringComparison.OrdinalIgnoreCase));
            existingFile = existingFile ?? newName;

            if (oldName.Equals(newName, StringComparison.OrdinalIgnoreCase) || !Exists(newName))
            {
                Store.RenameFile(oldName, newName, existingFile);
                editViewController.Dismiss();
                Reload();
                controller.TableView.ReloadData();
                if (controller.ScriptViewController?.ScriptName == oldName)
                    controller.ScriptViewController.SetScript(newName);
                return;
            }


            controller.ConfirmFileReplace(editViewController, newName, (edit, n) =>
             {
                 Store.RenameFile(oldName, newName, existingFile);
                 edit.Dismiss();
                 Reload();
                 controller.TableView.ReloadData();
                 if (controller.ScriptViewController?.ScriptName == oldName)
                     controller.ScriptViewController.SetScript(newName);
             });
        }

    }
}
