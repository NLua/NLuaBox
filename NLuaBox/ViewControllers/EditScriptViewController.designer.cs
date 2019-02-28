// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace NLuaBox
{
    [Register ("EditScriptViewController")]
    partial class EditScriptViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField scriptName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (scriptName != null) {
                scriptName.Dispose ();
                scriptName = null;
            }
        }
    }
}