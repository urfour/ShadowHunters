using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EventSystemTest
{
    public class LogEvent : Event
    {
        public string Msg { get; set; }

        public LogEvent()
        {

        }

        public LogEvent(string msg)
        {
            this.Msg = msg;
        }
    }
}
