using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ServerInterface.AuthEvents;
using EventSystem;

public class SignInComponent : MonoBehaviour
{

    public InputField login;
    public InputField password;
    public InputField confirm_password;
    public Button signinButton;


    private void Start()
    {
        OnPassChange();
    }

    public void OnButtonClic()
    {
        if (confirm_password.text == password.text)
        {
            EventView.Manager.Emit(new SignInEvent() { Login = login.text, Password = password.text });
            Debug.Log(login.text + " " + password.text);
        }
    }

    public void OnPassChange()
    {
        signinButton.interactable = password.text == confirm_password.text && login.text.Length > 0 && password.text.Length > 0;
    }
}
