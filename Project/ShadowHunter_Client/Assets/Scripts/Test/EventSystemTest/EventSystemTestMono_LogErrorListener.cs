using UnityEngine;
using System.Collections;
using EventSystem;
using Assets.Scripts.EventSystemTest;

public class EventSystemTestMono_LogErrorListener : MonoBehaviour, IListener<LogErrorEvent>
{

    public RectTransform parent;
    public EventSystemTestTextWriter prefab;
    public bool mainThreaded = true;

    public Color ErrorColor;

    public void OnEvent(LogErrorEvent e, string[] tags = null)
    {
        GameObject o = Instantiate(prefab.gameObject, parent);
        o.GetComponent<EventSystemTestTextWriter>().SetText("IListener<LogErrorEvent> : " + e.Msg, ErrorColor);
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
