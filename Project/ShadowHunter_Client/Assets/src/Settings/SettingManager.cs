﻿using System;
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
    /// <summary>
    /// Conventions de nommage des settings : 
    ///     {CatégorieName}_{SettingName}_{accéssibilité : Any/Dev/Advance/User}
    ///     exemple : General_Lang_User
    /// </summary>
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
                file.Close();
            }
            else
            {
                Settings = new SettingManager();
            }
        }

        public static void Save(string path)
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
