using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLua;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace NLuaBox
{
	class NLuaBoxUITableViewSourceBinder : UITableViewSource, ILuaGeneratedType
	{
		public LuaTable __luaInterface_luaTable;
		public Type [] [] __luaInterface_returnTypes;

		public NLuaBoxUITableViewSourceBinder ()
		{

		}

		public NLuaBoxUITableViewSourceBinder (LuaTable luaTable, Type [] [] returnTypes)
		{
			__luaInterface_luaTable = luaTable;
			__luaInterface_returnTypes = returnTypes;
		}

		public LuaTable LuaInterfaceGetLuaTable ()
		{
			return __luaInterface_luaTable;
		}

		public int __luaInterface_base_NumberOfSections (UITableView tableView)
		{
			return base.NumberOfSections (tableView);
		}

		public string __luaInterface_base_TitleForHeader (UITableView tableView, int section)
		{
			return base.TitleForHeader (tableView, section);
		}


		public bool __luaInterface_base_CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			return base.CanEditRow (tableView, indexPath);
		}

		public void __luaInterface_base_CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			base.CommitEditingStyle (tableView, editingStyle, indexPath);
		}

		public void __luaInterface_base_RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			base.RowSelected (tableView, indexPath);
		}

		public void __luaInterface_base_AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
		{
			base.AccessoryButtonTapped (tableView, indexPath);
		}

		public override int NumberOfSections (UITableView tableView)
		{
			object [] args = new object [] {
				__luaInterface_luaTable ,
				tableView,
			};

			object [] inArgs = new object [] {
				__luaInterface_luaTable,
				tableView,
			};

			int [] outArgs = new int [] { };
			Type [] returnTypes = new Type [1] { typeof (int) };
			LuaFunction function = NLua.Method.LuaClassHelper.GetTableFunction (__luaInterface_luaTable, "NumberOfSections");
			return (int)NLua.Method.LuaClassHelper.CallFunction (function, args, returnTypes, inArgs, outArgs);
		}

		public override string TitleForHeader (UITableView tableView, int section)
		{
			object [] args = new object [] {
				__luaInterface_luaTable ,
				tableView,
			};

			object [] inArgs = new object [] {
				__luaInterface_luaTable,
				tableView,
			};

			int [] outArgs = new int [] { };
			Type [] returnTypes = new Type [1] { typeof (string) };
			LuaFunction function = NLua.Method.LuaClassHelper.GetTableFunction (__luaInterface_luaTable, "TitleForHeader");
			return (string)NLua.Method.LuaClassHelper.CallFunction (function, args, returnTypes, inArgs, outArgs);
		}


		public override int RowsInSection (UITableView tableview, int section)
		{
			object [] args = new object [] {
				__luaInterface_luaTable ,
				tableview,
				section,
			};

			object [] inArgs = new object [] {
				__luaInterface_luaTable,
				tableview,
				section,
			};

			int [] outArgs = new int [] { };
			Type [] returnTypes = new Type [1] { typeof(int) };
			LuaFunction function = NLua.Method.LuaClassHelper.GetTableFunction (__luaInterface_luaTable, "RowsInSection");
			return (int)NLua.Method.LuaClassHelper.CallFunction (function, args, returnTypes, inArgs, outArgs);
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			object [] args = new object [] {
				__luaInterface_luaTable ,
				tableView,
				indexPath,
			};

			object [] inArgs = new object [] {
				__luaInterface_luaTable,
				tableView,
				indexPath,
			};

			int [] outArgs = new int [] { };
			Type [] returnTypes = new Type [1] { typeof (UITableViewCell) };
			LuaFunction function = NLua.Method.LuaClassHelper.GetTableFunction (__luaInterface_luaTable, "GetCell");
			return (UITableViewCell)NLua.Method.LuaClassHelper.CallFunction (function, args, returnTypes, inArgs, outArgs);
		}


		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			object [] args = new object [] {
				__luaInterface_luaTable ,
				tableView,
				indexPath,
			};

			object [] inArgs = new object [] {
				__luaInterface_luaTable,
				tableView,
				indexPath,
			};

			int [] outArgs = new int [] { };
			Type [] returnTypes = new Type [1] { typeof (bool) };
			LuaFunction function = NLua.Method.LuaClassHelper.GetTableFunction (__luaInterface_luaTable, "CanEditRow");
			return (bool)NLua.Method.LuaClassHelper.CallFunction (function, args, returnTypes, inArgs, outArgs);

		}

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			object [] args = new object [] {
				__luaInterface_luaTable ,
				tableView,
				editingStyle,
				indexPath,
			};

			object [] inArgs = new object [] {
				__luaInterface_luaTable,
				tableView,
				editingStyle,
				indexPath,
			};

			int [] outArgs = new int [] { };
			Type [] returnTypes = new Type [1] { typeof (void) };
			LuaFunction function = NLua.Method.LuaClassHelper.GetTableFunction (__luaInterface_luaTable, "CommitEditingStyle");
			NLua.Method.LuaClassHelper.CallFunction (function, args, returnTypes, inArgs, outArgs);
		}
	}
}
