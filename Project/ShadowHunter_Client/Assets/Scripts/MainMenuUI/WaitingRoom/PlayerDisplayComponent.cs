using UnityEngine;
using System.Collections;
using ServerInterface.AuthEvents;
using UnityEngine.UI;
using Assets.Scripts.MainMenuUI.Accounts;
using EventSystem;
using ServerInterface.RoomEvents;
using Assets.Scripts.MainMenuUI.SearchGame;

public class PlayerDisplayComponent : MonoBehaviour
{
    public Text pseudoDisplay;
    public Button readyButton;
    public Text ReadyDisplay;
    public string notReadyText;
    public string readyText;

    private Account assigned;
    //private bool ready = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayPlayer(Account account, int index)
    {
        assigned = account;
        pseudoDisplay.text = account.Login;
        if (account.Login == GAccount.Instance.LoggedAccount.Login)
        {
            readyButton.interactable = true;
        }
        else
        {
            readyButton.interactable = false;
        }
        if (GRoom.Instance.JoinedRoom.RawData.ReadyPlayers[index])
        {
            ReadyDisplay.text = readyText;
        }
        else
        {
            ReadyDisplay.text = notReadyText;
        }
    }

    public void ReadyButtonClick()
    {
        //ready = !ready;
        EventView.Manager.Emit(new ReadyEvent() { RoomData = GRoom.Instance.JoinedRoom.RawData });
        //DisplayPlayer(assigned);
    }
}
