using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using NLua;

namespace NLuaSample.Android
{
	[Activity (Label = "NLuaSample.Android", MainLauncher = true)]
	public class MainActivity : Activity
	{
		int count = 1;
		Lua context = new Lua (); 

		public string Func (double param1)
		{
			Console.WriteLine ("Inside Func {0}", param1);
			return "FuncReturn" + param1.ToString ();
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			context.LoadCLRPackage ();
			context ["instance"] = this;
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
				context.DoString (@" x = instance:Func (10 + 3*7) ");
				Console.WriteLine (" x = {0} ", context ["x"] );
			};
		}
	}
}


