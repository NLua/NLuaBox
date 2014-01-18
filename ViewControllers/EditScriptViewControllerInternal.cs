using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace NLuaBox
{
	public class EditScriptViewControllerInternal : DialogViewController
	{
		Action<string, Action> doneAction;
		EntryElement nameEntry;

		public EditScriptViewControllerInternal (Action<string, Action> onDone, string fileName = "") : base (UITableViewStyle.Grouped, null)
		{
			string title;
			if (string.IsNullOrEmpty (fileName))
				title = "New Script";
			else
				title = "Edit Script";

			nameEntry = new EntryElement ("Name", "Enter script name", fileName);
			Root = new RootElement (title) {
				new Section ("Script") {
					nameEntry
				},
			};
			doneAction = onDone;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem (UIBarButtonSystemItem.Cancel, OnCancel), false);
			NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Done, OnDone);

			nameEntry.BecomeFirstResponder (false);
		}

		void OnCancel (object sender, EventArgs args)
		{
			DismissViewController (true, null);
		}

		void OnDone (object sender, EventArgs args)
		{
			if (doneAction != null)
				doneAction (nameEntry.Value, () => DismissViewController (true, null));
		}
	}
}
