using System.Collections;
using System.Collections.Generic;
using EventSystem;

namespace Assets.Scripts.ChatSystem
{
    public class ChatMessageEvent : Event
    {
        public string User { get; set; }
        public string MessageSend { get; set; }
        public ChatMessageEvent(string user, string msg)
        {
            this.User = User;
            this.MessageSend = msg;
        }

        public ChatMessageEvent()
        {

        }
    }
}