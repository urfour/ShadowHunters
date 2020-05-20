using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Noyau.Players.controller
{
    /// <summary>
    /// Classe qui va instancier tout les joueurs.
    /// </summary>
    class GPlayer
    {
        public Player[] Players { get; private set; }

        /// <summary>
        /// Fonction qui va instancier chaque joueur avec un personnage différent.
        /// </summary>
        /// <param name="nbPlayers">Le nombre de joueurs</param>
        /// <param name="realPlayers">Nombre de vrais joueurs</param>
        /// <param name="withExtension">Activation ou non de l'extension</param>
        public GPlayer(int nbPlayers, int realPlayers, bool withExtension)
        {
            GCharacter characters = new GCharacter(nbPlayers, withExtension);

            Players = new Player[nbPlayers];
            for (int i = 0; i < realPlayers; i++)
            {
                Players[i] = new Player(i, characters.PickCharacter(), false);
            }
            for (int i = realPlayers; i < nbPlayers; i++)
            {
                Players[i] = new Player(i, characters.PickCharacter(), true);
            }
        }
    }
}
