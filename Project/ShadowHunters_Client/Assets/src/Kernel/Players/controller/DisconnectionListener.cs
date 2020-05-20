using Assets.Noyau.Manager.view;
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
    /// <summary>
    /// En cas de déconnexion d'un joueur, il meurt.
    /// </summary>
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
                        if (!p.Dead.Value)
                        {
                            p.Dead.Value = true;
                        }
                        if (GameManager.BotHandler.Value == p)
                        {
                            GameManager.BotHandler.Value = PlayerView.GetPlayer(p.Id + 1);
                        }
                        break;
                    }
                }
            }
        }
    }
}
