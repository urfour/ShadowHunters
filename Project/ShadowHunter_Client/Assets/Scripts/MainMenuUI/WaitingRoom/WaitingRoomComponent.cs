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
    public Text roomCode;

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
                roomCode.text = "#" + r.Code.Value;
                foreach (string p in r.Players.Value)
                {
                    if (p == null)
                    {
                        //Debug.LogError("null login");
                        continue;
                    }
                    Account a = GAccount.Instance.GetAccount(p);
                    GameObject o = Instantiate(playerPrefab.gameObject, playersContent);
                    o.GetComponent<PlayerDisplayComponent>().DisplayPlayer(a);
                }
            }
        }
        else
        {
            Debug.LogError("GRoom.Instance.JoinedRoom is null");
        }
    }

    public void ExitRoomButtonClick()
    {
        Debug.Log("Exit " + GRoom.Instance.JoinedRoom.Name);
        EventView.Manager.Emit(new LeaveRoomEvent() { RoomData = GRoom.Instance.JoinedRoom.RawData });
    }
}
