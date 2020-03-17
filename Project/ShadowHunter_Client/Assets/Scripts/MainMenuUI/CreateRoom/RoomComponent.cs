using EventSystem;
using ServerInterface.RoomEvents;
using UnityEngine;



class RoomComponent : MonoBehaviour, IListener<RoomEvent>
{
    //public MenuSelection NotLogged;
    //public MenuSelection Logged;


    //public MenuSelection[] interactableOnInRoomOnly;
    //public MenuSelection[] activeOnInRoomOnly;

    public MenuSelection createRoom;
    public MenuSelection searchRoom;
    public MenuSelection waitingRoom;

    public void JoinRoom()
    {
        createRoom.gameObject.SetActive(false);
        searchRoom.gameObject.SetActive(false);
        waitingRoom.gameObject.SetActive(true);
        waitingRoom.manager.SetSelected(waitingRoom);
    }

    public void OnEvent(RoomEvent e, string[] tags = null)
    {
        if (e is RoomJoinedEvent rj)
        {
            JoinRoom();
        }
    }
}