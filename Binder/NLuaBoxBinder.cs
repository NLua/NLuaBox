using MonoTouch.UIKit;
using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLuaBox.Binders
{
	class LuaEventArgsHandler : NLua.Method.LuaDelegate
	{
		void CallFunction (object sender, EventArgs eventArgs)
		{
			object [] args = new object [] { sender, eventArgs };
			object [] inArgs = new object [] { sender, eventArgs };
			int [] outArgs = new int [] { };
			base.CallFunction (args, inArgs, outArgs);
		}
	}

	class LuaButtonEventArgsHandler : NLua.Method.LuaDelegate
	{
		void CallFunction (object sender, UIButtonEventArgs eventArgs)
		{
			object [] args = new object [] { sender, eventArgs };
			object [] inArgs = new object [] { sender, eventArgs };
			int [] outArgs = new int [] { };
			base.CallFunction (args, inArgs, outArgs);
		}
	}

	class NLuaBoxBinder
	{
		static public void RegisterNLuaBox (Lua context)
		{
			context.RegisterLuaDelegateType (typeof (EventHandler<EventArgs>), typeof (LuaEventArgsHandler));
			context.RegisterLuaDelegateType (typeof (EventHandler), typeof (LuaEventArgsHandler));
			context.RegisterLuaDelegateType (typeof (EventHandler<UIButtonEventArgs>), typeof (LuaButtonEventArgsHandler));

			context.RegisterLuaClassType (typeof (UIViewController), typeof (NLuaBoxUIViewControllerBinder));
		}
	}
}
