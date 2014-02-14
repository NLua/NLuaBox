--
-- string_extra.lua : Add extra implementations to Lua string (StartsWith, EndsWith)
--
-- Authors:
--	Vinicius Jarina (vinicius.jarina@xamarin.com)
--
-- Copyright 2013-2014 Xamarin Inc.
-- 
-- Licensed under MIT License
--
function string.StartsWith(String,Start)
   return string.sub(String,1,string.len(Start))==Start
end

function string.EndsWith(String,End)
   return End=='' or string.sub(String,-string.len(End))==End
end
