using EventSystem;
using Network.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.events
{
    public class ServerOnlyEvent : Event
    {
        [NonSerialized] private Client sender;

        public Client GetSender()
        {
            return sender;
        }

        public void SetSender(Client c)
        {
            //if (sender != null) Logger.Error(new InvalidOperationException("Cannot set a sender if Client.sender != null"));
            this.sender = c;
        }

        public string Msg { get; set; }

        public ServerOnlyEvent()
        {

        }

        public ServerOnlyEvent(string msg = null)
        {
            this.Msg = msg;
        }
    }
}
