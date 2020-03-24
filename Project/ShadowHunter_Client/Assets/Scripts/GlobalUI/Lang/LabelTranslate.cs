using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EventSystem;

[RequireComponent(typeof(Text))]
public class LabelTranslate : MonoBehaviour
{
    public string label;
    
    private OnNotification listener = null;
    
    // Use this for initialization
    void Start()
    {
        if (label != null)
        {
            Text t = gameObject.GetComponent<Text>();
            t.text = Lang.Language.Translate(label);
            listener = (sender) => { t.text = Lang.Language.Translate(label); };
            //Lang.Language.AddListener(listener);
            if (isActiveAndEnabled)
            {
                OnEnable();
            }
        }
    }

    public void Refresh()
    {
        Text t = gameObject.GetComponent<Text>();
        t.text = Lang.Language.Translate(label);
    }

    private void OnDisable()
    {
        Lang.Language.RemoveListener(listener);
    }

    private void OnEnable()
    {
        if (listener != null)
            Lang.Language.AddListener(listener);
        Refresh();
    }
}
