using Assets.Scripts.MainMenuUI.Accounts;
using EventSystem;
using Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ChatSystem
{
    public class ChatMessage : MonoBehaviour, IListener<ChatMessageEvent>
    {
        public InputField inputField;

        public void Start()
        {
            EventView.Manager.AddListener(this);
        }

        public void OnEndEdit()
        {
            if (inputField.text != "")
            {
                EventView.Manager.Emit(new ChatMessageEvent(GAccount.Instance.LoggedAccount.Login, inputField.text));
                inputField.text = "";
            }
        }
        public void OnEvent(ChatMessageEvent e, string[] tags = null)
        {
            ChatLog.Instance.SendMessage(e.User, e.MessageSend);
        }

        public void OnDestroy()
        {
            EventView.Manager.RemoveListener(this);
        }

    }
}

