using UnityEngine;
using System.Collections;
using EventSystem;
using Assets.Scripts.EventSystemTest;
using System.Threading;

namespace Test.EventSystemU
{
    public class EventSystemTestManager : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnBeforeSceneLoadRuntimeMethod()
        {
            EventView.Load();
            EventView.Manager.LogError = (msg) => { Debug.LogWarning(msg); };
        }

        private void OnApplicationQuit()
        {
            this.stop = true;
            this.thread.Abort();
            this.thread.Join();
        }

        public bool stop = false;
        public bool SimulerTypeEventInconnu = false;
        public bool MainThreadedListener = true;

        System.Random rand = new System.Random();
        Thread thread = null;

        // Use this for initialization
        void Start()
        {
            LogListener l = new LogListener();
            EventView.Manager.AddListener(l, MainThreaded:MainThreadedListener);

            EventSystem.Event e = new LogEvent("hello world");
            string s = e.Serialize();
            if (SimulerTypeEventInconnu)
            {
                s = s.Substring(1);
            }
            Debug.Log("serialize : " + s);
            e = EventSystem.Event.Deserialize(s);
            if (e != null)
                EventView.Manager.Emit(e);

            EventView.Manager.RemoveListener(l);

            this.thread = new Thread(() => { this.EventEmiter(); });
            this.thread.Start();
        }

        // Update is called once per frame
        void Update()
        {
            EventView.Manager.ExecMainThreaded();
        }

        private void EventEmiter()
        {
            Debug.Log("Event Emiter started");
            while (!stop)
            {
                System.Threading.Thread.Sleep(rand.Next(500, 3000));
                LogEvent e = null;
                switch (rand.Next(0, 3))
                {
                    case 0:
                        e = new LogWarningEvent("Warning : hello world : " + rand.Next(0, 1000));
                        break;
                    case 1:
                        e = new LogErrorEvent("Error : hello world : " + rand.Next(0, 1000));
                        break;
                    case 2:
                        e = new LogEvent("Info : hello world : " + rand.Next(0, 1000));
                        break;
                    default:
                        break;
                }
                Debug.Log("Emit(new " + e.GetType().Name + "(" + e.Msg + ");");
                EventView.Manager.Emit(e);
            }
            Debug.Log("Event Emiter stopped");
        }
    }
}
