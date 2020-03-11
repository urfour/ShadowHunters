using Network.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.AuthEvents
{
    class SignInEvent : AuthEvent
    {
        public string Login { get; set; }
        public string Password { get; set; }
        
    }
}
