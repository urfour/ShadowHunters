using Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogComponent : MonoBehaviour
{
    public RectTransform content;
    public LogBarComponent prefab;
    public RectTransform viewMask;
    private int index = 0;

    private void Start()
    {
        KernelLog.Instance.AddListener((sender) =>
        {
            this.Refresh();
        });
    }

    public void Refresh()
    {
        for (int i = index; i < KernelLog.Instance.Messages.Count; i++)
        {
            GameObject log = Instantiate(prefab.gameObject, content);
            LogBarComponent logComponent = log.GetComponent<LogBarComponent>();

            logComponent.Display(KernelLog.Instance.Messages[i].msg, KernelLog.Instance.Messages[i].type);
        }
        index = KernelLog.Instance.Messages.Count;

        index = KernelLog.Instance.Messages.Count;

        float choicesheigh = 0;
        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform c = content.GetChild(i) as RectTransform;
            choicesheigh += c.sizeDelta.y;
        }
        choicesheigh += (content.childCount - 1) * content.GetComponent<VerticalLayoutGroup>().spacing;
        content.sizeDelta = new Vector2(content.sizeDelta.x, choicesheigh);

        content.localPosition = new Vector3(content.localPosition.x, (-choicesheigh/2) + viewMask.sizeDelta.y, content.localPosition.z);
    }
}
