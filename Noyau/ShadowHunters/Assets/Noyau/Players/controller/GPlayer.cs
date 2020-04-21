using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Players.controller
{
    class GPlayer
    {
        public Player[] Players { get; private set; }

        public GPlayer(int nbPlayers)
        {

            GCharacter characters = new GCharacter(nbPlayers);

            Players = new Player[nbPlayers];
            for (int i = 0; i < nbPlayers; i++)
            {
                Players[i] = new Player(i, characters.PickCharacter());
            }

            foreach (Player p in Players)
            {
                p.Character.goal.setWinningListeners(p);
                p.Character.power.addListeners(p);
            }
        }
        
    }
}
