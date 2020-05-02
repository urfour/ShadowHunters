using Assets.Noyau.Players.view;
using Assets.Scripts.MainMenuUI.SearchGame;
using EventSystem;
using Network.events;
using ServerInterface.RoomEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.src.Kernel.Players.controller
{
    class DisconnectionListener : IListener<RoomDataEvent>
    {
        public void OnEvent(RoomDataEvent e, string[] tags = null)
        {
            if (e.RoomData.Code == GRoom.Instance.JoinedRoom.RawData.Code)
            {
                foreach (Player p in PlayerView.GetPlayers())
                {
                    if (!e.RoomData.Players.Contains(p.Name))
                    {
                        p.Disconnected.Value = true;
                        p.Dead.Value = true;
                        break;
                    }
                }
            }
        }
    }

    //class 
}
