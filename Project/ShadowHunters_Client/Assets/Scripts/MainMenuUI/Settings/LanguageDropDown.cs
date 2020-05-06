using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Lang;
using Kernel.Settings;

//[RequireComponent(typeof(Dropdown))]
public class LanguageDropDown : SettingPrefab
{
    public Dropdown dp;
    // Use this for initialization
    Dictionary<string, (Language lang,string path)> languages = new Dictionary<string, (Language lang, string path)>();


    Setting<string> target;

    public override void Configurate(string label, SettingItem target, string config = null)
    {
        base.Configurate(label, target, config);
        this.target = (Setting<string>)target;
    }

    void Start()
    {
        OnEnable();
    }

    public void OnEnable()
    {
        languages.Clear();
        //dp = gameObject.GetComponent<Dropdown>();
        (Language lang,string path)[] langs = Language.GetAllLanguages();
        List<string> optiondata = new List<string>();
        int currentindex = 0;
        for (int i = 0; i < langs.Length; i++)
        {
            if (langs[i].lang.GetText("lang").Equals(Language.Translate("lang")))
            {
                currentindex = i;
            }
            if (!languages.ContainsKey(langs[i].lang.GetText("lang")))
            {
                languages.Add(langs[i].lang.GetText("lang"), langs[i]);
                optiondata.Add(langs[i].lang.GetText("lang"));
            }
            else
            {
                languages.Add(langs[i].path, langs[i]);
                optiondata.Add(langs[i].path);
            }
        }
        dp.ClearOptions();
        dp.AddOptions(optiondata);
        selfChanged = true;
        dp.value = currentindex;
        selfChanged = false;
        Language.AddListener((sender) =>
        {
            dp.value = dp.options.FindIndex((item) => { return item.text == Language.Translate("lang"); });
        });
    }

    public void OnSelect(Int32 index)
    {
        if (selfChanged) return;
        //Debug.Log(dp.value + " : " + dp.options[dp.value].text);
        //new Language.Language(languages[dp.options[dp.value].text]);
        //Language.Instance = languages[dp.options[dp.value].text];
        selfChanged = true;
        target.Value = languages[dp.options[dp.value].text].path;
        selfChanged = false;
    }

    bool selfChanged = false;
    public override void Refresh()
    {
        if (!selfChanged) return;
        base.Refresh();
    }
}
