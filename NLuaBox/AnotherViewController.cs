using Foundation;
using System;
using UIKit;

namespace NLuaBox
{
    public partial class AnotherViewController : UIViewController
    {
        public AnotherViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            codeView.Text = "-- foo\n";

            string foo = codeView.Text;
            codeView.Text = foo;

            foo = codeView.Text;
            codeView.Text = foo;
            foo = codeView.Text;
            codeView.Text = foo;
            foo = codeView.Text;
            codeView.Text = foo;
            foo = codeView.Text;
            codeView.Text = foo;
            foo = codeView.Text;
            codeView.Text = foo;
            foo = codeView.Text;
            codeView.Text = foo;
            foo = codeView.Text;
            codeView.Text = foo;
            foo = codeView.Text;
            codeView.Text = foo;
        }
    }
}