using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ServerInterface.AuthEvents;
using EventSystem;
using System.Security.Cryptography;
using System.Text;

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
            var sha256 = SHA256.Create();
            var sha1pass = sha256.ComputeHash(Encoding.Unicode.GetBytes(password.text));
            EventView.Manager.Emit(new SignInEvent() { Login = login.text, Password = Encoding.Unicode.GetString(sha1pass) });
        }
    }

    public void OnPassChange()
    {
        signinButton.interactable = password.text == confirm_password.text && login.text.Length > 0 && password.text.Length > 0;
    }
}
