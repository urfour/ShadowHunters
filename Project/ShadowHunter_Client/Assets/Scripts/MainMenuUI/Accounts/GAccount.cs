using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventSystem;
using ServerInterface.AuthEvents;

namespace Assets.Scripts.MainMenuUI.Accounts
{
    class GAccount : IListener<AuthEvent>
    {
        public static GAccount Instance { get; private set; } = null;

        public Dictionary<string, Account> Accounts { get; private set; } = new Dictionary<string, Account>();
        public Account LoggedAccount { get; private set; } = null;

        public ListenableObject AssignChange { get; private set; } = new ListenableObject();

        private GAccount()
        {
            if (Instance != null)
            {

            }
            else
            {
                Instance = this;
            }
        }

        public static void Init()
        {
            new GAccount();
            EventView.Manager.AddListener(Instance, true);
        }

        public void OnEvent(AuthEvent e, string[] tags = null)
        {
            UnityEngine.Debug.Log("GAccount : " + e);
            if (e is AssingAccountEvent aae)
            {
                this.LoggedAccount = aae.Account;
                AssignChange.Notify();
            }
            else if (e is AccountDataEvent ade)
            {
                if (LoggedAccount != null && ade.Account.Login == LoggedAccount.Login)
                {
                    LoggedAccount = ade.Account;
                }
                if (Accounts.ContainsKey(ade.Account.Login))
                {
                    Accounts[ade.Account.Login] = ade.Account;
                }
                else
                {
                    Accounts.Add(ade.Account.Login, ade.Account);
                }
            }
            else if (e is AuthInvalidEvent aie)
            {
                UnityEngine.Debug.Log(aie.Msg);
            }
        }
    }
}
