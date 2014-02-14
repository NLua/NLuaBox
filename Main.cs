//
// Main.cs : iOS Main file
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

namespace NLuaBox
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
