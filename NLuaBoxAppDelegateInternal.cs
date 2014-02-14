//
// NLuaBoxAppDelegateInternal.cs : iOS AppDelegate class
//
//
// Authors:
//	Vinicius Jarina (vinicius.jarina@xamarin.com)
//
// Copyright 2013-2014 Xamarin Inc.
// 
// Licensed under MIT License
//

using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using NLua;
using NLuaBox.Binders;
using System.IO;

namespace NLuaBox
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class NLuaBoxAppDelegateInternal : UIApplicationDelegate
	{
		// class-level declarations
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
			NLuaBoxBinder.RegisterNLuaBox (context);
			InitNLua ();

			bool res = true;

			try {
				string mainFile = Path.Combine(LocalPathPrepare.SourcePath, "main.lua");

				context.DoFile(mainFile);

				LuaFunction initFunction = context ["Init"] as LuaFunction;

				res = (bool)initFunction.Call (this).First ();

			} catch (Exception e) {

				Console.WriteLine (e.ToString ());
				ReportErrorAndRecoverSourceDir (e);
				return false;
			}
			return res;
		}

		void InitNLua()
		{
			context.LoadCLRPackage ();

			string source = "\";" + LocalPathPrepare.ScriptsPath + "/?.lua\"";
			string scripts = "\";" + LocalPathPrepare.SourcePath + "/?.lua\"";

			context.DoString ("package.path = package.path .. " + source + ".." + scripts);
		}

		void ReportErrorAndRecoverSourceDir (Exception e)
		{
			UIAlertView alert = new UIAlertView ();
			alert.Title = " Error running NLuaBox ";
			alert.Message = string.Format (" There was a error running the NLuaBox code ({0}) \n" +
				"The original will be reverted", e.ToString ());
			LocalPathPrepare.ResetLocalSourcePath();
			alert.AddButton ("OK");
			alert.Show ();
		}
	}
}

