using Foundation;
using System;
using System.Text;
using UIKit;

namespace NLuaBox
{
    public partial class OutputViewController : UIViewController
    {
        NSTimer evalTimer;
        string scriptName;
        ScriptStore store;
        ScriptRunner runner;

        public OutputViewController (IntPtr handle) : base (handle)
        {
        }

        public static OutputViewController Create(string name, ScriptStore store, ScriptRunner runner)
        {
            UIStoryboard mainStoryboard = UIStoryboard.FromName("Main", null);
            OutputViewController outputViewController = (OutputViewController)mainStoryboard.InstantiateViewController("OutputViewController");
            outputViewController.scriptName = name;
            outputViewController.store = store;
            outputViewController.runner = runner;
            return outputViewController;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            runner.ErrorFunc = Error;
            runner.OutputFunc = Print;

            evalTimer = NSTimer.CreateScheduledTimer(0.1, EvalScriptTimer);
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, OnDone);
        }

        void OnDone(object sender, EventArgs e)
        {
            DismissViewController(true, null);
        }


        void EvalScriptTimer(NSTimer obj)
        {
            runner.DoFile(scriptName);
        }

        void Error(string error)
        {
            outputView.TextColor = UIColor.Red;
            OutputStringRaw(error);
        }

        void Print(object output, params object [] extra)
        {
            outputView.TextColor = UIColor.DarkTextColor;
            OutputStringRaw(output.ToString(), extra);
        }

        void OutputStringRaw(string output, params object [] extra)
        {
            var builder = new StringBuilder();
            builder.Append(outputView.Text);
            builder.Append(output);
            for (int i = 0; i < extra.Length; i++)
            {
                builder.Append(' ');
                builder.Append(extra[i]);
            }
            builder.Append('\n');

            outputView.Text = builder.ToString();
        }


    }
}