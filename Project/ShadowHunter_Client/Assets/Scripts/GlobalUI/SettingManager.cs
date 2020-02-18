using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kernel.Settings
{
    public partial class SettingManager
    {
        public Dictionary<string, Setting<Color>> ColorTheme { get; set; } = new Dictionary<string, Setting<Color>>();

        public Setting<string> Lang { get; set; } = new Setting<string>("FR-fr");
    }
}
