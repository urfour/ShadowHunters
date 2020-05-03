using Network.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.AuthEvents
{
    public class AccountDataEvent : AuthEvent
    {
        public Account Account { get; set; }

        public AccountDataEvent(Account account, string msg = null) : base(msg)
        {
            this.Account = account;
        }

        public AccountDataEvent()
        {

        }
    }
}
