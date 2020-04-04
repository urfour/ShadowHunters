using UnityEngine;
using System.Collections;
using ServerInterface.AuthEvents;
using UnityEngine.UI;
using Assets.Scripts.MainMenuUI.Accounts;

public class PlayerDisplayComponent : MonoBehaviour
{
    public Text pseudoDisplay;
    public Button readyButton;
    public Text ReadyDisplay;
    public string notReadyText;
    public string readyText;

    private Account assigned;
    private bool ready = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayPlayer(Account account)
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
        if (ready)
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
        ready = !ready;
        DisplayPlayer(assigned);
        // todo emit event
    }
}
