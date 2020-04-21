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

        public static void Init(int nbPlayers)
        {
            PlayerView.Init(nbPlayers);
            CardView.Init();
        }
    }
}
