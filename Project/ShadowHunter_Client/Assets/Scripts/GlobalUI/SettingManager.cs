using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kernel.Settings
{
    public partial class SettingManager
    {
        //public Dictionary<string, Setting<Color>> Display_ColorTheme_Advance { get; set; } = new Dictionary<string, Setting<Color>>();
        public Setting<Color> Display_TitleColor_Advance { get; set; } = new Setting<Color>(new Color(229f/255, 161f/255, 86f/255));
        public Setting<Color> Display_TitleTextColor_Advance { get; set; } = new Setting<Color>(Color.black);
        public Setting<double> Display_TitleTextEmMult_Advance { get; set; } = new Setting<double>(1);

        public Setting<Color> Display_SubTitleColor_Advance { get; set; } = new Setting<Color>(new Color(116f/255,163f/255,166f/255));
        public Setting<Color> Display_SubTitleTextColor_Advance { get; set; } = new Setting<Color>(Color.black);
        public Setting<double> Display_SubTitleTextEmMult_Advance { get; set; } = new Setting<double>(1);

        public Setting<Color> Display_CorpsColor_Advance { get; set; } = new Setting<Color>(new Color(219f/255,238f/255,238f/255));
        public Setting<Color> Display_CorpsTextColor_Advance { get; set; } = new Setting<Color>(Color.black);
        public Setting<double> Display_CorpsTextEmMult_Advance { get; set; } = new Setting<double>(1);

        public Setting<double> Display_TextBaseEm_Advance { get; set; } = new Setting<double>(14);
        public Setting<string> Display_TextBaseFont_Advance { get; set; } = new Setting<string>("Consola");

        public Setting<string> General_Lang_User { get; set; } = new Setting<string>("FR-fr");
    }
}
