
using Foundation;
using System.Linq;
using System.IO;

namespace NLuaBox
{
    public static class LocalPathPrepare
    {
        static readonly string scripts_diretory = "scripts";

        public static string LocalPath
        {
            get
            {
                string path = GetBasePath();
                EnsureContentOnPath(path);
                return path;
            }
        }

        public static string ScriptsPath => Path.Combine(LocalPath, scripts_diretory);

        static string GetBasePath()
        {
            string applicationDocumentsDirectory = NSSearchPath.GetDirectories
            (NSSearchPathDirectory.DocumentDirectory,
             NSSearchPathDomain.User, true).LastOrDefault();
            return Path.Combine(applicationDocumentsDirectory);
        }

        static void EnsureContentOnPath(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string readDir = NSBundle.MainBundle.ResourcePath;

            string fullPath = Path.Combine(path, scripts_diretory);

            if (Directory.Exists(fullPath))
                return;

            Directory.CreateDirectory(fullPath);
            string fullReadDir = Path.Combine(readDir, scripts_diretory);
            CopyFiles(fullReadDir, fullPath);
        }

        static void CopyFiles(string fromDir, string toDir)
        {
            foreach (string sourceFileName in Directory.GetFiles(fromDir))
            {
                string destFileName = Path.Combine(toDir, Path.GetFileName(sourceFileName));
                File.Copy(sourceFileName, destFileName);
            }
        }
    }
}

