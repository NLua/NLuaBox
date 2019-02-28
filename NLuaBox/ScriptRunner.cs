using System;
using NLua;

namespace NLuaBox
{
    public delegate void OutputDelegate(string s, params object[] args);

    public class ScriptRunner
    {
        OutputDelegate printFunction;
        Lua state;
        public ScriptStore Store { get; set; }
        public Action<string> ErrorFunc { get; set; }
        public OutputDelegate OutputFunc
        {
            get => printFunction;
            set
            {
                printFunction = value;
                if (printFunction == null)
                    return;
                state["print"] = printFunction;
                state.DoString("io.write = print");
            }
        }

        public ScriptRunner(ScriptStore store)
        {
            Store = store;
            state = new Lua();
        }

        public void DoFile (string name)
        {
            string fileName = Store.GetFilePath(name);
            object[] result;
            try
            {
                result =  state.DoFile(fileName);
            }
            catch (Exception e)
            {
                string error = e.Message;
                if (e.InnerException != null)
                    error += " InnerException: " + e.InnerException.Message;

                ErrorFunc?.Invoke(error);
            }
        }



    }
}
