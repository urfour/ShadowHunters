using Assets.Noyau.Cards.view;
using Assets.Noyau.Players.controller;
using Assets.Noyau.Players.view;
using Assets.src.Kernel.Players.controller;
using EventSystem;
using Kernel.Settings;
using Scripts;
using Scripts.event_in;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Noyau.Manager.view
{

    public enum Position
    {
        None,
        Antre,
        Cimetiere,
        Foret,
        Monastere,
        Porte,
        Sanctuaire
    }

    public static class GameManager
    {
        private static PlayerListener playerListener = null;
        private static DisconnectionListener disconnectionListener = null;

        private static System.Random Rand;
        private static int nbRandCall = 0;

        public static System.Random rand
        {
            get
            {
                nbRandCall++;
                Logger.Comment("rand call " + nbRandCall + " \n" + Environment.StackTrace);
                return Rand;
            }
            set
            {
                Rand = value;
            }
        }

        public static Setting<Player> LocalPlayer { get; private set; } = new Setting<Player>(null);

        /// <summary>
        /// Propriété d'accès au joueur dont c'est le tour.
        /// </summary>
        public static Setting<Player> PlayerTurn { get; private set; } = new Setting<Player>(null);
        /// <summary>
        /// Booléen qui annonce si c'est le début du tour.
        /// </summary>
        public static Setting<bool> StartOfTurn { get; private set; } = new Setting<bool>(true);
        /// <summary>
        /// Booléen qui annonce si le déplacement du personnage est possible.
        /// </summary>
        public static Setting<bool> MovementAvailable { get; private set; } = new Setting<bool>(false);
        /// <summary>
        /// Booléen qui annonce si l'action d'attaquer est possible.
        /// </summary>
        public static Setting<bool> AttackAvailable { get; private set; } = new Setting<bool>(false);
        /// <summary>
        /// Booléen qui annonce si l'on peut piocher une carte Vision.
        /// </summary>

        public static Setting<bool> PickVisionDeck { get; private set; } = new Setting<bool>(false);
        /// <summary>
        /// Booléen qui annonce si l'on peut piocher une carte Ténèbre.
        /// </summary>
        public static Setting<bool> PickDarknessDeck { get; private set; } = new Setting<bool>(false);
        /// <summary>
        /// Booléen qui annonce si l'on peut piocher une carte Lumière.
        /// </summary>
        public static Setting<bool> PickLightnessDeck { get; private set; } = new Setting<bool>(false);
        /// <summary>
        /// Booléen qui annonce si l'on a tué un autre personnage.
        /// </summary>

        public static Setting<bool> HasKilled {get; private set; } = new Setting<bool>(false);
        /// <summary>
        /// Booléen qui annonce si l'on peut terminer le tour.
        /// </summary>

        public static Setting<bool> TurnEndable { get; private set; } = new Setting<bool>(false);


        public static Setting<Player> WaitingPlayer { get; private set; } = new Setting<Player>();

        public static int PlayerAttackedByBob = -1;
        public static int DamageDoneByBob = -1;
        /// <summary>
        /// Dictionnaire de tuple pour le terrain du jeu.
        /// </summary>
        public static Dictionary<int, Position> Board { get; private set; } = new Dictionary<int, Position>();

        public static Setting<bool> GameEnded { get; private set; } = new Setting<bool>(false);

        /// <summary>
        /// Initialise l'ensemble du jeu.
        /// </summary>
        /// <param name="nbPlayers">Le nombre de joueurs de la partie</param>
        public static void Init(int nbPlayers, int randSeed, int localPlayer = -1)
        {
            playerListener = new PlayerListener();
            disconnectionListener = new DisconnectionListener();
            EventView.Manager.AddListener(playerListener, true);
            EventView.Manager.AddListener(disconnectionListener);

            rand = new System.Random(randSeed);
            
            PlayerView.Init(nbPlayers);
            if (localPlayer != -1)
            {
                LocalPlayer.Value = PlayerView.GetPlayer(localPlayer);
            }
            CardView.Init();

            List<Position> p = new List<Position>()
            {
                Position.Antre,
                Position.Cimetiere,
                Position.Foret,
                Position.Monastere,
                Position.Porte,
                Position.Sanctuaire
            };
            

            int index;

            for (int i = 0; i < 6; i++)
            {
                index = rand.Next(0, p.Count);
                Board.Add(i, p[index]);
                p.RemoveAt(index);
            }


            foreach (Player player in PlayerView.GetPlayers())
            {
                player.Character.goal.setWinningListeners(player);
                OnNotification gameEnded = (sender) =>
                {
                    if (player.HasWon.Value && !GameEnded.Value)
                    {
                        GameEnded.Value = true;
                    }
                };
                player.HasWon.AddListener(gameEnded);

                OnNotification playerDisconnect = (sender) =>
                {
                    if (GameManager.PlayerTurn.Value == player && GameManager.LocalPlayer.Value == PlayerView.NextPlayer(player))
                    {
                        EventView.Manager.Emit(new EndTurnEvent(player.Id));
                    }
                    else if (GameManager.WaitingPlayer.Value == player)
                    {
                        GameManager.TurnEndable.Value = true;
                    }
                };

                player.Disconnected.AddListener(playerDisconnect);
            }
        }

        public static void Clean()
        {
            EventView.Manager.RemoveListener(playerListener);
            EventView.Manager.RemoveListener(disconnectionListener);
            PlayerView.Clean();
            CardView.Clean();
            rand = null;
        }
    }
}
