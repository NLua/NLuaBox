using System;
using MonoTouch.Foundation;
using System.Linq;
using System.IO;

namespace NLuaBox
{
	public static class LocalPathPrepare
	{
        enum DirectoryType
        {
            Scripts,
            Source,
            Utils,
        }
		static readonly  string [] dirs = { "scripts", "source", "utils" };

		public static string LocalPath {
			get {

				string path = GetBasePath ();

				EnsureContentOnPath (path);

				return path;
			}
		}

        public static string SourcePath {
            get {
                return Path.Combine(LocalPath, dirs[(int)DirectoryType.Source]);
            }
        }

        public static string UtilsPath {
            get {
                return Path.Combine(LocalPath, dirs[(int)DirectoryType.Utils]);
            }
        }

        public static string ScriptsPath {
            get {
                return Path.Combine(LocalPath, dirs[(int)DirectoryType.Scripts]);
            }
        }

		static string GetBasePath ()
		{
			string applicationDocumentsDirectory = NSSearchPath.GetDirectories (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User, true).LastOrDefault ();

			return Path.Combine (applicationDocumentsDirectory);
		}

		static void EnsureContentOnPath (string path)
		{
			if (!Directory.Exists (path))
				Directory.CreateDirectory (path);

			string readDir = NSBundle.MainBundle.ResourcePath;

			foreach (string dir in dirs) {

				string fullPath = Path.Combine (path, dir);
				if (!Directory.Exists (fullPath)) {
					Directory.CreateDirectory (fullPath);
					string fullReadDir = Path.Combine (readDir, dir);
					CopyFiles (fullReadDir, fullPath);
				}
			}			
		}

		static void CopyFiles (string fromDir, string toDir)
		{
			foreach (string sourceFileName in Directory.GetFiles (fromDir)) {
				string destFileName = Path.Combine (toDir, Path.GetFileName (sourceFileName));
				File.Copy (sourceFileName, destFileName);
			}
		}
	}
}

