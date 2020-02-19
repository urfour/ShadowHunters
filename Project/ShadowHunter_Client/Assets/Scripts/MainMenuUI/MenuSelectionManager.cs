using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuSelectionManager : MonoBehaviour
{
    public MenuSelection currentSelected;
    public RectTransform ViewPort;
    public ScrollRect container;

    public void SetSelected(MenuSelection ms)
    {
        container.content = ms.Content;
        if (currentSelected != null)
        {
            currentSelected.Content.gameObject.SetActive(false);
            currentSelected.Content.localPosition = new Vector3(0, ViewPort.rect.height / 2 + currentSelected.Content.rect.height / 2, 0);
        }

        currentSelected = ms;
        ms.Content.gameObject.SetActive(true);
        ms.Content.localPosition = new Vector3(0, ViewPort.rect.height / 2 + ms.Content.rect.height / 2, 0);
    }

    public void Init(MenuSelection ms)
    {
        //currentSelected = ms;
        ms.Content.gameObject.SetActive(ms == currentSelected);
        //ms.Content.localPosition = new Vector3(0, ViewPort.rect.height / 2 + ms.Content.rect.height / 2, 0);
    }
}
