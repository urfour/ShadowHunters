using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IO
{
    class IOSystem : MonoBehaviour
    {
        public static string DataFolder = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ShadowHunter"));
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnBeforeSceneLoadRuntimeMethod()
        {
           
        }

        public static bool FileExists(string path)
        {
            return File.Exists(Path.GetFullPath(Path.Combine(DataFolder, path)));
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(Path.GetFullPath(Path.Combine(DataFolder, path)));
        }

        public static void CreateFile(string path, string[] allLines = null, bool deletePrevious = false)
        {
            path = Path.GetFullPath(Path.Combine(DataFolder, path));
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            if (deletePrevious)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            FileStream fs = File.Create(path);
            if (allLines != null)
            {
                for (int i = 0; i < allLines.Length; i++)
                {
                    byte[] b = Encoding.Unicode.GetBytes(allLines[i] + "\n");
                    fs.Write(b, 0, b.Length);
                }
            }
            fs.Close();
        }

        public static FileStream CreateFile(string path, bool deletePrevious = false)
        {
            path = Path.GetFullPath(Path.Combine(DataFolder, path));
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            if (deletePrevious)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            FileStream fs = File.Create(path);
            return fs;
        }

        public static void DeleteFile(string path)
        {
            path = Path.GetFullPath(Path.Combine(DataFolder, path));
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static string GetFullPath(string path)
        {
            return Path.GetFullPath(Path.Combine(DataFolder, path));
        }

        public static string[] GetAllFileNames(string directory, bool with_extension = false)
        {
            string[] s = Directory.GetFiles(GetFullPath(directory));
            for (int i = 0; i < s.Length; i++)
            {
                if (with_extension)
                {
                    s[i] = Path.GetFileName(s[i]);
                }
                else
                {
                    s[i] = Path.GetFileNameWithoutExtension(s[i]);
                }
            }
            return s;
        }

        public static string[] GetAllLines(string path)
        {
            path = GetFullPath(path);
            if (File.Exists(path))
            {
                return File.ReadAllLines(path, Encoding.Unicode);
            }
            return null;
        }

        private void Start()
        {
            Debug.Log(DataFolder);
        }
    }
}
