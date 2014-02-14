--
-- ui.lua : UI methods helpers 
--
-- Authors:
--	Vinicius Jarina (vinicius.jarina@xamarin.com)
--
-- Copyright 2013-2014 Xamarin Inc.
-- 
-- Licensed under MIT License
--

local ui = {}

function ui.MessageBox (message)
	local view = UIAlertView();
	view:AddButton ("OK");
	view.Message = message;
	view:Show ();
end

return ui;