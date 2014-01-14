`NLua` is the bind between Lua and .NET. It supports iOS,
Android and Windows Phone 7/8.

## Features

 - Easily eval expressions.
 - Call Lua from C#.
 - Call C# from Lua. 
 - Use C# Objects, Properties, Methods from any assembly in your scripts.

## Examples

### Adding a `NLua` to your iOS app:


```csharp
    using NLua;
    ...

    // Store Lua context 
    Lua context = new Lua ();

    public override void ViewDidLoad ()
    {
      base.ViewDidLoad ();  
      context.LoadCLRPackage (); // Enable call methods using reflection

      ...

    }
```

### Adding a `NLua` to your Android app:

```csharp
    using NLua;
    ...

    Lua context = new Lua ();

    protected override void OnCreate (Bundle bundle)
    {
      base.OnCreate (bundle);
      context.LoadCLRPackage (); // Enable call methods using reflection
      
    }
```

### Evaluating expressions

You can easily eval any expresion.

```csharp

    double val = (double)context.DoString ("return math.sin (10)*10 + 7") [0];

```  

### Calling Static Methods

You can call any static method using ".".

```csharp

    public MyClass 
    {
        public static void Func (int val, string val2)
        {
            ...
        }
    }

    ...

    context.DoString ("MyClass.Func(10,'string 3')");
``` 

### Send/Retrieve value between Lua/C\# 

You can set or get values using [] operator.

```csharp

    // Getting global value from Lua
    context.DoString ("global_x = 10 + 10 + 2*3");
    var x = context ["global_x"];

    // Send value to Lua
    context ["val_x"] = 10;

``` 

### Calling any method, using BCL from Lua 

You can call any public API from Lua.
Using ":" to instance methods, "." to Properties.

```csharp
    
    public void MyMethod (int val)
    {
        ...
    }

    public int MyProperty { get; private set; }


    // Send this to Lua
    context ["instance"] = this;
    context.DoString("  Console.WriteLine ('Calling CWL from Lua') " + 
                     "  instance:MyMethod (10) " +      // calling MyMethod from Lua
                     "  local p = instance.MyProperty " // calling MyProperty from Lua

``` 

If you want to use your class from Lua you need to use [Preserve] to preserve your class, NLua will call the methods using Reflection. [More info](http://docs.xamarin.com/guides/ios/advanced_topics/linker)


