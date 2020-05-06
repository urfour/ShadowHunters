using Lang;
using Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogBarComponent : MonoBehaviour
{
    public Text text;
    public void Display(string message, KernelLogType type)
    {
        text.text = Language.Translate(message);
    }

}
