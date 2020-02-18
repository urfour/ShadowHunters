using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.EventSystemTest
{
    class LogListener : IListener<LogEvent>
    {
        public void OnEvent(LogEvent e, string[] tags = null)
        {
            Debug.Log("LogListener : " + e.Msg);
        }
    }
}
