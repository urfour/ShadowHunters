using EventSystem;
using ServerInterface.AuthEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenuUI.Accounts
{
    class LoginComponent : MonoBehaviour
    {
        public InputField login;
        public InputField password;
        public Button loginButton;

        public void Start()
        {
            OnPassChange();
        }

        public void OnButtonClic()
        {
            EventView.Manager.Emit(new LogInEvent(new Account() { Login = login.text }, password.text));
            Debug.Log(login.text + " " + password.text);
        }

        public void OnPassChange()
        {
            loginButton.interactable = login.text.Length > 0 && password.text.Length > 0;
        }
    }
}
