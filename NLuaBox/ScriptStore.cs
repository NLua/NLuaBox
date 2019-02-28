using System;
using System.IO;
using System.Linq;

namespace NLuaBox
{
    public class ScriptStore
    {
        string path;

        public ScriptStore(string basePath)
        {
            if (string.IsNullOrEmpty(basePath))
                throw new ArgumentException(nameof(basePath));

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            path = basePath;
        }

        public string[] Scripts => Directory.GetFiles(path);

        public string GetFilePath(string name) => Path.Combine(path, name);

        public void WriteContent (string name, string content)
        {
            File.WriteAllText(GetFilePath(name), content);
        }

        public string ReadContent(string name)
        {
            return File.ReadAllText(GetFilePath(name));
        }

        public string ReadFirstLine(string name)
        {
            string filePath = GetFilePath(name);
            string line1 = File.ReadLines(filePath).FirstOrDefault();
            if (string.IsNullOrEmpty(line1))
                return " -- " + name;
            return line1;
        }

        public bool Exists(string name)
        {
            return File.Exists(GetFilePath(name));
        }

        public void RemoveFile(string name)
        {
            File.Delete(GetFilePath(name));
        }

        public void RenameFile(string oldName, string newName, string existingFileName)
        {
            string oldPath = GetFilePath(oldName);
            string newPath = GetFilePath(newName);
            string existingPath = GetFilePath(existingFileName);

            // Case renaming Foo -> foo, maybe is a bug on File.Move
            if (newName.Equals(oldName, StringComparison.OrdinalIgnoreCase))
            {
                string tempOld = Path.GetTempFileName();
                if (File.Exists(tempOld))
                    File.Delete(tempOld);
                File.Move(oldPath, tempOld);
                File.Move(tempOld, newPath);
                return;
            }

            if (File.Exists(existingPath))
                File.Delete(existingPath);

            File.Move(oldPath, newPath);
        }

        public static string FixName(string name)
        {
            name = name.Replace('?', '_');
            name = name.Replace(':', '_');
            name = name.Replace('/', '_');
            name = name.Replace('\\', '_');
            name = name.Replace('>', '_');
            name = name.Replace('<', '_');
            name = name.Replace('|', '_');

            if (name.EndsWith(".lua", StringComparison.OrdinalIgnoreCase))
                return name;
            return name + ".lua";
        }
    }
}
