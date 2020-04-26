using Assets.Noyau.Players.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Noyau.Players.view
{
    public static class PlayerView
    {
        private static GPlayer gPlayer;

        /// <summary>
        /// Initialise les joueurs.
        /// </summary>
        /// <param name="nbPlayers">Le nombre de joueurs de la partie</param>
        public static void Init(int nbPlayers)
        {
            NbPlayer = nbPlayers;
            gPlayer = new GPlayer(nbPlayers);
        }

        /// <summary>
        /// Fonction qui renvoie le joueur en fonction de son Id.
        /// </summary>
        /// <param name="id">Id du joueur</param>
        public static Player GetPlayer(int id)
        {
            return gPlayer.Players[id];
        }

        /// <summary>
        /// Fonction qui renvoie tout les joueurs.
        /// </summary>
        public static Player[] GetPlayers()
        {
            return gPlayer.Players;
        }

        public static int NbPlayer { get; private set; }

        /// <summary>
        /// Fonction qui renvoie le joueur pour le prochain tour.
        /// </summary>
        /// <param name="currentPlayer">Joueur du tour actuel</param>
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
