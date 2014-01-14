using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Chormatism;


namespace NLuaBox
{
	public class ScriptViewController : JLTextViewController
	{
		UIPopoverController masterPopoverController;
		string scriptName;
		bool isSource;

		ScriptsStore store;
		NSTimer timer;

		public ScriptViewController (ScriptsStore store)
			: base ()
		{
			this.store = store;

			// Custom initialization
		}

		public string ScriptName {
			get {
				return scriptName;
			}
		}

		public bool IsSource {
			get {
				return isSource;
			}
		}

		public void LoadScript (string name, bool source)
		{
			if (scriptName != name) {
				scriptName = name;
				isSource = source;
				// Update the view
				ConfigureView ();
			}
			
			if (masterPopoverController != null)
				masterPopoverController.Dismiss (true);
		}

		void EnablePlayButton ()
		{
			var playButton = new UIBarButtonItem (UIBarButtonSystemItem.Play, OnRun);
			NavigationItem.RightBarButtonItem = playButton;
		}

		void DisablePlayButton ()
		{
			NavigationItem.RightBarButtonItem = null;
		}

		void ConfigureView ()
		{
			// Update the user interface for the detail item
			if (!IsViewLoaded)
				return;
			if (scriptName != null) {
				if (isSource) {
					TextView.Text = store.GetSourceContent (scriptName);
					DisablePlayButton ();
				}
				else {
					TextView.Text = store.GetScriptContent (scriptName);
					EnablePlayButton ();
				}
					
				TextView.Editable = true;
				Title = scriptName;
			} else {
				TextView.Text = " -- <no script selected>";
				TextView.Editable = false;
				Title = "Script";
				DisablePlayButton ();
			}
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

			View.Frame = UIScreen.MainScreen.Bounds;
			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			TextView.Changed += OnChanged;
			ConfigureView ();
		}

		void OnRun (object sender, EventArgs args)
		{
			string script = TextView.Text;

			var output = new OutputViewController (script);
			ModalPresentationStyle =  UIModalPresentationStyle.CurrentContext;
			var nav = new UINavigationController (output);
			PresentViewController (nav, true, null);
		}


		void OnChanged (object sender, EventArgs e)
		{
			if (timer != null) {
				timer.Invalidate ();
				timer.Dispose ();
				timer = null;
			}

			timer = NSTimer.CreateScheduledTimer (0.7, new NSAction (() => {
				SaveFileContent ();
			}));
		}

		void SaveFileContent ()
		{
			store.SaveScriptContent (scriptName, TextView.Text);
		}

		[Export ("splitViewController:willHideViewController:withBarButtonItem:forPopoverController:")]
		public void WillHideViewControllerZeugma (UISplitViewController splitController, UIViewController viewController, UIBarButtonItem barButtonItem, UIPopoverController popoverController)
		{
			barButtonItem.Title = NSBundle.MainBundle.LocalizedString ("Scripts", "Scripts");
			NavigationItem.SetLeftBarButtonItem (barButtonItem, true);
			masterPopoverController = popoverController;
		}

		[Export ("splitViewController:willShowViewController:invalidatingBarButtonItem:")]
		public void WillShowViewControllerZeugma (UISplitViewController svc, UIViewController vc, UIBarButtonItem button)
		{
			// Called when the view is shown again in the split view, invalidating the button and popover controller.
			NavigationItem.SetLeftBarButtonItem (null, true);
			masterPopoverController = null;
		}
	}
}

