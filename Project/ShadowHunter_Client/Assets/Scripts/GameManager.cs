using Assets.Scripts.MainMenuUI.Accounts;
using Assets.Scripts.MainMenuUI.SearchGame;
using EventSystem;
using Kernel.Settings;
using Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class GameManager : MonoBehaviour
    {
        static bool emulServer = true;
        static bool EventLogger = true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnBeforeSceneLoadRuntimeMethod()
        {
            EventView.Load();
            SettingManager.Load();
            Language.Init();
            GAccount.Init();
            GRoom.Init();
            if (EventLogger)
            {
                EventView.Manager.AddListener(new EventLogger());
            }
            if (emulServer)
            {
                ServerInterface.ServerTestEmul.Init();
            }
        }

        private void OnApplicationQuit()
        {
            SettingManager.Save();
        }

        private void Update()
        {
            EventView.Manager.ExecMainThreaded();
        }
    }

    class EventLogger : IListener<EventSystem.Event>
    {
        public void OnEvent(EventSystem.Event e, string[] tags = null)
        {
            Debug.Log(e.GetType().FullName + "\n\n" + e.Serialize());
        }
    }
}
