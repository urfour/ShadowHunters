using EventSystem;
using Network.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.listener
{
    class ChatListener : IListener<ChatMSGEvent>
    {
        public void OnEvent(ChatMSGEvent e, string[] tags = null)
        {
            Logger.Comment(e.MSG);
        }
    }
}
