using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Log
{
    class UnityLog : ILogger
    {
        public void Comment(string msg)
        {
            Debug.Log("[COMMENT] " + msg);
        }

        public void Error(string msg)
        {
            Debug.LogError("[ERROR] " + msg);
        }

        public void Error(Exception msg)
        {
            Debug.LogError("[ERROR] " + msg + " " + msg.Message + " " + msg.TargetSite);
        }

        public void Indent(int amount = 1)
        {
            return;
        }

        public void Info(string msg)
        {
            Debug.Log("[INFO] " + msg);
        }

        public void Warning(string msg)
        {
            Debug.LogWarning("[WARNING] " + msg);
        }
    }
}
