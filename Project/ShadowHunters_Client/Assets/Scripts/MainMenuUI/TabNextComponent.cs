using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabNextComponent : MonoBehaviour, IUpdateSelectedHandler
{
    public Selectable nextField;

    public void OnUpdateSelected(BaseEventData data)
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            nextField.Select();
    }
}
