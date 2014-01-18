
local ui = {}

function ui.MessageBox (message)
	local view = UIAlertView();
	view:AddButton ("OK");
	view.Message = message;
	view:Show ();
end

return ui;