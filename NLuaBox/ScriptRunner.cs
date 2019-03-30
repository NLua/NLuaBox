using System;
using NLua;

namespace NLuaBox
{
    public delegate void PrintDelegate(string s, params object[] args);

    public class ScriptRunner
    {
        Lua state;
        public ScriptStore Store { get; set; }
        public Action<string> ErrorFunc { get; set; }
        public PrintDelegate PrintFunc
        {
            get => (PrintDelegate)state["print"];
            set => state["print"] = value;
        }

        public Action<string> WriteFunc
        {
            get => (Action<string>)state["io.write"];
            set => state["io.write"] = value;
        }

        public ScriptRunner(ScriptStore store)
        {
            Store = store;
            state = new Lua();
            state.State.Encoding = System.Text.Encoding.UTF8;
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
