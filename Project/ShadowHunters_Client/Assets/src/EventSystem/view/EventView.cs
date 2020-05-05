using EventSystem.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem
{
    static class EventView
    {
        public static IEventManager Manager { get; private set; }/* = new EventManager();*/

        /// <summary>
        /// Instancie le Manager.
        /// </summary>
        public static void Load()
        {
            Manager = new controller.EventManager();
        }
    }
}
