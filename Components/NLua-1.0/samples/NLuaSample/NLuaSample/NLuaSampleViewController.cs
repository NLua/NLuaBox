using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using NLua;

namespace NLuaSample
{
	public partial class NLuaSampleViewController : UIViewController
	{
		Lua lua;
		bool error = false;

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public NLuaSampleViewController ()
			: base (UserInterfaceIdiomIsPhone ? "NLuaSampleViewController_iPhone" : "NLuaSampleViewController_iPad", null)
		{
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

			string script = System.IO.File.ReadAllText ("sample.lua");

			codeView.Text = script;
			codeView.TextColor = UIColor.Green;

			// Initialize the Lua interpreter.
			if (lua == null) {
				lua = new Lua ();

				lua.LoadCLRPackage ();

				// Import assemblies (remember link will remove unused types/methods).
				// http://docs.xamarin.com/guides/ios/advanced_topics/linker

				lua.DoString (@"import ('System')
						import ('System','System.Net')
						import ('System','System.Drawing')
						import ('monotouch', 'MonoTouch.Foundation')
						import ('monotouch', 'MonoTouch.UIKit') 
						import ('NLuaSample') ");

			}
			lua ["self"] = codeView;
			var printOutputFunc = typeof(NLuaSampleViewController).GetMethod ("PrintOutput");
			lua.RegisterFunction ("print", this, printOutputFunc);

			SetupToolbarOnKeyboard (codeView);
		}

		public void PrintOutput (string output)
		{
			outputView.Text += output + "\n";
		}

		private void OnRun ()
		{
			string script = codeView.Text;
			try {
				if (error) {
					outputView.TextColor = UIColor.DarkTextColor;
					outputView.Text = string.Empty;
					error = false;
				}

				lua.DoString (script);

			} catch (Exception e) {
				outputView.TextColor = UIColor.Red;
				outputView.Text = "ERROR: " + e;
				if (e.InnerException != null)
					outputView.Text += "\n" + e.InnerException;
				error = true;
			}
		}

		public void SetupToolbarOnKeyboard (UITextView txt)
		{
			UIToolbar toolbar = new UIToolbar ();
			toolbar.BarStyle = UIBarStyle.Black;
			toolbar.Translucent = true;
			toolbar.SizeToFit ();
			UIBarButtonItem doneButton = new UIBarButtonItem ("Close", UIBarButtonItemStyle.Done,
			                                                  (s, e) => {
				txt.ResignFirstResponder ();
			});
			doneButton.TintColor = UIColor.Gray;

			UIBarButtonItem goButton = new UIBarButtonItem ("Run", UIBarButtonItemStyle.Done,
			                                                  (s, e) => {

				txt.ResignFirstResponder ();
				OnRun ();
			});

			toolbar.SetItems (new UIBarButtonItem[]{doneButton, goButton}, true);
			
			txt.InputAccessoryView = toolbar;
		}

		protected override void Dispose (bool disposing)
		{
			if (lua != null && disposing) {
				lua.Dispose ();
				lua = null;
			}

			base.Dispose (disposing);
		}
	}
}

