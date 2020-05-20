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
        /// le nombre de joueur dans la partie
        /// </summary>
        public static int NbPlayer { get; private set; }

        /// <summary>
        /// Initialise les joueurs.
        /// </summary>
        /// <param name="nbPlayers">Le nombre de joueurs de la partie</param>
        /// <param name="realPlayers">Nombre de vrais joueurs</param>
        /// <param name="withExtension">Activation ou non de l'extension</param>
        public static void Init(int nbPlayers, int realPlayers, bool withExtension)
        {
            NbPlayer = nbPlayers;
            gPlayer = new GPlayer(nbPlayers, realPlayers, withExtension);


            foreach (Player p in GetPlayers())
            {
                if (p.Character.goal != null)
                    p.Character.goal.setWinningListeners(p);
                if (p.Character.power != null)
                    p.Character.power.addListeners(p);
            }
        }

        public static void Clean()
        {
            gPlayer = null;
            NbPlayer = -1;
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
