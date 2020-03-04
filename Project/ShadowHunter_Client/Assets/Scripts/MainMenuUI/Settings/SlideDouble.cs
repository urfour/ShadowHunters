using UnityEngine;
using System.Collections;
using Kernel.Settings;
using UnityEngine.UI;
using System;

public class SlideDouble : SettingPrefab
{

    public Slider slider;
    public Text valueDisplay;

    private int nbDecimals = 3;
    private int nbDisplayedDecimals = 3;
    private double displayedMultiplier = 1.0;
    private double min = 0.0;
    private double max = 1.0;
    private string displayedCast = "";
    private Setting<double> target;

    public override void Configurate(string label, SettingItem target, string config = null)
    {
        //this.label.label = label;
        //this.label.Refresh();
        this.target = (Setting<double>)target;
        if (config != null)
        {
            string[] args = config.Split('&');
            foreach (string arg in args)
            {
                string[] param = arg.Split('=');
                switch (param[0])
                {
                    case "max":
                        {
                            max = double.Parse(param[1]);
                        }
                        break;
                    case "min":
                        {
                            min = double.Parse(param[1]);
                        }
                        break;
                    case "nb_decimals":
                        {
                            nbDecimals = int.Parse(param[1]);
                        }
                        break;
                    case "displayed_multiplier":
                        {
                            displayedMultiplier = double.Parse(param[1]);
                        }
                        break;
                    case "nb_displayed_decimals":
                        {
                            nbDecimals = int.Parse(param[1]);
                        }
                        break;
                    case "displayed_cast":
                        {
                            displayedCast = param[1];
                        }
                        break;
                }
            }
        }
        //slider.value = (float)(((double)target.ItemValue - min)/max);
        base.Configurate(label, target, config);
        Refresh();
    }
    
    public void OnChange()
    {
        double val = Math.Round(slider.value * (max - min) + min, nbDecimals);
        //double dval = Math.Round(val * displayedMultiplier, nbDisplayedDecimals);
        //if (displayedCast == "int")
        //{
        //    valueDisplay.text = ((int)dval).ToString();
        //}
        //else
        //{
        //    valueDisplay.text = dval.ToString();
        //}
        target.Value = val;
    }

    public override void Refresh()
    {
        slider.value = (float)((target.Value - min) / (max - min));
        double dval = Math.Round(target.Value * displayedMultiplier, nbDisplayedDecimals);
        if (displayedCast == "int")
        {
            valueDisplay.text = ((int)dval).ToString();
        }
        else
        {
            valueDisplay.text = dval.ToString();
        }
    }
}
