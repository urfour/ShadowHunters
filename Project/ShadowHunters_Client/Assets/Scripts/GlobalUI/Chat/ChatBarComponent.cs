using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ChatSystem
{
    public class ChatBarComponent : MonoBehaviour
    {
        public Text text;
        public void Display(string message)
        {
            text.text = message;
        }
    }
}