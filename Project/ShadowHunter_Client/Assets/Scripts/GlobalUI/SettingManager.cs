using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kernel.Settings
{
    public partial class SettingManager
    {
        public Setting<Color> UI_TitleColor { get; set; } = new Setting<Color>(new Color(229f/255, 161f/255, 86f/255));
        public Setting<Color> UI_TitleTextColor { get; set; } = new Setting<Color>(Color.black);
        public Setting<double> UI_TitleTextEmMult { get; set; } = new Setting<double>(1);

        public Setting<Color> UI_SubTitle_Color { get; set; } = new Setting<Color>(new Color(147f/255,136f/255,136f/255));
        public Setting<Color> UI_SubTitle_TextColor { get; set; } = new Setting<Color>(Color.black);
        public Setting<double> UI_SubTitle_TextEmMult { get; set; } = new Setting<double>(1);

        public Setting<Color> UI_Corps_Color { get; set; } = new Setting<Color>(new Color(210f/255,210f/255,210f/255));
        public Setting<Color> UI_Corps_TextColor { get; set; } = new Setting<Color>(Color.black);
        public Setting<double> UI_Corps_TextEmMult { get; set; } = new Setting<double>(1);

        public Setting<Color> UI_BackGround_Color { get; set; } = new Setting<Color>(new Color(0f / 255, 0f / 255, 0f / 255, 0.5f));

        public Setting<double> UI_TextBaseEm { get; set; } = new Setting<double>(14);
        public Setting<string> UI_TextBaseFont { get; set; } = new Setting<string>("Consola");

        public Setting<string> UI_Lang { get; set; } = new Setting<string>("FR-fr");

        public Setting<double> UI_MusicVolume { get; set; } = new Setting<double>(0.5);
        public Setting<double> UI_EffectVolume { get; set; } = new Setting<double>(0.5);

        public static Setting<string[]> UI_Settings { get; set; } = new Setting<string[]>(
            new string[]
            {
                // split (';') : "accessibility" ; "category path" ; "setting parametre" ; "prefab path" ; "send to prefab"
                //"normal;settings.display/settings.language;UI_Lang;select_language;null",
                //"advanced;settings.display/settings.title.color;UI_TitleColor;select_color;null",
                "normal;settings.general/settings.general.language;UI_Lang;Prefabs/UI/Settings/LangSelector;",

                "normal;settings.audio/settings.audio.music;UI_MusicVolume;Prefabs/UI/Settings/SliderDouble;max=1&min=0&displayed_multiplier=100&nb_displayed_decimals=3&displayed_cast=int",
                "normal;settings.audio/settings.audio.music;UI_MusicVolume;Prefabs/UI/Settings/SliderDouble;max=1&min=0&displayed_multiplier=100&nb_displayed_decimals=3&displayed_cast=int",
                "normal;settings.audio/settings.audio.effect;UI_EffectVolume;Prefabs/UI/Settings/SliderDouble;max=1&min=0&displayed_multiplier=100&nb_displayed_decimals=3&displayed_cast=int",

                "normal;settings.display/settings.display.em;UI_TextBaseEm;Prefabs/UI/Settings/SliderDouble;max=25&min=10&displayed_cast=int&nb_displayed_decimals=5",

                // title theme
                "advanced;settings.display/settings.display.title_theme;;Prefabs/UI/Settings/Separator;",
                "advanced;settings.display/settings.display.title_color;UI_TitleColor;Prefabs/UI/Settings/ColorSelector;",
                "advanced;settings.display/settings.display.title_text_color;UI_TitleTextColor;Prefabs/UI/Settings/ColorSelector;",
                "advanced;settings.display/settings.display.title_em_multiplier;UI_TitleTextEmMult;Prefabs/UI/Settings/SliderDouble;max=3&min=0,5&nb_displayed_decimals=2",

                // subtitle theme
                "advanced;settings.display/settings.display.subtitle_theme;;Prefabs/UI/Settings/Separator;",
                "advanced;settings.display/settings.display.subtitle_color;UI_SubTitle_Color;Prefabs/UI/Settings/ColorSelector;",
                "advanced;settings.display/settings.display.subtitle_text_color;UI_SubTitle_TextColor;Prefabs/UI/Settings/ColorSelector;",
                "advanced;settings.display/settings.display.subtitle_em_multiplier;UI_SubTitle_TextEmMult;Prefabs/UI/Settings/SliderDouble;max=3&min=0,5&nb_displayed_decimals=2",

                // corps theme
                "advanced;settings.display/settings.display.corps_theme;;Prefabs/UI/Settings/Separator;",
                "advanced;settings.display/settings.display.corps_color;UI_Corps_Color;Prefabs/UI/Settings/ColorSelector;",
                "advanced;settings.display/settings.display.corps_text_color;UI_Corps_TextColor;Prefabs/UI/Settings/ColorSelector;",
                "advanced;settings.display/settings.display.corps_em_multiplier;UI_Corps_TextEmMult;Prefabs/UI/Settings/SliderDouble;max=3&min=0,5&nb_displayed_decimals=2",
            }
        );

        public Setting<string> Lang { get; set; } = new Setting<string>("FR-fr");
    }
}
