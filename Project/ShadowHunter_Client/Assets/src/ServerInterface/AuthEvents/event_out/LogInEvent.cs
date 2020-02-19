using Network.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.AuthEvents.event_out
{
    class LogInEvent : AuthEvent
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public LogInEvent()
        {

        }

        public LogInEvent(string login, string password, string msg = null) : base(msg)
        {
            this.Login = login;
            this.Password = password;
        }
    }
}
