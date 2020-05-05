using UnityEngine;
using System.Collections;
using Assets.Scripts.MainMenuUI.Accounts;
using ServerInterface.AuthEvents;
using UnityEngine.UI;

public class AuthComponent : MonoBehaviour
{

    public MenuSelection NotLogged;
    public MenuSelection Logged;
    

    public MenuSelection[] interactableOnLoggedOnly;
    public MenuSelection[] activeOnLoggedOnly;

    public void Start()
    {
        GAccount.Instance.AssignChange.AddListener((sender) =>
        {
            this.Refresh();
        });
        this.Refresh();
    }

    public void Refresh()
    {
        Account a = GAccount.Instance.LoggedAccount;
        if (a != null)
        {
            //Logged.gameObject.SetActive(true);
            if (activeOnLoggedOnly != null)
            {
                foreach (var m in activeOnLoggedOnly)
                {
                    m.gameObject.SetActive(true);
                }
            }
            if (interactableOnLoggedOnly != null)
            {
                foreach (var m in interactableOnLoggedOnly)
                {
                    m.GetComponent<Button>().interactable = true;
                }
            }
            NotLogged.manager.SetSelected(Logged);
            NotLogged.gameObject.SetActive(false);
        }
        else
        {
            if (interactableOnLoggedOnly != null)
            {
                foreach (var m in interactableOnLoggedOnly)
                {
                    m.GetComponent<Button>().interactable = false;
                }
            }
            if (activeOnLoggedOnly != null)
            {
                foreach (var m in activeOnLoggedOnly)
                {
                    m.gameObject.SetActive(false);
                }
            }
            NotLogged.gameObject.SetActive(true);
            Logged.manager.SetSelected(NotLogged);
            //Logged.gameObject.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
