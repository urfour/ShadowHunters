using UnityEngine;
using System.Collections;
using Kernel.Settings;
using UnityEngine.UI;
using System;

public class InputBar : SettingPrefab
{
    public enum INPUTTYPE
    {
        TEXT,
        IPADDRESS,
        INT,
        DOUBLE
    }

    public InputField input;
    public Text placeholder;
    private INPUTTYPE type;
    SettingItem target;

    public override void Configurate(string label, SettingItem target, string config = null)
    {
        base.Configurate(label, target, config);
        this.target = target;
        
        if (config != null)
        {
            string[] args = config.Split('&');
            foreach (string arg in args)
            {
                string[] param = arg.Split('=');
                switch (param[0])
                {
                    case "type":
                        {
                            type = (INPUTTYPE)Enum.Parse(typeof(INPUTTYPE), param[1]);
                            
                        }
                        break;
                }
            }
        }
    }
    
    private bool selfChanged = false;
    public void OnValueChange(string s)
    {
        selfChanged = true;
        switch (type)
        {
            case INPUTTYPE.TEXT:
                ((Setting<string>)target).Value = input.text;
                break;
        }
        selfChanged = false;
    }

    public void OnEndEdit(string s)
    {

    }

    public override void Refresh()
    {
        if (!selfChanged) return;
        base.Refresh();
    }
}
