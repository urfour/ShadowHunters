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

    public Text input;
    public Text placeholder;
    private INPUTTYPE type;

    public override void Configurate(string label, SettingItem target, string config = null)
    {
        base.Configurate(label, target, config);
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
                            type = (INPUTTYPE)Enum.Parse(typeof(INPUTTYPE), args[1]);
                        }
                        break;
                }
            }
        }
    }
    
    public void OnValueChange(string s)
    {
        switch (type)
        {
            case INPUTTYPE.TEXT:
                break;
        }
    }

    public void OnEndEdit(string s)
    {

    }
}
