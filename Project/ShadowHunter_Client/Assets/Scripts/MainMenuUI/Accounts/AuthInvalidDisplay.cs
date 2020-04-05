using EventSystem;
using ServerInterface.AuthEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class AuthInvalidDisplay : MonoBehaviour, IListener<AuthInvalidEvent>
{
    public Text messageBox;

    private void Start()
    {
        print("AuthInvalidDisplay");
        EventView.Manager.AddListener(this, true);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventView.Manager.RemoveListener(this);
    }

    public void OnEvent(AuthInvalidEvent e, string[] tags = null)
    {
        if (e.Msg != null)
        {
            messageBox.text = e.Msg.Replace("\\n", "\n");
            gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("AuthInvalidEvent message null");
        }
    }

    public void OnValidButtonClick()
    {
        gameObject.SetActive(false);
    }
}
