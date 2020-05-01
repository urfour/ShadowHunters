using Assets.Scripts.GameUI;
using Assets.Scripts.MainMenuUI.Accounts;
using Assets.Scripts.MainMenuUI.SearchGame;
using EventSystem;
using IO;
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
        static bool emulServer = false;
        static bool EventLogger = true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnBeforeSceneLoadRuntimeMethod()
        {
            TextAsset[] langs = Resources.LoadAll<TextAsset>("AppData/Lang");
            foreach (TextAsset t in langs)
            {
                Debug.Log(t.name);
                if (!IOSystem.FileExists(IOSystem.GetFullPath("Lang/" + t.name + ".txt")))
                {
                    IOSystem.CreateFile("Lang/" + t.name + ".txt", t.text.Split('\n'));
                }
            }

            ResourceLoader.Load();
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
            else
            {
                ServerInterface.Network.NetworkView.Connect();
            }
        }

        public List<GameObject> forceStartCall;

        private void Start()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.quitting += OnApplicationQuit;
#endif
            DontDestroyOnLoad(gameObject);
            foreach(GameObject o in forceStartCall)
            {
                o.SetActive(true);
            }
        }

        private void OnApplicationQuit()
        {
            SettingManager.Save();
            ServerInterface.Network.NetworkView.Disconnect();
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
            if (tags != null)
            {
                Debug.Log(e.GetType().FullName + " [tags=" + tags[0] +"]\n\n" + e.Serialize());
            }
            else
            {
                Debug.Log(e.GetType().FullName + " [tag=null]\n\n" + e.Serialize());
            }
        }
    }
}
