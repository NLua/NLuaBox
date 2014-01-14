

import ('System')
import ('System','System.Drawing')
import ('monotouch', 'MonoTouch.Foundation')
import ('monotouch', 'MonoTouch.UIKit') 
import ('Chormatism', 'Chormatism') 
import ('NLuaBox') 

NLuaBoxAppDelegate = require ('NLuaBoxAppDelegate')

function Init (AppDelegate)
	local nluaBoxAppDelegate = NLuaBoxAppDelegate(AppDelegate)
	return nluaBoxAppDelegate:FinishLaunching()
end