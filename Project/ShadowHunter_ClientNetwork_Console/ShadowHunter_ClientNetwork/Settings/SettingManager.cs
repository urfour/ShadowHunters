using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using IO;

namespace Kernel.Settings
{
    [Serializable]
    public partial class SettingManager
    {
        public static SettingManager Settings { get; private set; }


        public static void Load(string path = "Settings.XML")
        {
            StreamReader file = null;
            try
            {
                path = IOSystem.GetFullPath(path);
                if (File.Exists(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SettingManager));
                    file = new StreamReader(path);
                    Settings = (SettingManager)serializer.Deserialize(file);
                    file.Close();
                }
                else
                {
                    Settings = new SettingManager();
                }
            }
            catch (Exception e)
            {
                if (file != null) file.Close();
                Settings = new SettingManager();
                Logger.Error(e);
            }

        }

        public static void Save(string path = "Settings.XML")
        {
            try
            {
                path = IOSystem.GetFullPath(path);
                XmlSerializer serializer = new XmlSerializer(typeof(SettingManager));
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                FileStream file = File.Create(path);
                serializer.Serialize(file, Settings);
                file.Close();
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}
