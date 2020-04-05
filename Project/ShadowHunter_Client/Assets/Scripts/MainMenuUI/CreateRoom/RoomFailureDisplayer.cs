using UnityEngine;
using System.Collections;
using EventSystem;
using ServerInterface.RoomEvents;
using UnityEngine.UI;

public class RoomFailureDisplayer : MonoBehaviour, IListener<RoomFailureEvent>
{
    public Text messageBox;


    public void OnEvent(RoomFailureEvent e, string[] tags = null)
    {
        if (e.Msg != null)
        {
            messageBox.text = e.Msg.Replace("\\n", "\n");
            gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("AuthInvalidEvent message null");
        }
    }

    private void Start()
    {
        print("RoomFailureDisplayer loaded");
        EventView.Manager.AddListener(this, true);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventView.Manager.RemoveListener(this);
    }


    public void OnValidButtonClick()
    {
        gameObject.SetActive(false);
    }
}
