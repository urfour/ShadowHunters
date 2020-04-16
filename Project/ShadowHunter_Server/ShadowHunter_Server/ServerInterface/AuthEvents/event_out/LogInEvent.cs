using Network.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.AuthEvents
{
    public class LogInEvent : AuthEvent
    {
        public Account Account { get; set; }
        public string Password { get; set; }

        public LogInEvent()
        {

        }

        public LogInEvent(Account account, string password, string msg = null) : base(msg)
        {
            this.Account = account;
            this.Password = password;
        }
    }
}
