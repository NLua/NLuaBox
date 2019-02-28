using System;
using Foundation;
using UIKit;

namespace NLuaBox
{
    public partial class CodeViewController : UIViewController
    {
        public string ScriptName { get; set; }
        public ScriptStore Store { get; set; }
        public ScriptRunner Runner { get; set; }

        const string NLuaAsciiArt =
 @"
=======================================+
==============================+OZZO====+
=============================$OOOOOOZ==+
+++++++++++++++++++++++++++++OOOOOOOO+++
+++++++++++++OOZZZZZZZZZOO+++OOOOOOOO+++
+++++++++++OOOOOOOOOOOOOOOOO++=OOOO$+++?
?????????OOOOOOOOOOOOOO....?OO??????????
????????OOOOOOOOOOOOO~.......OO?????????
???????O8888888888888........888???????I
I?????I88888888888888~.. ....8887IIIIIII
IIIIII8888888888888888D....?88888IIIIIII
IIIIIIDDDDDDDDDDDDDDDDDDDDDDDDDD8IIIIIII
777777DDDDDDDDDDDDDDDDDDDDDDDDDDD7III777
777777DDDDDDDDDDDDDDDDDDDDDDDDDDD7777777
777777$DDDDDDDDDDDDDDDDDDDDDDDDDZ7777777
77$7777DDDDDDDDDDDDDDDDDDDDDDDDD7777777$
$$$$$$$$NNNNNNNNNNNNNNNNNNNNNNN$$$$$$$$$
$$$$$$$$$NNNNNNNNNNNNNNNNNNNNN$$$$$$$$$$
$$$$$$$$$$ZNNNNNNNNNNNNNNNNNZ$$$$$$$$$$Z
ZZZZZZZZZZZZZDNNNNNNNNNNNN$ZZZZZZZZZZZZZ
ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ
ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ
ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ
ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ
ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZO";

        NSObject hideKeyboardObserver;
        NSObject showKeyboardObserver;

        protected CodeViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void SetScript(string scriptName)
        {
            if (ScriptName == scriptName)
                return;

            ScriptName = scriptName;
        }
        string FixiOSBUG(string content)
        {
            content = content.Replace("\x2014", "--");
            content = content.Replace("\x201C", "\"");
            content = content.Replace("\x201D", "\"");
            return content;
        }
        void ConfigureView()
        {
            if (!IsViewLoaded)
                return;
            // Update the user interface for the detail item
            if (string.IsNullOrEmpty(ScriptName))
            {
                codeView.Text = "-- " + NSBundle.MainBundle.GetLocalizedString("Select_Script", "Select or create a script") + NLuaAsciiArt;
                DisablePlayButton();

                return;
            }

            codeView.Text = ReadFileContent();
            codeView.Text = FixiOSBUG(codeView.Text);

            EnablePlayButton();
        }

        void EnablePlayButton()
        {
            if (NavigationItem.RightBarButtonItem != null)
                return;

            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Play, OnRun);
        }

        void DisablePlayButton()
        {
            if (NavigationItem.RightBarButtonItem == null)
                return;

            NavigationItem.RightBarButtonItem.Clicked -= OnRun;
            NavigationItem.RightBarButtonItem = null;
        }

        void OnRun(object sender, EventArgs e)
        {
            SaveFileContent();

            var outputView = OutputViewController.Create(ScriptName, Store, Runner);

            var navigation = new UINavigationController(outputView);

            navigation.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;

            PresentViewController(navigation, true, null);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            ConfigureView();

            codeView.BecomeFirstResponder();
        }

        public override void ViewWillDisappear(bool animated)
        {
            UnregisterForKeyboardNotifications();

            SaveFileContent();

            base.ViewWillDisappear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            RegisterForKeyboardNotifications();

        }

        void SaveFileContent()
        {
            if (codeView == null || string.IsNullOrEmpty(ScriptName))
                return;

            string content = codeView.Text;

            Store.WriteContent(ScriptName, content);
        }

        string ReadFileContent()
        {
            return Store.ReadContent(ScriptName);
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        void RegisterForKeyboardNotifications()
        {
            if (!IsViewLoaded || View.Window == null)
                return;

            if (hideKeyboardObserver == null)
                hideKeyboardObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardWillHideNotification);

            if (showKeyboardObserver == null)
                showKeyboardObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardWillShowNotification);
        }

        void UnregisterForKeyboardNotifications()
        {
            if (hideKeyboardObserver != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(hideKeyboardObserver);

            if (showKeyboardObserver != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(showKeyboardObserver);

            showKeyboardObserver = null;
            hideKeyboardObserver = null;
        }

        private void OnKeyboardWillHideNotification(NSNotification notification)
        {
            if (!IsViewLoaded || View.Window == null)
                return;

            double duration = UIKeyboard.AnimationDurationFromNotification(notification);

            //Pass the notification, calculating keyboard height, etc.

            UIView.Animate(duration, () => bottomContraint.Constant = 0, null);
        }

        private void OnKeyboardWillShowNotification(NSNotification notification)
        {
            if (!IsViewLoaded || View.Window == null)
                return;


            double duration = UIKeyboard.AnimationDurationFromNotification(notification);

            //Pass the notification, calculating keyboard height, etc.
            var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
            nfloat newConstant = keyboardFrame.Height;

            UIView.Animate(duration, () => bottomContraint.Constant = newConstant, null);
        }

    }
}

