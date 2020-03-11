using Assets.Scripts.MainMenuUI.ThemeColor;
using EventSystem;
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
    PARENT,
    BACKGROUND,
}

enum ElemType
{
    BUTTON,
    TEXT,
    TEXT_ICON,
    PANEL,
    AUTO
}

class ColoredUI : MonoBehaviour
{
    public ElemClass elemClass = ElemClass.PARENT;
    public ElemType elemType = ElemType.AUTO;
    public double emMultiplier = 1;
    private static FontUI font = null;

    private Dictionary<ListenableObject,OnNotification> observed = new Dictionary<ListenableObject, OnNotification>();

    private void Start()
    {
        if (font == null) font = new FontUI();
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
            while (parent != null && ((parentColoredui = parent.GetComponent<ColoredUI>()) == null) || (parent.GetComponent<ColoredUI>().elemClass == ElemClass.PARENT))
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
            if (elemClass == ElemClass.TITLE)
            {
                observed.Add(SettingManager.Settings.UI_TitleTextColor,(sender) => { this.Apply(); });
                observed.Add(SettingManager.Settings.UI_TitleTextEmMult,(sender) => { this.Apply(); });
            }
            else if (elemClass == ElemClass.SUBTITLE)
            {
                observed.Add(SettingManager.Settings.UI_SubTitle_TextColor,(sender) => { this.Apply(); });
                observed.Add(SettingManager.Settings.UI_SubTitle_TextEmMult,(sender) => { this.Apply(); });
            }
            else if (elemClass == ElemClass.CORPS)
            {
                observed.Add(SettingManager.Settings.UI_Corps_TextColor,(sender) => { this.Apply(); });
                observed.Add(SettingManager.Settings.UI_Corps_TextEmMult,(sender) => { this.Apply(); });
            }
            observed.Add(SettingManager.Settings.UI_TextBaseEm,(sender) => { this.Apply(); });
            observed.Add(font,(sender) => { gameObject.GetComponent<Text>().font = font.Font; });
            gameObject.GetComponent<Text>().font = font.Font;
        }
        else if (elemType == ElemType.TEXT_ICON)
        {
            if (elemClass == ElemClass.TITLE) observed.Add(SettingManager.Settings.UI_TitleTextColor,(sender) => { this.Apply(); });
            else if (elemClass == ElemClass.SUBTITLE) observed.Add(SettingManager.Settings.UI_SubTitle_TextColor,(sender) => { this.Apply(); });
            else if (elemClass == ElemClass.CORPS) observed.Add(SettingManager.Settings.UI_Corps_TextColor,(sender) => { this.Apply(); });
        }
        else
        {
            if (elemClass == ElemClass.TITLE) observed.Add(SettingManager.Settings.UI_TitleColor,(sender) => { this.Apply(); });
            else if (elemClass == ElemClass.SUBTITLE) observed.Add(SettingManager.Settings.UI_SubTitle_Color,(sender) => { this.Apply(); });
            else if (elemClass == ElemClass.CORPS) observed.Add(SettingManager.Settings.UI_Corps_Color,(sender) => { this.Apply(); });
            else if (elemClass == ElemClass.BACKGROUND) observed.Add(SettingManager.Settings.UI_BackGround_Color,(sender) => { this.Apply(); });
        }
        if (this.isActiveAndEnabled)
        {
            OnEnable();
        }
    }


    public void Apply()
    {
        Color c;
        double em = 0;
        if (elemType == ElemType.TEXT)
        {
            if (elemClass == ElemClass.TITLE)
            {
                c = SettingManager.Settings.UI_TitleTextColor.Value;
                em = SettingManager.Settings.UI_TitleTextEmMult.Value;
            }
            else if (elemClass == ElemClass.SUBTITLE)
            {
                c = SettingManager.Settings.UI_SubTitle_TextColor.Value;
                em = SettingManager.Settings.UI_SubTitle_TextEmMult.Value;
            }
            else if (elemClass == ElemClass.CORPS)
            {
                c = SettingManager.Settings.UI_Corps_TextColor.Value;
                em = SettingManager.Settings.UI_Corps_TextEmMult.Value;
            }
            else c = Color.cyan;
            em *= SettingManager.Settings.UI_TextBaseEm.Value;
        }
        else if (elemType == ElemType.TEXT_ICON)
        {
            if (elemClass == ElemClass.TITLE) c = SettingManager.Settings.UI_TitleTextColor.Value;
            else if (elemClass == ElemClass.SUBTITLE) c = SettingManager.Settings.UI_SubTitle_TextColor.Value;
            else if (elemClass == ElemClass.CORPS) c = SettingManager.Settings.UI_Corps_TextColor.Value;
            else c = Color.cyan;
        }
        else
        {
            if (elemClass == ElemClass.TITLE) c = SettingManager.Settings.UI_TitleColor.Value;
            else if (elemClass == ElemClass.SUBTITLE) c = SettingManager.Settings.UI_SubTitle_Color.Value;
            else if (elemClass == ElemClass.CORPS) c = SettingManager.Settings.UI_Corps_Color.Value;
            else if (elemClass == ElemClass.BACKGROUND) c = SettingManager.Settings.UI_BackGround_Color.Value;
            else c = Color.cyan;
        }
        em *= emMultiplier;
        switch (elemType)
        {
            case ElemType.TEXT:
                {
                    Text t = this.GetComponent<Text>();
                    t.color = c;
                    t.horizontalOverflow = HorizontalWrapMode.Overflow;
                    t.verticalOverflow = VerticalWrapMode.Overflow;
                    t.fontSize = (int)em;
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
            case ElemType.TEXT_ICON:
                {
                    Image p = this.GetComponent<Image>();
                    p.color = c;
                }
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        foreach(KeyValuePair<ListenableObject,OnNotification> pair in observed)
        {
            pair.Key.RemoveListener(pair.Value);
        }
    }

    private void OnEnable()
    {
        foreach(var pair in observed)
        {
            pair.Key.AddListener(pair.Value);
            //pair.Value(pair.Key);
        }
        Apply();
    }
}
