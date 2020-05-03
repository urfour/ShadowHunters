using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenuUI.ThemeColor
{
    [RequireComponent(typeof(Image))]
    class IconLoader : MonoBehaviour
    {
        public string path;
        public string replacer;
        public bool useReplacer = false;
        public bool transparentIfNull = false;
        public Text indirectReplacer = null;

        private void Start()
        {
            if (path != null)
            {
                Sprite icon = Resources.Load<Sprite>("Icons/" + path);
                if (icon != null)
                {
                    gameObject.GetComponent<Image>().sprite = icon;
                    if (indirectReplacer != null)
                    {
                        indirectReplacer.text = "";
                    }
                }
                else if (useReplacer)
                {
                    if (indirectReplacer)
                    {
                        if (transparentIfNull)
                        {
                            ColoredUI coloredUI = gameObject.GetComponent<ColoredUI>();
                            if (coloredUI != null)
                            {
                                Destroy(coloredUI);
                            }
                            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                        }
                        indirectReplacer.text = replacer;
                    }
                    else
                    {
                        Destroy(gameObject.GetComponent<Image>());
                        ColoredUI coloredUI = gameObject.GetComponent<ColoredUI>();
                        if (coloredUI != null)
                        {
                            Destroy(coloredUI);
                        }
                        Text t = gameObject.AddComponent<Text>();
                        t.text = replacer;
                        gameObject.AddComponent<ColoredUI>();
                    }
                }
            }
        }
    }
}
