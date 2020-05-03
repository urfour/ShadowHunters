using Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EventSystem
{
    public delegate void OnNotification(ListenableObject sender);

    public class ListenableObject
    {
        private List<OnNotification> observers = new List<OnNotification>();

        public virtual void AddListener(OnNotification listener)
        {
            if (listener == null) Debug.LogWarning("Adding null listener");
            else observers.Add(listener);
        }
        public virtual void RemoveListener(OnNotification listener)
        {
            observers.Remove(listener);
        }

        /// <summary>
        /// Notifie tous les observateurs. Si l'un engendre une exception, les suivants ne seront pas notifié et l'exception sera transmise à l'appelant
        /// </summary>
        public virtual void Notify()
        {
            foreach (OnNotification o in observers)
            {
                if (o != null)
                {
                    try
                    {
                        o(this);
                    }
                    catch (MissingReferenceException)
                    {
                        // rien à faire ou alors supprimer l'observateur de la liste
                    }
                }
                else
                {
                    Debug.LogWarning("null observator");
                }
            }
        }

        /// <summary>
        /// Notifie avec un catch sur MissingReferenceException provenant de UnityEngine
        /// </summary>
        public virtual void TryNotify()
        {
            foreach (OnNotification o in observers)
            {
                try
                {
                    o(this);
                }
                catch (MissingReferenceException)
                {
                    // rien à faire ou alors supprimer l'observateur de la liste
                }
            }
        }
    }
}
