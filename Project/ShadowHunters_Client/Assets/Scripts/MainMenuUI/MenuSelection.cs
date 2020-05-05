using EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuSelection : MonoBehaviour {
    
    public RectTransform Content;
    public MenuSelectionManager manager;
    public ListenableObject OnClicked = new ListenableObject();

    public void Start()
    {
        if (Content == null || manager == null)
        {
            Debug.LogWarning("Missing field in <" + gameObject.name + ">");
            GetComponent<Button>().interactable = false;
        }
        manager.Init(this);
    }


    public void Clicked()
    {
        //Text.r;
        //Button b;
        manager.SetSelected(this);
        OnClicked.Notify();
    }
}
