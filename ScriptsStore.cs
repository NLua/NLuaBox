using System;
using System.IO;
using System.Collections.Generic;

namespace NLuaBox
{
	public class ScriptsStore
	{
		string path;
		string sourcePath;

		public ScriptsStore (string basePath, string source)
		{
			if (string.IsNullOrEmpty (basePath))
				throw new ArgumentException ("Base path must be not empty");
			if (string.IsNullOrEmpty (source))
				throw new ArgumentException ("Source path must be not empty");
			if (!Directory.Exists (basePath))
				Directory.CreateDirectory (basePath);

			path = basePath;
			sourcePath = source;
		}

		public string[] Scripts
		{
			get {
				return Directory.GetFiles (path);
			}
		}

		public string[] Sources
		{
			get {
				return Directory.GetFiles (sourcePath);
			}
		}

		public void SaveSourceContent (string name, string content)
		{
			string filePath = Path.Combine (sourcePath, name);
			File.WriteAllText (filePath, content);
		}

		public void SaveScriptContent (string name, string content)
		{
			string filePath = Path.Combine (path, name);
			File.WriteAllText (filePath, content);
		}

		public bool Exists (string name)
		{
			string filePath = Path.Combine (path, name);
			return File.Exists (filePath);
		}

		public void RemoveFile (string name)
		{
			string filePath = Path.Combine (path, name);
			File.Delete (filePath);
		}

		public void RenameFile (string oldName, string newName)
		{
			string oldPath = Path.Combine (path, oldName);
			string newPath = Path.Combine (path, newName);
			if (File.Exists (newPath))
				File.Delete (newPath);
			File.Move (oldPath, newPath);
		}

		public string GetScriptContent (string name)
		{
			string filePath = Path.Combine (path, name);
			return File.ReadAllText (filePath);
		}

		public string GetSourceContent (string name)
		{
			string filePath = Path.Combine (sourcePath, name);
			return File.ReadAllText (filePath);
		}
	}
}

