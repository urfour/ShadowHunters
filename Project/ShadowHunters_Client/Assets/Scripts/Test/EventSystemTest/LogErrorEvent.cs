using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EventSystemTest
{
    public class LogErrorEvent : LogEvent
    {
        public LogErrorEvent() : base()
        {

        }

        public LogErrorEvent(string msg) : base(msg)
        {

        }
    }
}
