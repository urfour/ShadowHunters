﻿using UnityEngine;
using System.Collections;
using EventSystem;
using Assets.Scripts.EventSystemTest;

public class EventSystemTestMono_LogListener : MonoBehaviour, IListener<LogEvent>
{

    public RectTransform parent;
    public EventSystemTestTextWriter prefab;
    public bool mainThreaded = true;

    public void OnEvent(LogEvent e, string[] tags = null)
    {
        GameObject o = Instantiate(prefab.gameObject, parent);
        o.GetComponent<EventSystemTestTextWriter>().SetText("IListener<LogEvent> : " + e.Msg);
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
