using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventSystem;

namespace EventSystem.controller
{
    internal class ListenerInstance
    {
        public Type Type { get; private set; }
        //public IListener<> listener { get; private set; }
        public object Listener { get; private set; }
        public bool MainThreaded { get; private set; } = false;

        public ListenerInstance(object listener, bool mainThreaded = false)
        {
            Type = listener.GetType();
            this.Listener = listener;
            this.MainThreaded = mainThreaded;
        }

        public void OnEvent(Event e, string[] tags = null)
        {
            Type.GetMethod("OnEvent").Invoke(Listener, new object[] { e , tags });
        }
    }

    internal class WaitingLaunchEvent
    {
        public ListenerInstance Listener { get; private set; }
        public Event Event { get; private set; }
        public string[] Tags { get; private set; }

        public WaitingLaunchEvent(ListenerInstance listener, Event e, string[] tags){
            this.Listener = listener;
            this.Event = e;
            this.Tags = tags;
        }
    }

    class EventManager : IEventManager
    {

        /**
         * Dictionnary that contains all listeners for every subtype of IEvent
         */
        internal Dictionary<Type, List<ListenerInstance>> Listeners { get; set; } = new Dictionary<Type, List<ListenerInstance>>();


        /**
         * Contains all active Listeners
         */
        internal List<ListenerInstance> AllListeners { get; set; } = new List<ListenerInstance>();

        internal Mutex listenersMutex = new Mutex();

        internal Queue<WaitingLaunchEvent> EventsToLaunch { get; private set; } = new Queue<WaitingLaunchEvent>();


        public Log Log { get ; set; }
        public Log LogWarning { get; set; }
        public Log LogError { get; set; }

        internal Mutex eventsToLaunch_Mutex = new Mutex();

        public void AddListener<T>(IListener<T> listener, bool MainThreaded = false) where T : Event
        {
            ListenerInstance li = new ListenerInstance(listener, MainThreaded);
            AllListeners.Add(li);
            Type listenedType = li.Type.GetInterface("IListener`1").GetGenericArguments()[0];
            //Type listenedType = T;
            //Debug.Log("listened : " + listenedType);
            //bool found = false;
            foreach (KeyValuePair<Type, List<ListenerInstance>> pair in Listeners)
            {
                if (listenedType.IsAssignableFrom(pair.Key))
                {
                    pair.Value.Add(li);
                    //Debug.Log("AddListener : <" + listener.GetType() + "> added to <" + listenedType + "> event");
                    //found = true;
                }
            }
            //throw new NotImplementedException();
        }


        public void RemoveListener<T>(IListener<T> listener) where T : Event
        {
            int i = AllListeners.FindIndex((item) => item.Listener == listener);
            ListenerInstance li = AllListeners[i];
            AllListeners.RemoveAt(i);

            foreach (KeyValuePair<Type,List<ListenerInstance>> pair in Listeners)
            {
                pair.Value.Remove(li);
            }
        }

        public void Emit(Event e, params string[] tags)
        {
            listenersMutex.WaitOne();
            if (!Listeners.ContainsKey(e.GetType())) // si c'est la première fois qu'un type d'event est envoyé, on recherche tous les listeners existant qui lisent ce type d'event
            {
                List<ListenerInstance> matchListeners = new List<ListenerInstance>(); // liste de tous les listeners qui écoutent un type assignable au type de 'e'
                //Debug.Log("number of listeners : " + AllListeners.Count);
                foreach (ListenerInstance listener in AllListeners) // parcours de tous les listeners qui n'ont pas encore de type d'event compatible dans le dictionnaire Listeners
                {
                    Type listenedType = listener.Type.GetInterface("IListener`1").GetGenericArguments()[0]; // le type lu par le listener
                    //Debug.Log("compare : " + listenedType + " with " + e.GetType() + " : " + (listenedType.IsAssignableFrom(e.GetType())));
                    if (listenedType.IsAssignableFrom(e.GetType()))
                    {
                        matchListeners.Add(listener);
                    }
                }
                //Debug.Log("Add " + matchListeners.Count + " elements for " + e.GetType());
                Listeners.Add(e.GetType(), matchListeners);
            }


            // récupération de la liste des listeners qui écoutent le type de l'evenement, puis appel de la fonction OnEvent(e) pour tous ces listeners
            List<ListenerInstance> listeners = Listeners[e.GetType()];
            listenersMutex.ReleaseMutex();
            if (listeners.Count == 0)
            {
                Logger.Warning("Empty listeners list : " + e.GetType());
            }
            foreach (ListenerInstance l in listeners)
            {
                if (l.MainThreaded)
                {
                    eventsToLaunch_Mutex.WaitOne();
                    EventsToLaunch.Enqueue(new WaitingLaunchEvent(l, e, tags));
                    eventsToLaunch_Mutex.ReleaseMutex();
                }
                else
                {
                    l.OnEvent(e, tags);
                }
            }
        }

        public void ExecMainThreaded()
        {
            if (EventsToLaunch.Count > 0)
            {
                eventsToLaunch_Mutex.WaitOne();
                foreach (WaitingLaunchEvent e in EventsToLaunch)
                {
                    e.Listener.OnEvent(e.Event, e.Tags);
                }
                EventsToLaunch.Clear();
                eventsToLaunch_Mutex.ReleaseMutex();
            }
        }
    }
}
