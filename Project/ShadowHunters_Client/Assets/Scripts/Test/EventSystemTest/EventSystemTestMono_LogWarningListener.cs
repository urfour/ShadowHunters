using UnityEngine;
using System.Collections;
using EventSystem;
using Assets.Scripts.EventSystemTest;

public class EventSystemTestMono_LogWarningListener : MonoBehaviour, IListener<LogWarningEvent>
{

    public RectTransform parent;
    public EventSystemTestTextWriter prefab;
    public bool mainThreaded = true;

    public Color WarningColor;

    public void OnEvent(LogWarningEvent e, string[] tags = null)
    {
        GameObject o = Instantiate(prefab.gameObject, parent);
        o.GetComponent<EventSystemTestTextWriter>().SetText("IListener<LogWarningEvent> : " + e.Msg, WarningColor);
        o.SetActive(true);
    }

    // Use this for initialization
    void Start()
    {
        EventView.Manager.AddListener(this, MainThreaded:mainThreaded);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
