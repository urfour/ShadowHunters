using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LabelTranslate : MonoBehaviour
{
    public string label;

    // Use this for initialization
    void Start()
    {
        if (label != null)
        {
            Text t = gameObject.GetComponent<Text>();
            t.text = Lang.Language.Translate(label);
            Lang.Language.AddListener((sender) => { t.text = Lang.Language.Translate(label); });
        }
    }
}
