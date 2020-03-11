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
        public static GAccount Instance { get; private set; }

        public Dictionary<int, Account> Accounts { get; private set; }
        public Account LoggedAccount { get; private set; }


        public void OnEvent(AuthEvent e, string[] tags = null)
        {
            if (e is AssingAccountEvent aae)
            {

            }
            else if (e is AccountDataEvent ade)
            {

            }
            else if (e is AuthInvalidEvent aie)
            {

            }
        }
    }
}
