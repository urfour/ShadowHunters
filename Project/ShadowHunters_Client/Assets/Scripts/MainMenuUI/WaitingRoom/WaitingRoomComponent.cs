using Assets.Scripts.MainMenuUI.Accounts;
using Assets.Scripts.MainMenuUI.SearchGame;
using EventSystem;
using ServerInterface.AuthEvents;
using ServerInterface.RoomEvents;
using UnityEngine;
using UnityEngine.UI;

class WaitingRoomComponent : MonoBehaviour
{
    public RectTransform playersContent;
    public PlayerDisplayComponent playerPrefab;
    public Button startGame;
    public Text roomCode;
    public Text roomName;

    private OnNotification notification;

    public void Start()
    {

    }

    public void OnEnable()
    {
        if (notification == null)
        {
            notification = (sender) =>
            {
                this.Refresh();
            };
        }
        Refresh();
        GRoom.Instance.JoinedRoom.AddListener(notification);
    }

    public void OnDisable()
    {
        GRoom.Instance.JoinedRoom.RemoveListener(notification);
    }

    public void Refresh()
    {
        if (GRoom.Instance.JoinedRoom != null)
        {
            Room r = GRoom.Instance.JoinedRoom;
            //int nb = playersContent.childCount;
            for (int i = playersContent.childCount - 1; i >= 0; i--)
            {
                Transform t = playersContent.GetChild(i);
                Destroy(t.gameObject);
            }
            if (r.Players.Value != null)
            {
                roomName.text = r.Name.Value;
                roomCode.text = "#" + r.Code.Value;
                for (int i = 0; i < r.CurrentNbPlayer.Value; i++)
                {
                    Account a = GAccount.Instance.GetAccount(r.Players.Value[i]);
                    GameObject o = Instantiate(playerPrefab.gameObject, playersContent);
                    o.GetComponent<PlayerDisplayComponent>().DisplayPlayer(a, i);
                }
                if (r.Players.Value[0] == GAccount.Instance.LoggedAccount.Login)
                {
                    bool ready = true;
                    for (int i = 0; i < r.MaxNbPlayer.Value; i++)
                    {
                        if (!r.RawData.ReadyPlayers[i])
                        {
                            ready = false;
                            break;
                        }
                    }
                    startGame.interactable = ready;
                }
                else
                {
                    startGame.interactable = false;
                }
            }
            else
            {
                startGame.interactable = false;
            }

        }
        else
        {
            Debug.LogError("GRoom.Instance.JoinedRoom is null");
        }
    }

    public void ExitRoomButtonClick()
    {
        //Debug.Log("Exit " + GRoom.Instance.JoinedRoom.Name);
        EventView.Manager.Emit(new LeaveRoomEvent() { RoomData = new RoomData() { Code = GRoom.Instance.JoinedRoom.RawData.Code } });
    }

    public void StartGameButtonClick()
    {
        EventView.Manager.Emit(new StartGameEvent() { RoomData = new RoomData() { Code = GRoom.Instance.JoinedRoom.RawData.Code } });
    }
}
