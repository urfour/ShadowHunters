using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventSystem;

namespace ServerInterface.AuthEvents
{
    class AuthTestEmul : IListener<AuthEvent>
    {
        private Dictionary<string, string> passwords = new Dictionary<string, string>(){
            {"lui", "lui" },
            {"elle", "elle" },
            {"l'autre", "l'autre" },
        };
        private Dictionary<string, Account> accounts = new Dictionary<string, Account>() {
            {"lui", new Account() {Login="lui", IsLogged=false } },
            {"elle", new Account() {Login="elle", IsLogged=false } },
            {"l'autre", new Account() {Login="l'autre", IsLogged=false } },
        };

        public void OnEvent(AuthEvent e, string[] tags = null)
        {
            //UnityEngine.Debug.Log("AuthTestEmul : " + e);
            if (e is LogInEvent lie)
            {
                if (accounts.ContainsKey(lie.Account.Login))
                {
                    if (passwords[lie.Account.Login] == lie.Password)
                    {
                        EventView.Manager.Emit(new AssingAccountEvent(accounts[lie.Account.Login]));
                    }
                    else
                    {
                        EventView.Manager.Emit(new AuthInvalidEvent() { Msg = "message.auth.invalid.login.password_invalid" });
                    }
                }
                else
                {
                    EventView.Manager.Emit(new AuthInvalidEvent() { Msg = "message.auth.invalid.login.login_invalid" });
                }
            }
            else if (e is LogOutEvent loe)
            {
                // set IsLogged to false
                EventView.Manager.Emit(new AssingAccountEvent(null));
            }
            else if (e is SignInEvent sie)
            {
                if (accounts.ContainsKey(sie.Login))
                {
                    EventView.Manager.Emit(new AuthInvalidEvent() { Msg = "message.auth.invalid.signin.login_unavailable&" + sie.Login });
                }
                else
                {
                    Account a = new Account()
                    {
                        Login = sie.Login,
                        IsLogged = true
                    };
                    passwords.Add(sie.Login, sie.Password);
                    accounts.Add(sie.Login, a);
                    EventView.Manager.Emit(new AssingAccountEvent(accounts[sie.Login]));
                }
            }
        }
    }
}
