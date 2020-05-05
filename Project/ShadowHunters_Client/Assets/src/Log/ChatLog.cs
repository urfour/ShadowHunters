using EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Log
{
    public class ChatLog : ListenableObject
    {
        public static ChatLog Instance { get; private set; }
        public List<string> Messages { get; private set; } = new List<string>();


        public ChatLog()
        {
            Instance = this;
        }

        public void SendMessage(string sender, string msg)
        {
            Messages.Add(sender + " : " + msg);
            Notify();
        }

        
    }

}
