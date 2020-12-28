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

        public void AddListener(OnNotification listener)
        {
            observers.Add(listener);
        }
        public void RemoveListener(OnNotification listener)
        {
            observers.Remove(listener);
        }

        public void Notify()
        {
            foreach (OnNotification o in observers)
            {
                o(this);
            }
        }
    }
}

