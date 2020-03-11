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



    public void OnButtonClic()
    {
        EventView.Manager.Emit(new SignInEvent() { Login = login.text, Password = password.text });
        Debug.Log(login.text + " " + password.text);
    }
}
