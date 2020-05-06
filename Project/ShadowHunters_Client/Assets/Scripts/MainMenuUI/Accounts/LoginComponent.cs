using EventSystem;
using ServerInterface.AuthEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            var sha256 = SHA256.Create();
            var sha1pass = sha256.ComputeHash(Encoding.Unicode.GetBytes(password.text));
            EventView.Manager.Emit(new LogInEvent(new Account() { Login = login.text }, Encoding.Unicode.GetString(sha1pass)));
        }

        public void OnPassChange()
        {
            loginButton.interactable = login.text.Length > 0 && password.text.Length > 0;
        }
    }
}
