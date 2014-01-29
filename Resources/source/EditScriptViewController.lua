

local EditScriptViewController = {}


EditScriptViewController.mt = { __call = function(self, ...)
													return self.new (...)
											end
							  }

setmetatable (EditScriptViewController,EditScriptViewController.mt)

function EditScriptViewController.new (...)

		EditScriptViewController.m = {}; -- create a table members to store Lua fields


		return EditScriptViewController;
end
