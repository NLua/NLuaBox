using System;

using UIKit;
using Foundation;

namespace NLuaBox
{
    public partial class ScriptListViewController : UITableViewController
    {
        public CodeViewController ScriptViewController { get; set; }

        ScriptsDataSource dataSource;
        ScriptRunner runner;

        public ScriptRunner Runner { get; private set; }

        protected ScriptListViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = NSBundle.MainBundle.GetLocalizedString("Scripts", "Scripts");

            // Perform any additional setup after loading the view, typically from a nib.
            NavigationItem.LeftBarButtonItem = EditButtonItem;

            var addButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, AddNewItem);
            addButton.AccessibilityLabel = "addButton";
            NavigationItem.RightBarButtonItem = addButton;

            ScriptViewController = (CodeViewController)((UINavigationController)SplitViewController.ViewControllers[1]).TopViewController;

            TableView.Source = dataSource = new ScriptsDataSource(this);

            ScriptViewController.Store = dataSource.Store;
            ScriptViewController.Runner = runner = new ScriptRunner(dataSource.Store);

            dataSource.Reload();
        }

        public override void ViewWillAppear(bool animated)
        {
            ClearsSelectionOnViewWillAppear = SplitViewController.Collapsed;
            base.ViewWillAppear(animated);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        void AddNewItem(object sender, EventArgs args)
        {
            var editScriptViewController = EditScriptViewController.Create(OnAddNewFile);
            var nav = new UINavigationController(editScriptViewController);
            nav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;

            PresentViewController(nav, true, null);
        }

        void WriteNewFileAndReloadList(string name)
        {
            dataSource.Store.WriteContent(name, "-- " + name + "\n");

            dataSource.Reload();

            TableView.ReloadData();

            NSIndexPath index = GetScriptIndex(name);

            TableView.SelectRow(index, true, UITableViewScrollPosition.None);
        }

        void OnAddNewFile(EditScriptViewController editScriptViewController, string name)
        {
            name = ScriptStore.FixName(name);

            if (!dataSource.Exists(name))
            {
                WriteNewFileAndReloadList(name);
                editScriptViewController.Dismiss();
                PerformSegue("showDetail", this);
                return;
            }

            ConfirmFileReplace(editScriptViewController, name, Replace_AddNew_Dismissed);
        }

        public void ConfirmFileReplace(EditScriptViewController editScriptViewController, string name, Action<EditScriptViewController, string> confirm)
        {
            var alert = new UIAlertController();
            string message = NSBundle.MainBundle.GetLocalizedString("Replace_Message", "\"{0}\" already exists. Do you want to replace it?");
            alert.Title = string.Format(message, name);
            var cancel = UIAlertAction.Create(NSBundle.MainBundle.GetLocalizedString("Cancel", "Cancel"),
                UIAlertActionStyle.Cancel, null);

            var replace = UIAlertAction.Create(NSBundle.MainBundle.GetLocalizedString("Replace", "Replace"),
                UIAlertActionStyle.Destructive, (obj) => confirm(editScriptViewController, name));
            alert.AddAction(replace);
            alert.AddAction(cancel);
            editScriptViewController.PresentViewController(alert, true, null);
        }

        void Replace_AddNew_Dismissed(EditScriptViewController editScriptViewController, string name)
        {
            WriteNewFileAndReloadList(name);
            editScriptViewController.Dismiss();
            PerformSegue("showDetail", this);
        }


        NSIndexPath GetScriptIndex (string name)
        {
            int index = dataSource.Objects.IndexOf(name);
            if (index == -1)
                return null;

            return NSIndexPath.FromRowSection(index, 0);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier != "showDetail")
                return;

            ScriptViewController = (CodeViewController)((UINavigationController)segue.DestinationViewController).TopViewController;

            var indexPath = TableView.IndexPathForSelectedRow;
            var item = dataSource.Objects[indexPath.Row];
            ScriptViewController.Runner = runner;
            ScriptViewController.Store = dataSource.Store;
            ScriptViewController.Title = item;
            ScriptViewController.SetScript(item);
            ScriptViewController.NavigationItem.LeftBarButtonItem = SplitViewController.DisplayModeButtonItem;
            ScriptViewController.NavigationItem.LeftItemsSupplementBackButton = true;
        }
    }
}
