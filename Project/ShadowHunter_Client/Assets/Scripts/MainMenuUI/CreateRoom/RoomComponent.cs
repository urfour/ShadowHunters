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

    private OnNotification gRoomNotification;

    public MenuSelection createRoom;
    public MenuSelection searchRoom;
    public MenuSelection waitingRoom;

    public GameObject WaitingRoomListContent;
    public GameObject RoomPrefab;

    public void JoinRoom()
    {
        createRoom.gameObject.SetActive(false);
        searchRoom.gameObject.SetActive(true);
        waitingRoom.gameObject.SetActive(true);
        waitingRoom.manager.SetSelected(waitingRoom);
    }

    public void LeaveRoom()
    {
        createRoom.gameObject.SetActive(true);
        searchRoom.gameObject.SetActive(true);
        waitingRoom.gameObject.SetActive(false);
        waitingRoom.manager.SetSelected(searchRoom);
    }

    private void Start()
    {
        searchRoom.OnClicked.AddListener((sender) =>
        {
            RefreshSearchRoom();
        });
        waitingRoom.gameObject.SetActive(false);
        EventView.Manager.AddListener(this, true);
        gRoomNotification = (sender) =>
        {
            RefreshSearchRoom();
        };
        GRoom.Instance.AddListener(gRoomNotification);
    }

    private void OnDestroy()
    {
        EventView.Manager.RemoveListener(this);
    }

    public void OnEvent(RoomEvent e, string[] tags = null)
    {
        if (e is RoomJoinedEvent rj)
        {
            JoinRoom();
        }
        else if (e is RoomLeavedEvent rle)
        {
            LeaveRoom();
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