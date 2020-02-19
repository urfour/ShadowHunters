using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelection : MonoBehaviour {
    
    public RectTransform Content;
    public MenuSelectionManager manager;

    public void Start()
    {
        if (Content == null || manager == null)
        {
            Debug.LogWarning("Missing field in <" + gameObject.name + ">");
        }
        manager.Init(this);
    }


    public void Clicked()
    {
        //Text.r;
        //Button b;
        manager.SetSelected(this);
    }
}
