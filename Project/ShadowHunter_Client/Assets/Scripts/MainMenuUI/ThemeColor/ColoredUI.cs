using Kernel.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

enum ElemClass
{
    TITLE,
    SUBTITLE,
    CORPS,
    PARENT
}

enum ElemType
{
    BUTTON,
    TEXT,
    PANEL,
    AUTO
}

class ColoredUI : MonoBehaviour
{
    public ElemClass elemClass = ElemClass.PARENT;
    public ElemType elemType = ElemType.AUTO;


    private void Start()
    {
        if (this.elemType == ElemType.AUTO)
        {
            if (gameObject.GetComponent<Text>() != null) this.elemType = ElemType.TEXT;
            else if (gameObject.GetComponent<Button>() != null) this.elemType = ElemType.BUTTON;
            else if (gameObject.GetComponent<Image>() != null) this.elemType = ElemType.PANEL;
        }

        if (this.elemClass == ElemClass.PARENT)
        {
            Transform parent = transform.parent;
            ColoredUI parentColoredui = null;
            while (parent != null && (parentColoredui = parent.GetComponent<ColoredUI>()) == null)
            {
                parent = parent.parent;
            }

            if (parentColoredui != null)
            {
                this.elemClass = parentColoredui.elemClass;
            }
            else
            {
                Debug.LogWarning("No ColoredUI in parent");
            }
        }
        Apply();
        if (elemType == ElemType.TEXT)
        {
            if (elemClass == ElemClass.TITLE) SettingManager.Settings.Display_TitleTextColor_Advance.AddListener((sender) => { this.Apply(); });
            else if (elemClass == ElemClass.SUBTITLE) SettingManager.Settings.Display_SubTitleTextColor_Advance.AddListener((sender) => { this.Apply(); });
            else if (elemClass == ElemClass.CORPS) SettingManager.Settings.Display_CorpsTextColor_Advance.AddListener((sender) => { this.Apply(); });
        }
        else
        {
            if (elemClass == ElemClass.TITLE) SettingManager.Settings.Display_TitleColor_Advance.AddListener((sender) => { this.Apply(); });
            else if (elemClass == ElemClass.SUBTITLE) SettingManager.Settings.Display_SubTitleColor_Advance.AddListener((sender) => { this.Apply(); });
            else if (elemClass == ElemClass.CORPS) SettingManager.Settings.Display_CorpsColor_Advance.AddListener((sender) => { this.Apply(); });
        }
    }


    public void Apply()
    {
        Color c;
        if (elemType == ElemType.TEXT)
        {
            if (elemClass == ElemClass.TITLE) c = SettingManager.Settings.Display_TitleTextColor_Advance.Value;
            else if (elemClass == ElemClass.SUBTITLE) c = SettingManager.Settings.Display_SubTitleTextColor_Advance.Value;
            else if (elemClass == ElemClass.CORPS) c = SettingManager.Settings.Display_CorpsTextColor_Advance.Value;
            else c = Color.cyan;
        }
        else
        {
            if (elemClass == ElemClass.TITLE) c = SettingManager.Settings.Display_TitleColor_Advance.Value;
            else if (elemClass == ElemClass.SUBTITLE) c = SettingManager.Settings.Display_SubTitleColor_Advance.Value;
            else if (elemClass == ElemClass.CORPS) c = SettingManager.Settings.Display_CorpsColor_Advance.Value;
            else c = Color.cyan;
        }
        switch (elemType)
        {
            case ElemType.TEXT:
                {
                    Text t = this.GetComponent<Text>();
                    t.color = c;
                    t.horizontalOverflow = HorizontalWrapMode.Overflow;
                    t.verticalOverflow = VerticalWrapMode.Overflow;
                }
                break;
            case ElemType.BUTTON:
                {
                    Button bt = this.GetComponent<Button>();
                    bt.targetGraphic.color = Color.white;
                    var colors = bt.colors;
                    colors.highlightedColor = c;
                    colors.selectedColor = c;
                    colors.normalColor = c;
                    colors.pressedColor = new Color(c.r * 0.8f, c.g * 0.8f, c.b * 0.8f, c.a);
                    bt.colors = colors;
                }
                break;
            case ElemType.PANEL:
                {
                    Image p = this.GetComponent<Image>();
                    p.color = c;
                }
                break;
            default:
                break;
        }
    }
}
