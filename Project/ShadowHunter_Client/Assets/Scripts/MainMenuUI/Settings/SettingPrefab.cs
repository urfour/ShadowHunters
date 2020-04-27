using UnityEngine;
using System.Collections;
using Kernel.Settings;
using EventSystem;

public class SettingPrefab : MonoBehaviour
{
    private OnNotification listener;
    public LabelTranslate label;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Configurate(string label, SettingItem target, string config = null)
    {
        if (label != null)
        {
            this.label.label = label;
            this.label.Refresh();
        }
        if (listener != null)
        {
            target.RemoveListener(listener);
        }
        if (target != null)
        {
            listener = (sender) => { this.Refresh(); };
            target.AddListener(listener);
        }
    }

    public virtual void Refresh()
    {

    }
}
