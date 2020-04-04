using Assets.Scripts.MainMenuUI.Accounts;
using Assets.Scripts.MainMenuUI.SearchGame;
using EventSystem;
using ServerInterface.AuthEvents;
using UnityEngine;

class WaitingRoomComponent : MonoBehaviour
{
    public RectTransform playersContent;
    public PlayerDisplayComponent playerPrefab;

    private OnNotification notification;

    public void Start()
    {
        notification = (sender) =>
        {
            this.Refresh();
        };
    }

    public void OnEnable()
    {
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
}
