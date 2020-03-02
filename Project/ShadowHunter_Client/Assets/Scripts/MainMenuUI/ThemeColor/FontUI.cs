using EventSystem;
using Kernel.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MainMenuUI.ThemeColor
{
    class FontUI : ListenableObject
    {
        public Font Font { get; set; }

        public FontUI()
        {
            Font = Resources.Load<Font>("Fonts/" + SettingManager.Settings.Display_TextBaseFont_Advance.Value);
            SettingManager.Settings.Display_TextBaseFont_Advance.AddListener(
                (sender) =>
                {
                    this.Font = Resources.Load<Font>("Fonts/" + SettingManager.Settings.Display_TextBaseFont_Advance.Value);
                    this.TryNotify();
                });
            this.TryNotify();
        }
    }
}
