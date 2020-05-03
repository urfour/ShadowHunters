using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem
{
    public delegate void OnNotification(ListenableObject sender);

    public class ListenableObject
    {
        private List<OnNotification> observers = new List<OnNotification>();

        public virtual void AddListener(OnNotification listener)
        {
            observers.Add(listener);
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
                o(this);
            }
        }
    }
}
