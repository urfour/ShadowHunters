using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EventSystemTest
{
    public class LogWarningEvent : LogEvent
    {
        public LogWarningEvent() : base()
        {

        }

        public LogWarningEvent(string msg) : base(msg)
        {

        }
    }
}
