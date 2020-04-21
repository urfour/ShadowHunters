using Assets.Noyau.Players.model;
using Assets.Noyau.Players.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Players.controller
{
    public static class GGoal
    {
        public static Goal HunterGoal = new Goal
            (
                checkWinning: (owner) =>
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
                    bool status = !shadowAlive;
                    if (status != owner.HasWon.Value)
                    {
                        owner.HasWon.Value = status;
                    }
                },
                setWinningListeners: (owner) =>
                {
                    foreach (Player p in PlayerView.GetPlayers())
                    {
                        if (p.Character.team.Equals(CharacterTeam.Shadow))
                        {
                            p.Dead.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                        }
                    }
                }
            );

        public static Goal ShadowGoal = new Goal
            (
                checkWinning: (owner) =>
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
                    bool status = !HunterAlive || nbNeutralDead >= 3;
                    if (status != owner.HasWon.Value)
                    {
                        owner.HasWon.Value = status;
                    }
                },
                setWinningListeners: (owner) =>
                {
                    foreach (Player p in PlayerView.GetPlayers())
                    {
                        if (!p.Character.team.Equals(CharacterTeam.Shadow))
                        {
                            p.Dead.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                        }
                    }
                }
            );
    }
}
