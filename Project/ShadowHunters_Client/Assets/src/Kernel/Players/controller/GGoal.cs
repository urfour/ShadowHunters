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
    /// <summary>
    /// Classe qui va instancier toute les conditions de victoire des personnages du jeu.
    /// </summary>
    public static class GGoal
    {
        /// <summary>
        /// Fonction qui va instancier la condition de victoire des personnages de l'équipe Hunter.
        /// </summary>
        /// <param name="checkWinning">Fonction qui test la condition de victoire</param>
        /// <param name="setWinningListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        public static Goal HunterGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    if (owner.HasWon.Value || GameManager.GameEnded.Value) return;
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

        /// <summary>
        /// Fonction qui va instancier la condition de victoire des personnages de l'équipe Shadow.
        /// </summary>
        /// <param name="checkWinning">Fonction qui test la condition de victoire</param>
        /// <param name="setWinningListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        public static Goal ShadowGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    if (owner.HasWon.Value || GameManager.GameEnded.Value) return;
                    bool HunterAlive = false;
                    int nbNeutralDead = 0;
                    foreach (Player p in PlayerView.GetPlayers())
                    {
                        if (p.Character.team.Equals(CharacterTeam.Hunter) && !p.Dead.Value)
                        {
                            HunterAlive = true;
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

        /// <summary>
        /// Fonction qui va instancier la condition de victoire d'Allie.
        /// </summary>
        /// <param name="checkWinning">Fonction qui test la condition de victoire</param>
        /// <param name="setWinningListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        public static Goal AllieGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    if (owner.HasWon.Value || GameManager.GameEnded.Value) return;
                    if (!owner.Dead.Value && !owner.HasWon.Value)
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
                        if (p.Id != owner.Id)
                        {
                            p.HasWon.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                        }
                    }
                }
            );

        /// <summary>
        /// Fonction qui va instancier la condition de victoire de Bob.
        /// </summary>
        /// <param name="checkWinning">Fonction qui test la condition de victoire</param>
        /// <param name="setWinningListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        public static Goal BobGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    if (owner.HasWon.Value || GameManager.GameEnded.Value) return;
                    if (owner.NbEquipment.Value >= 5)
                        owner.HasWon.Value=true;
                },
                setWinningListeners: (owner) =>
                {
                    owner.NbEquipment.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                }
            );

        /// <summary>
        /// Fonction qui va instancier la condition de victoire de Charles.
        /// </summary>
        /// <param name="checkWinning">Fonction qui test la condition de victoire</param>
        /// <param name="setWinningListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        public static Goal CharlesGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    if (owner.HasWon.Value || GameManager.GameEnded.Value) return;
                    int nbDead=0;
                    foreach (Player p in PlayerView.GetPlayers())
                    {
                        if (p.Dead.Value)
                        {
                            nbDead++;
                        }
                    }
                    // impossible pour Charles de gagner avec la condition normale à 5 joueurs
                    if (PlayerView.NbPlayer == 5)
                    {
                        if (nbDead >= 3 && GameManager.HasKilled.Value && GameManager.PlayerTurn.Value == owner)
                            owner.HasWon.Value = true;
                    }
                    else
                    {
                        if (nbDead > 3 && GameManager.HasKilled.Value && GameManager.PlayerTurn.Value == owner)
                            owner.HasWon.Value = true;
                    }
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

        /// <summary>
        /// Fonction qui va instancier la condition de victoire de Daniel.
        /// </summary>
        /// <param name="checkWinning">Fonction qui test la condition de victoire</param>
        /// <param name="setWinningListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        public static Goal DanielGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    if (owner.HasWon.Value || GameManager.GameEnded.Value) return;
                    bool ShadowAlive = false;
                    int nbDead = 0;
                    foreach (Player p in PlayerView.GetPlayers())
                    {
                        if (p.Character.team.Equals(CharacterTeam.Shadow) && !p.Dead.Value)
                        {
                            ShadowAlive = true;
                        }
                        if (p.Dead.Value)
                        {
                            nbDead++;
                        }
                    }
                    bool status = (!ShadowAlive && !owner.Dead.Value) || (nbDead <= 1 && owner.Dead.Value);
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

        /// <summary>
        /// Fonction qui va instancier la condition de victoire de Bryan.
        /// </summary>
        /// <param name="checkWinning">Fonction qui test la condition de victoire</param>
        /// <param name="setWinningListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        public static Goal BryanGoal = new Goal
            (
                 checkWinning: (owner) =>
                 {
                     if (owner.HasWon.Value || owner.Position.Value == -1) return;
                     if (GameManager.GameEnded.Value && GameManager.Board[owner.Position.Value] == Position.Sanctuaire)
                     {
                         owner.HasWon.Value = true;
                     }
                     else if (GameManager.HasKilled.Value && GameManager.PlayerTurn.Value == owner 
                              && PlayerView.GetPlayer(owner.OnAttackingPlayer.Value).Character.characterHP >= 13)
                     {
                         owner.HasWon.Value = true;
                     }
                 },
                 setWinningListeners: (owner) =>
                 {
                     GameManager.HasKilled.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                     foreach (Player p in PlayerView.GetPlayers())
                     {
                         if (p.Id != owner.Id)
                         {
                             p.HasWon.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                         }
                     }
                 }
            );

        /// <summary>
        /// Fonction qui va instancier la condition de victoire de Catherine.
        /// </summary>
        /// <param name="checkWinning">Fonction qui test la condition de victoire</param>
        /// <param name="setWinningListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        public static Goal CatherineGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    if (owner.HasWon.Value || GameManager.GameEnded.Value) return;
                    int nbDead = 0;
                    foreach (Player p in PlayerView.GetPlayers())
                    {
                        if (p.Dead.Value)
                        {
                            nbDead++;
                        }
                    }
                    bool status = (nbDead <= 1 && owner.Dead.Value) || ((PlayerView.GetPlayers().Length - nbDead) == 2 && !owner.Dead.Value);
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

        /// <summary>
        /// Fonction qui va instancier la condition de victoire de Agnes.
        /// </summary>
        /// <param name="checkWinning">Fonction qui test la condition de victoire</param>
        /// <param name="setWinningListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        public static Goal AgnesGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    if (owner.HasWon.Value || GameManager.GameEnded.Value) return;
                    if (PlayerView.GetPlayer((owner.Id + 1) % PlayerView.GetPlayers().Length).HasWon.Value)
                    {
                        owner.HasWon.Value = true;
                    }
                },
                setWinningListeners: (owner) =>
                {
                    PlayerView.GetPlayer((owner.Id + 1) % PlayerView.GetPlayers().Length).HasWon.AddListener(
                        (sender) => { owner.Character.goal.checkWinning(owner); });
                }
            );

        public static Goal AgnesGoalPower = new Goal
            (
                checkWinning: (owner) =>
                {
                    if (owner.HasWon.Value || GameManager.GameEnded.Value) return;
                    if (PlayerView.GetPlayer((owner.Id - 1) % PlayerView.GetPlayers().Length).HasWon.Value)
                    {
                        owner.HasWon.Value = true;
                    }
                },
                setWinningListeners: (owner) =>
                {
                    PlayerView.GetPlayer((owner.Id - 1) % PlayerView.GetPlayers().Length).HasWon.AddListener(
                        (sender) => { owner.Character.goal.checkWinning(owner); });
                }
            );

        /// <summary>
        /// Fonction qui va instancier la condition de victoire de David.
        /// </summary>
        /// <param name="checkWinning">Fonction qui test la condition de victoire</param>
        /// <param name="setWinningListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        public static Goal DavidGoal = new Goal
            (
                checkWinning: (owner) =>
                {
                    if (owner.HasWon.Value || GameManager.GameEnded.Value) return;
                    int nbStuff = 0;
                    if (owner.HasCrucifix.Value)
                        nbStuff++;
                    if (owner.HasAmulet.Value)
                        nbStuff++;
                    if (owner.HasSpear.Value)
                        nbStuff++;
                    if (owner.HasToge.Value)
                        nbStuff++;

                    if (nbStuff >= 3)
                    {
                        owner.HasWon.Value = true;
                    }

                },
                setWinningListeners: (owner) =>
                {
                    owner.HasCrucifix.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                    owner.HasAmulet.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                    owner.HasSpear.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                    owner.HasToge.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                }
            );
    }
}
