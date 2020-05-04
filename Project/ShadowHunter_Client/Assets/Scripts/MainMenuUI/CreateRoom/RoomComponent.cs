using Assets.Scripts.MainMenuUI.SearchGame;
using EventSystem;
using ServerInterface.RoomEvents;
using UnityEngine;
using UnityEngine.UI;

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

    public InputField privateJoinCode;
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

        if (GRoom.Instance.JoinedRoom.RawData != null)
        {
            Logger.Info("LeaveRoom on RoomComponent.Start()");
            EventView.Manager.Emit(new LeaveRoomEvent() { RoomData = new RoomData() { Code = GRoom.Instance.JoinedRoom.RawData.Code } });
        }
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
        if (WaitingRoomListContent == null)
        {
            Logger.Error("WaitingRoomListContent == null");
            return;
        }

        for (int i = WaitingRoomListContent.transform.childCount - 1; i >= 0; i--)
        {
            Logger.Info("WaitingRoomListContent destroying : " + WaitingRoomListContent.transform.GetChild(i).name);
            Destroy(WaitingRoomListContent.transform.GetChild(i).gameObject);
        }
        
        foreach (var pair in GRoom.Instance.Rooms)
        {
            if (!pair.Value.IsPrivate.Value)
            {
                GameObject o = GameObject.Instantiate(RoomPrefab, WaitingRoomListContent.transform);
                RoomBarComponent rbc = o.GetComponent<RoomBarComponent>();
                rbc.Display(pair.Value);
            }
        }
    }

    public void OnPrivateJoin()
    {
        int code;
        if (int.TryParse(privateJoinCode.text.Substring(1), out code))
        {
            EventView.Manager.Emit(new JoinRoomEvent() { RoomData = new RoomData() { Code = code } });
        }
    }

    public void OnCodeTextModified(string newText)
    {
        Debug.Log("1 : " + privateJoinCode.text);
        if (privateJoinCode.text.Length <= 0)
        {
            privateJoinCode.text = "#";
        }
        if (privateJoinCode.text[0] != '#')
        {
            privateJoinCode.text = '#' + privateJoinCode.text;
        }
        for (int i = privateJoinCode.text.Length-1; i > 0; i--)
        {
            if (!char.IsDigit(privateJoinCode.text[i]))
            {
                privateJoinCode.text = privateJoinCode.text.Remove(i,1);
                //Debug.Log("1 : " + privateJoinCode.text);
            }
        }
    }
}