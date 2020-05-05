using Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ChatSystem
{
    public class ChatComponent : MonoBehaviour
    {
        public RectTransform content;
        public ChatBarComponent prefab;
        public RectTransform viewMask;
        private int index = 0;

        private void Start()
        {
            new ChatLog();
            ChatLog.Instance.AddListener((sender) =>
            {
                this.Refresh();
            });
        }

        public void Refresh()
        {
            for (int i = index; i < ChatLog.Instance.Messages.Count; i++)
            {
                GameObject log = Instantiate(prefab.gameObject, content);
                ChatBarComponent chatComponent = log.GetComponent<ChatBarComponent>();

                chatComponent.Display(ChatLog.Instance.Messages[i]);
            }
            index = ChatLog.Instance.Messages.Count;

            float choicesheigh = 0;
            for (int i = 0; i < content.childCount; i++)
            {
                RectTransform c = content.GetChild(i) as RectTransform;
                choicesheigh += c.sizeDelta.y;
            }
            choicesheigh += (content.childCount - 1) * content.GetComponent<VerticalLayoutGroup>().spacing;
            content.sizeDelta = new Vector2(content.sizeDelta.x, choicesheigh);

        }
    }
}