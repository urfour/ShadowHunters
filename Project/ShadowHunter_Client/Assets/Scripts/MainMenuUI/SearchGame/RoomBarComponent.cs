using UnityEngine;
using System.Collections;
using Assets.Scripts.MainMenuUI.SearchGame;
using UnityEngine.UI;
using EventSystem;
using System.Collections.Generic;
using ServerInterface.RoomEvents;

public class RoomBarComponent : MonoBehaviour
{
    public Text roomName;
    //public Button joinButton;
    public Text nbPlayers;

    private Room displayed;
    private Dictionary<ListenableObject, OnNotification> notifications = new Dictionary<ListenableObject, OnNotification>();

    // Use this for initialization
    void Start()
    {

    }
    

    public void Display(Room r)
    {
        StopListen();
        notifications.Clear();
        displayed = r;
        notifications.Add(displayed.Name, (sender) =>
        {
            roomName.text = displayed.Name.Value;
        });

        notifications.Add(displayed.CurrentNbPlayer, (sender) =>
        {
            nbPlayers.text = displayed.CurrentNbPlayer.Value + " / " + displayed.MaxNbPlayer.Value;
        });

        notifications.Add(displayed.MaxNbPlayer, (sender) =>
        {
            nbPlayers.text = displayed.CurrentNbPlayer.Value + " / " + displayed.MaxNbPlayer.Value;
        });

        StartListen();
    }

    public void StopListen()
    {
        foreach (var pair in notifications)
        {
            pair.Key.RemoveListener(pair.Value);
        }
    }

    public void StartListen()
    {
        foreach (var pair in notifications)
        {
            pair.Key.AddListener(pair.Value);
            pair.Value(pair.Key);
        }
    }

    public void JoinButtonClick()
    {
        print("joinbuttonclick : " + displayed.Name.Value);
        EventView.Manager.Emit(new JoinRoomEvent() { RoomData = displayed.RawData });
    }

    private void OnDestroy()
    {
        StopListen();
    }
}
