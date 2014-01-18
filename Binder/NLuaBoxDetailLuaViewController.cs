using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLua;
using MonoTouch.UIKit;
using Chormatism;
using MonoTouch.Foundation;

namespace NLuaBox
{
	class NLuaBoxDetailLuaViewController : JLTextViewController, ILuaGeneratedType
	{
		public LuaTable __luaInterface_luaTable;
		public Type [] [] __luaInterface_returnTypes;

		public NLuaBoxDetailLuaViewController ()
		{

		}

		public NLuaBoxDetailLuaViewController (LuaTable luaTable, Type [] [] returnTypes)
		{
			__luaInterface_luaTable = luaTable;
			__luaInterface_returnTypes = returnTypes;
		}

		public LuaTable LuaInterfaceGetLuaTable ()
		{
			return __luaInterface_luaTable;
		}

		public void __luaInterface_base_ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

		public override void ViewDidLoad ()
		{
			object [] args = new object [] {
				__luaInterface_luaTable ,
			};

			object [] inArgs = new object [] {
				__luaInterface_luaTable,
			};

			int [] outArgs = new int [] { };
			Type [] returnTypes = new Type [1] { typeof(void) };
			LuaFunction function = NLua.Method.LuaClassHelper.GetTableFunction (__luaInterface_luaTable, "ViewDidLoad");
			NLua.Method.LuaClassHelper.CallFunction (function, args, returnTypes, inArgs, outArgs);
		}

		[Export ("splitViewController:willHideViewController:withBarButtonItem:forPopoverController:")]
		public void WillHideViewController (UISplitViewController splitController, UIViewController viewController, UIBarButtonItem barButtonItem, UIPopoverController popoverController)
		{
			object [] args = new object [] {
				__luaInterface_luaTable ,
				splitController,
				viewController,
				barButtonItem,
				popoverController
			};

			object [] inArgs = new object [] {
				__luaInterface_luaTable,
				splitController,
				viewController,
				barButtonItem,
				popoverController
			};

			int [] outArgs = new int [] { };
			Type [] returnTypes = new Type [1] { typeof(void) };
			LuaFunction function = NLua.Method.LuaClassHelper.GetTableFunction (__luaInterface_luaTable, "WillHideViewController");
			NLua.Method.LuaClassHelper.CallFunction (function, args, returnTypes, inArgs, outArgs);
		}

		[Export ("splitViewController:willShowViewController:invalidatingBarButtonItem:")]
		public void WillShowViewController (UISplitViewController svc, UIViewController vc, UIBarButtonItem button)
		{
			object [] args = new object [] {
				__luaInterface_luaTable ,
				svc,
				vc,
				button,
			};

			object [] inArgs = new object [] {
				__luaInterface_luaTable,
				svc,
				vc,
				button,
			};

			int [] outArgs = new int [] { };
			Type [] returnTypes = new Type [1] { typeof(void) };
			LuaFunction function = NLua.Method.LuaClassHelper.GetTableFunction (__luaInterface_luaTable, "WillShowViewController");
			NLua.Method.LuaClassHelper.CallFunction (function, args, returnTypes, inArgs, outArgs);
		}
	}
}
