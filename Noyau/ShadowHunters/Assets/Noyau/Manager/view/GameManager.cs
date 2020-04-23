﻿using Assets.Noyau.Cards.view;
using Assets.Noyau.Players.view;
using Scripts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static Setting<Player> PlayerTurn { get; private set; } = new Setting<Player>(null);
        public static Setting<bool> StartOfTurn { get; private set; } = new Setting<bool>(true);
        public static Setting<bool> MovementAvailable { get; private set; } = new Setting<bool>(false);
        public static Setting<bool> AttackAvailable { get; private set; } = new Setting<bool>(false);

        public static Setting<bool> PickVisionDeck { get; private set; } = new Setting<bool>(false);
        public static Setting<bool> PickDarknessDeck { get; private set; } = new Setting<bool>(false);
        public static Setting<bool> PickLightnessDeck { get; private set; } = new Setting<bool>(false);

        public static Setting<bool> HasKilled {get; private set; } = new Setting<bool>(false);

        public static Setting<bool> TurnEndable { get; private set; } = new Setting<bool>(false);

        public static Dictionary<int, Position> Board { get; private set; } = new Dictionary<int, Position>();

        public static void Init(int nbPlayers)
        {
            PlayerView.Init(nbPlayers);
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

            Random r = new Random();

            int index;

            for (int i = 0; i < 6; i++)
            {
                index = r.Next(0, p.Count);
                Board.Add(i, p[index]);
                p.RemoveAt(index);
            }
        }
    }
}
