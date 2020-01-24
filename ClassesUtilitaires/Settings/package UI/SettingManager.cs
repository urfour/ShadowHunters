using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Kernel.Settings
{
    public partial class SettingManager
    {
        public Setting<KeyCode> UI_SettingName1 { get; set; } = new Setting<KeyCode>(KeyCode.LeftControl);
        public Setting<KeyCode> UI_SettingName2 { get; set; } = new Setting<KeyCode>(KeyCode.G);
        public Setting<KeyCode> UI_SettingName3 { get; set; } = new Setting<KeyCode>(KeyCode.U);
        public Setting<KeyCode> UI_SettingName4 { get; set; } = new Setting<KeyCode>(KeyCode.B);

        public static void UnityDebugPrint()
        {
            Type t = typeof(SettingManager);
            TypeAttributes ta = t.Attributes;

            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (pi.PropertyType.IsSubclassOf(typeof(SettingItem)))
                {
                    SettingItem s = (SettingItem)pi.GetValue(Settings);
                    Debug.Log(pi.Name + " : " + s.ItemValue);
                }
            }
        }
    }
}
