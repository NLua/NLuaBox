`NLua` is the bind between Lua and .NET. It supports iOS,
Android, Windows Phone 7/8.

## Features

 - Easily eval expressions.
 - Call Lua from C#.
 - Call C# from Lua. 
 - Use C# Objects, Properties, Methods from any assembly in your scripts.

For script eval, you can use DoString() or DoFile()


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
        double val = (double)lua.DoString ("return math.sin (10)") [0];
        lua.DoFile (luaFile);
    }
```


```

    -- To create C# object from script, just call ClassName ()
    local s = Scriptable ("My String Parameter")

    -- To call methods use object:Method
    s:DoSomething ()

    -- To call static methods use ClassName.StaticMethod
    Scriptable.StaticMethod ('Param')

```

If you want to use your class from Lua you need to use [Preserve] to preserve your class, NLua will call the methods using Reflection. [More info](http://docs.xamarin.com/guides/ios/advanced_topics/linker)


