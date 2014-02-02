import ('System', 'System.IO')

local ScriptStore = {}

ScriptStore.mt = {__call = function(self, ...)
													return self.new (...)
											end
								  }
setmetatable (ScriptStore,ScriptStore.mt)

function ScriptStore.new (basePath, source)
	
	ScriptStore.m = {}; -- create a table members to store Lua fields
	
	if (String.IsNullOrEmpty (basePath)) then
		error ("Base path must be not empty");
	end
	
	if (String.IsNullOrEmpty (source)) then
		error ("Source path must be not empty");
	end

	if (not Directory.Exists (basePath)) then
			Directory.CreateDirectory (basePath);
	end

	ScriptStore.m.path = basePath;
	ScriptStore.m.sourcePath = source;
	
	return ScriptStore;
end

function ScriptStore:GetScripts()
	return Directory.GetFiles (self.m.path);
end

function ScriptStore:GetSources()
	return Directory.GetFiles (self.m.sourcePath);
end

function ScriptStore:SaveSourceContent (name, content)
			local filePath = Path.Combine (sourcePath, name);
			File.WriteAllText (filePath, content);
end

function ScriptStore:SaveScriptContent (name, content)

			local filePath = Path.Combine (self.m.path, name);
			File.WriteAllText (filePath, content);
end

function ScriptStore:Exists (name)
			local filePath = Path.Combine (self.m.path, name);
			return File.Exists (filePath);
end

function ScriptStore:RemoveFile (name)
			local filePath = Path.Combine (self.m.path, name);
			File.Delete (filePath);
end

function ScriptStore:RenameFile (oldName, newName)
			local path = self.m.path;
			local oldPath = Path.Combine (path, oldName);
			local newPath = Path.Combine (path, newName);
			if (File.Exists (newPath)) then
				File.Delete (newPath);
			end
			File.Move (oldPath, newPath);
end

function ScriptStore:GetScriptContent (name)
			local filePath = Path.Combine (self.m.path, name);
			return File.ReadAllText (filePath);
end

function ScriptStore:GetSourceContent (name)
			local filePath = Path.Combine (sourcePath, name);
			return File.ReadAllText (filePath);
end

return ScriptStore;
