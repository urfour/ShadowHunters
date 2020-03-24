using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using IO;
using System.IO;

namespace Kernel.Settings
{

    [Serializable]
    public partial class SettingManager
    {
        public static SettingManager Settings { get; private set; }
        
        public static void Load(string path = "Settings.XML")
        {
            path = IOSystem.GetFullPath(path);
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SettingManager));
                StreamReader file = new StreamReader(path);
                Settings = (SettingManager)serializer.Deserialize(file);
                if (Settings == null) Settings = new SettingManager();
                file.Close();
            }
            else
            {
                Settings = new SettingManager();
            }
        }
        
        public static void Save(string path = "Settings.XML")
        {
            path = IOSystem.GetFullPath(path);
            XmlSerializer serializer = new XmlSerializer(typeof(SettingManager));
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            FileStream file = File.Create(path);
            serializer.Serialize(file, Settings);
            file.Close();
        }
    }
}
