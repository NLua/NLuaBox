using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLua;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace NLuaBox
{
	class NLuaBoxDialogViewControllerBinder : DialogViewController, ILuaGeneratedType
	{
		public LuaTable __luaInterface_luaTable;
		public Type [] [] __luaInterface_returnTypes;

		public NLuaBoxDialogViewControllerBinder () : base (UITableViewStyle.Grouped, null)
		{

		}

		public NLuaBoxDialogViewControllerBinder (LuaTable luaTable, Type [] [] returnTypes) : base (UITableViewStyle.Grouped, null)
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
	}
}
