using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EventSystem;

[RequireComponent(typeof(Text))]
public class LabelTranslate : MonoBehaviour
{
    public string label;
    
    private OnNotification listener = null;
    private bool isListening = false;
    
    // Use this for initialization
    void Start()
    {
        if (label != null)
        {
            Text t = gameObject.GetComponent<Text>();
            t.text = Lang.Language.Translate(label);
            listener = (sender) => { t.text = Lang.Language.Translate(label); };
            //Lang.Language.AddListener(listener);
            if (gameObject.activeInHierarchy)
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
        if (listener != null)
        {
            Lang.Language.RemoveListener(listener);
            isListening = false;
        }
    }

    private void OnEnable()
    {
        if (listener != null)
        {
            Lang.Language.AddListener(listener);
            isListening = true;
        }
        Refresh();
    }

    private void OnDestroy()
    {
        OnDisable();
    }
}
