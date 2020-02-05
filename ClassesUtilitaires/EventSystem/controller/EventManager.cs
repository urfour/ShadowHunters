using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventSystem;
using UnityEngine;

namespace EventSystem.controller
{
    internal class ListenerInstance
    {
        public Type Type { get; private set; }
        //public IListener<> listener { get; private set; }
        public object Listener { get; private set; }

        public ListenerInstance(object listener)
        {
            Type = listener.GetType();
            this.Listener = listener;
        }

        public void OnEvent(Event e, string[] tags = null)
        {
            Type.GetMethod("OnEvent").Invoke(Listener, new object[] { e , tags });
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


        public void AddListener<T>(IListener<T> listener) where T : Event
        {
            ListenerInstance li = new ListenerInstance(listener);
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
            if (listeners.Count == 0)
            {
                Debug.LogWarning("Empty listeners list : " + e.GetType());
            }
            foreach (ListenerInstance l in listeners)
            {
                l.OnEvent(e, tags);
            }
        }

    }
}
