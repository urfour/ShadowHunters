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
            Font = Resources.Load<Font>("Fonts/" + SettingManager.Settings.UI_TextBaseFont.Value);
            SettingManager.Settings.UI_TextBaseFont.AddListener(
                (sender) =>
                {
                    this.Font = Resources.Load<Font>("Fonts/" + SettingManager.Settings.UI_TextBaseFont.Value);
                    this.TryNotify();
                });
            this.TryNotify();
        }
    }
}
