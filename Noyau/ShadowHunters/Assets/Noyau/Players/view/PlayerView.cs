using Assets.Noyau.Players.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Players.view
{
    public static class PlayerView
    {
        private static GPlayer gPlayer;

        public static void Init(int nbPlayers)
        {
            NbPlayer = nbPlayers;
            gPlayer = new GPlayer(nbPlayers);
        }

        public static Player GetPlayer(int id)
        {
            return gPlayer.Players[id];
        }

        public static Player[] GetPlayers()
        {
            return gPlayer.Players;
        }

        public static int NbPlayer { get; private set; }

        public static Player NextPlayer(Player currentPlayer)
        {
            if (!gPlayer.Players[(currentPlayer.Id + 1) % NbPlayer].Dead.Value)
            {
                return gPlayer.Players[(currentPlayer.Id + 1) % NbPlayer];
            }
            else return NextPlayer(gPlayer.Players[(currentPlayer.Id + 1) % NbPlayer]);
        }
    }
}
