using ServerInterface.AuthEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.events
{
    public class NetworkDisconnectedEvent : ServerOnlyEvent
    {
        public Account Account { get; set; }

        public NetworkDisconnectedEvent(Account account)
        {
            this.Account = account;
        }

        public NetworkDisconnectedEvent()
        {

        }
    }
}
