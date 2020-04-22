using Assets.Noyau.Players.model;
using Assets.Noyau.Players.view;
using Assets.Noyau.Manager.view;
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
                        if (p.Character.team.Equals(CharacterTeam.Hunter) && !p.Dead.Value)
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

        public static Goal AllieGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    if(!owner.Dead.Value)
                    {
                        foreach (Player p in PlayerView.GetPlayers())
                        {
                            if(p.HasWon.Value)
                                owner.HasWon.Value=true;
                        }
                    }
                },
                setWinningListeners: (owner) =>
                {
                    foreach (Player p in PlayerView.GetPlayers())
                    {
                        p.HasWon.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                    }
                }
            );

        public static Goal BobGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    if(owner.NbEquipment.Value >= 5)
                        owner.HasWon.Value=true;
                },
                setWinningListeners: (owner) =>
                {
                    owner.NbEquipment.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                }
            );

        public static Goal CharlesGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    int nbDead=0;
                    foreach (Player p in PlayerView.GetPlayers())
                    {
                        if (!p.Dead.Value)
                        {
                            nbDead++;
                        }
                        
                    }
                    if(nbDead>=3 && GameManager.HasKilled.Value)
                        owner.HasWon.Value=true;
                },
                setWinningListeners: (owner) =>
                {
                    GameManager.HasKilled.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                    foreach (Player p in PlayerView.GetPlayers())
                    {
                        p.Dead.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                    }
                }
            );

        public static Goal DanielGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    bool ShadowAlive = false;
                    int nbDead = 0;
                    foreach (Player p in PlayerView.GetPlayers())
                    {
                        if (p.Character.team.Equals(CharacterTeam.Shadow) && !p.Dead.Value)
                        {
                            ShadowAlive = true;
                            break;
                        }
                        if (p.Dead.Value)
                        {
                            nbDead++;
                        }
                    }
                    bool status = !ShadowAlive || (nbDead <= 1 && owner.Dead.Value);
                    if (status != owner.HasWon.Value)
                    {
                        owner.HasWon.Value = status;
                    }
                },
                setWinningListeners: (owner) =>
                {
                    foreach (Player p in PlayerView.GetPlayers())
                    {
                        p.Dead.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                    }
                }
            );
    }
}
