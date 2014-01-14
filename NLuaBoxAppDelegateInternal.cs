using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using NLua;

namespace NLuaBox
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class NLuaBoxAppDelegateInternal : UIApplicationDelegate
	{
		// class-level declarations
		UINavigationController navigationController;
		UISplitViewController splitViewController;
		UIWindow window;

		Lua context = new Lua ();

		public UIWindow MainWindow { get { return window; } set { window = value; } }
		public UISplitViewController SplitViewController { get { return splitViewController; } set { splitViewController = value; } }
		public Lua Context { get { return context; } }
		static public NLuaBoxAppDelegateInternal AppDelegate { get; private set; }
		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			AppDelegate = this;
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.TintColor = UIColor.Purple;
			// load the appropriate UI, depending on whether the app is running on an iPhone or iPad
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				var controller = new ScriptListViewController ();
				navigationController = new UINavigationController (controller);
				window.RootViewController = navigationController;
			} else {
				var masterViewController = new ScriptListViewController ();
				var masterNavigationController = new UINavigationController (masterViewController);
				var detailViewController = new ScriptViewController (masterViewController.ScriptsStore);
				var detailNavigationController = new UINavigationController (detailViewController);

				masterViewController.ScriptViewController = detailViewController;
				
				splitViewController = new UISplitViewController ();
				splitViewController.WeakDelegate = detailViewController;
				splitViewController.ViewControllers = new UIViewController[] {
					masterNavigationController,
					detailNavigationController
				};
				
				window.RootViewController = splitViewController;
			}

			// make the window visible
			window.MakeKeyAndVisible ();
			
			return true;
		}

		void InitNLua()
		{
			context.LoadCLRPackage ();

			context.DoString ("package.path = package.path .. \";./scripts/sandbox/?.lua\"");
		}
	}
}

