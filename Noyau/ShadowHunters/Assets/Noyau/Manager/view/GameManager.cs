using Assets.Noyau.Cards.view;
using Assets.Noyau.Players.view;
using Scripts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Manager.view
{
    public static class GameManager
    {
        public static Setting<Player> PlayerTurn { get; private set; } = new Setting<Player>(null);
        public static Setting<bool> StartOfTurn { get; private set; } = new Setting<bool>(true);
        public static Setting<bool> MovementAvailable { get; private set; } = new Setting<bool>(false);
        public static Setting<bool> AttackAvailable { get; private set; } = new Setting<bool>(false);

        public static Setting<bool> PickVisionDeck { get; private set; } = new Setting<bool>(false);
        public static Setting<bool> PickDarknessDeck { get; private set; } = new Setting<bool>(false);
        public static Setting<bool> PickLightnessDeck { get; private set; } = new Setting<bool>(false);

        public static Setting<bool> TurnEndable { get; private set; } = new Setting<bool>(false);

        public static void Init(int nbPlayers)
        {
            PlayerView.Init(nbPlayers);
            CardView.Init();
        }
    }
}
