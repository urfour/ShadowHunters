using Assets.Noyau.Players.view;
using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Players.model
{
    public delegate bool CheckWinningCondition(Player owner);
    public delegate void SetWinningListeners(Player owner);

    public static class WinningConditionFunction
    {
        public static void Hunter_listeners(Player owner)
        {
            foreach (Player p in PlayerView.GetPlayers())
            {
                if (p.Character.team.Equals(CharacterTeam.Shadow))
                {
                    p.Dead.AddListener((sender) => { owner.CheckWon(); });
                }
            }
        }
        public static bool Hunter(Player owner)
        {
            bool shadowAlive = false;
            foreach (Player p in PlayerView.GetPlayers())
            {
                if (p.Character.team.Equals(CharacterTeam.Shadow) && !p.Dead.Value)
                {
                    shadowAlive = true;
                    break;
                }
            }
            return !shadowAlive;
        }

        public static void Shadow_listeners(Player owner)
        {
            foreach (Player p in PlayerView.GetPlayers())
            {
                if (!p.Character.team.Equals(CharacterTeam.Shadow))
                {
                    p.Dead.AddListener((sender) => { owner.CheckWon(); });
                }
            }
        }
        public static bool Shadow(Player owner)
        {
            bool HunterAlive = false;
            int nbNeutralDead = 0;
            foreach (Player p in PlayerView.GetPlayers())
            {
                if (p.Character.team.Equals(CharacterTeam.Shadow) && !p.Dead.Value)
                {
                    HunterAlive = true;
                    break;
                }
                if (p.Character.team.Equals(CharacterTeam.Neutral) && p.Dead.Value)
                {
                    nbNeutralDead++;
                }
            }
            return !HunterAlive || nbNeutralDead >= 3;
        }
    }
}
