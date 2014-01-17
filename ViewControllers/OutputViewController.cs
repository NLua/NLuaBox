using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;
using System.Text;

namespace NLuaBox
{
	public class OutputViewController : UIViewController
	{
		public UITextView backView;
		string scriptCode;
		NSTimer timer;

		public OutputViewController (string code)
			: base ()
		{
			scriptCode = code;
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

			View.BackgroundColor = UIColor.Clear;
			View.Frame = UIScreen.MainScreen.Bounds;
			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			backView = new UITextView (View.Frame);
			backView.AutocapitalizationType = UITextAutocapitalizationType.None;
			backView.AutocorrectionType = UITextAutocorrectionType.No;
			backView.Font = UIFont.FromName ("Menlo", 14.0f);
			backView.Editable = false;
		

			backView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			View.AddSubview (backView);


			var playButton = new UIBarButtonItem (UIBarButtonSystemItem.Stop, OnStop);
			NavigationItem.RightBarButtonItem = playButton;

			View.LayoutIfNeeded ();

			timer = NSTimer.CreateScheduledTimer (0.1, new NSAction (() => {
				EvalScript ();
			}));

			// Perform any additional setup after loading the view, typically from a nib.
		}

		void EvalScript ()
		{
			var context = NLuaBoxAppDelegateInternal.AppDelegate.Context;
			var printOutputFunc = typeof(OutputViewController).GetMethod ("OutputString");
			context.RegisterFunction ("print", this, printOutputFunc);
			context.RegisterFunction ("io.write", this, printOutputFunc);

            context ["self"] = this;

			try {
				context.DoString (scriptCode);
			} catch (Exception e) {
				ErrorString ("Error running script" + e.ToString ());
			}
		}

		void ErrorString (string str)
		{
			backView.TextColor = UIColor.Red;
			OutputStringRaw (str);
		}

		public void OutputString (string output, params object [] extra)
		{
			backView.TextColor = UIColor.DarkTextColor;
			OutputStringRaw (output, extra);
		}

		void OutputStringRaw (string output, params object [] extra)
		{
			StringBuilder builder = new StringBuilder ();
			builder.Append (backView.Text);
			builder.Append (output);
			foreach (object part in extra) {
				builder.Append (' ');
				builder.Append (part.ToString ());
			}
			backView.Text = builder.ToString ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.AllButUpsideDown;
		}

		void OnStop (object sender, EventArgs args)
		{
			DismissViewController (true, null);
		}
	}
}

