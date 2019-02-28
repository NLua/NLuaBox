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
    [Register ("AnotherViewController")]
    partial class AnotherViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LuaCodeViewKit.LuaCodeView codeView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (codeView != null) {
                codeView.Dispose ();
                codeView = null;
            }
        }
    }
}