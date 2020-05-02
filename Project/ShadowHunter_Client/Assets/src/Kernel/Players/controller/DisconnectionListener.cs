using Assets.Noyau.Players.view;
using EventSystem;
using Network.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.src.Kernel.Players.controller
{
    class DisconnectionListener : IListener<NetworkDisconnectedEvent>
    {
        public void OnEvent(NetworkDisconnectedEvent e, string[] tags = null)
        {
            foreach (Player p in PlayerView.GetPlayers())
            {
                if (p.Name == e.Account.Login)
                {
                    p.Disconnected.Value = true;
                    p.Dead.Value = true;
                    break;
                }
            }
        }
    }
}
