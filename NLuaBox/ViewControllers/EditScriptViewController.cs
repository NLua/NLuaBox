using Foundation;
using System;
using UIKit;

namespace NLuaBox
{
    public partial class EditScriptViewController : UITableViewController
    {
        public Action<EditScriptViewController, string> DoneAction { get; set; }

        string initialScriptName = string.Empty;

        public EditScriptViewController(IntPtr handle) : base(handle)
        {
        }

        public static EditScriptViewController Create(Action<EditScriptViewController, string> onDone)
        {
            return Create(onDone, string.Empty);
        }

        public static EditScriptViewController Create(Action<EditScriptViewController, string> onDone, string fileName)
        {
            UIStoryboard mainStoryboard = UIStoryboard.FromName("Main", null);
            EditScriptViewController viewController = (EditScriptViewController)mainStoryboard.InstantiateViewController("EditScriptViewController");

            if (string.IsNullOrEmpty(fileName))
                viewController.Title = NSBundle.MainBundle.GetLocalizedString("New_Script", "New Script");
            else
                viewController.Title = NSBundle.MainBundle.GetLocalizedString("Edit_Script", "Edit Script");

            viewController.DoneAction = onDone;
            viewController.initialScriptName = fileName;
            return viewController;
        }

        void ScriptName_ValueChanged(object sender, EventArgs e)
        {
            NavigationItem.RightBarButtonItem.Enabled = !string.IsNullOrEmpty(scriptName.Text) && scriptName.Text != initialScriptName;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, OnCancel);
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, OnDone);
            NavigationItem.RightBarButtonItem.Enabled = false;
            scriptName.EditingChanged += ScriptName_ValueChanged;
            scriptName.WeakDelegate = this;
            scriptName.Text = initialScriptName;
            scriptName.BecomeFirstResponder();
        }

        void OnCancel(object sender, EventArgs e)
        {
            Dismiss();
        }

        [Export("textFieldShouldReturn:")]
        void TextFieldShouldReturn(UITextField field)
        {
            if (string.IsNullOrEmpty(scriptName.Text))
                return;

            OnDone(this, EventArgs.Empty);
        }

        void OnDone(object sender, EventArgs e)
        {
            if (DoneAction == null)
                return;

            DoneAction.Invoke(this, scriptName.Text);
        }

        public void Dismiss()
        {
            DismissViewController(true, null);
        }
    }
}