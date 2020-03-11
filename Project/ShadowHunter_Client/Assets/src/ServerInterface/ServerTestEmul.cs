using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerInterface.AuthEvents;

namespace ServerInterface
{
    class ServerTestEmul
    {
        public static void Init()
        {
            EventView.Manager.AddListener(new AuthTestEmul());
        }
    }
}
