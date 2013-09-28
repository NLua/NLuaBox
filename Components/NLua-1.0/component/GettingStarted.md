For script eval, you can use DoString or DoFile

```csharp
using NLua;
...

[Preserve (AllMembers = true)]
public class Scriptable 
{
     public Scriptable (string param)
     { // ... 
     }

     public void DoSomething ()
     {
         Console.WriteLine ("Do Something");
     }
}

void ExecuteScript ()
{
    string luaFile = "script.lua";

    var lua = new Lua ();
    double val = (double)lua.DoString ("return math.sin (10)");
    lua.DoFile (luaFile);
}
```
If you want to use your class from Lua you need to use [Preserve] to preserve your class, NLua will call the methods using Reflection. [More info](http://docs.xamarin.com/guides/ios/advanced_topics/linker)

```lua

local s = Scriptable ("My String Parameter")
s:DoSomething ()

```