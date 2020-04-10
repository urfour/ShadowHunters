using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EventSystem;
using Network.events;
using ServerInterface.RoomEvents;
using ServerInterface.AuthEvents;
using Lang;

public class ServerMessage : MonoBehaviour, IListener<ServerOnlyEvent>
{
    public Text messageBox;

    private void Start()
    {
        print("ServerMessage loaded");
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

    public void OnEvent(ServerOnlyEvent e, string[] tags = null)
    {
        if (e.Msg != null)
        {
            if (! (e is RoomFailureEvent) && ! (e is AuthInvalidEvent))
            {
                messageBox.text = Language.Translate(e.Msg).Replace("\\n", "\n");
                gameObject.SetActive(true);
            }
        }
    }
}
