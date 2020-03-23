using Assets.Scripts.MainMenuUI.SearchGame;
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

    public GameObject WaitingRoomListContent;
    public GameObject RoomPrefab;

    public void JoinRoom()
    {
        createRoom.gameObject.SetActive(false);
        searchRoom.gameObject.SetActive(false);
        waitingRoom.gameObject.SetActive(true);
        waitingRoom.manager.SetSelected(waitingRoom);
    }

    private void Start()
    {
        searchRoom.OnClicked.AddListener((sender) =>
        {
            RefreshSearchRoom();
        });
    }

    public void OnEvent(RoomEvent e, string[] tags = null)
    {
        if (e is RoomJoinedEvent rj)
        {
            JoinRoom();
        }
    }

    public void RefreshSearchRoom()
    {
        for (int i = WaitingRoomListContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(WaitingRoomListContent.transform.GetChild(i).gameObject);
        }

        foreach (var pair in GRoom.Instance.Rooms)
        {
            GameObject o = GameObject.Instantiate(RoomPrefab, WaitingRoomListContent.transform);
            RoomBarComponent rbc = o.GetComponent<RoomBarComponent>();
            rbc.Display(pair.Value);
        }
    }
}