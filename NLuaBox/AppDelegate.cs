using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Chormatism;
using NLua;

namespace NLuaBox
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		UITextView codeView;
		Lua context = new Lua ();

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			string script = @"
-- this is a Lua script
function myFunc (v1, v2)
	return v1 + v2
end

print (myFunc(10,20))
";
			JLTextViewController viewController = new JLTextViewController (new NSString(script));

			UINavigationController navigationContorler = new UINavigationController (viewController);
			navigationContorler.NavigationBar.BarStyle = UIBarStyle.Black;

			codeView = viewController.TextView;

			viewController.TextView.KeyboardAppearance = UIKeyboardAppearance.Dark;
			navigationContorler.SetNavigationBarHidden (true, false);

			InitNLua ();

			SetupToolbarOnKeyboard (viewController.TextView);

			window.RootViewController = navigationContorler;
			window.MakeKeyAndVisible ();
			
			return true;
		}

		void InitNLua()
		{
			context.LoadCLRPackage ();

				// Import assemblies (remember link will remove unused types/methods).
				// http://docs.xamarin.com/guides/ios/advanced_topics/linker

			context.DoString (@"import ('System')
								import ('System','System.Drawing')
								import ('monotouch', 'MonoTouch.Foundation')
								import ('monotouch', 'MonoTouch.UIKit') 
								import ('NLuaBox') ");



			var printOutputFunc = typeof(AppDelegate).GetMethod ("Print");
			context.RegisterFunction ("print", this, printOutputFunc);
		}

		public void Print(string output, params object[] extra )
		{
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

		void OnRun ()
		{
			string script = codeView.Text;
			try {

				context.DoString (script);

			} catch (Exception e) {

			}
		}

		protected override void Dispose (bool disposing)
		{
			if (context != null && disposing) {
				context.Dispose ();
				context = null;
			}

			base.Dispose (disposing);
		}
	}
}

