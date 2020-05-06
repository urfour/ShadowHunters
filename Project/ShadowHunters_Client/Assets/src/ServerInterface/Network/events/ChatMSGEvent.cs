using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.events
{
    public class ChatMSGEvent : Event
    {
        public string MSG { get; set; }

        public ChatMSGEvent()
        {

        }
    }
}
