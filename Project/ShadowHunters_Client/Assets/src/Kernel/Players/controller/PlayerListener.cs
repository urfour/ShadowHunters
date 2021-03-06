﻿using Assets.Noyau.Cards.controller;
using Assets.Noyau.Cards.model;
using Assets.Noyau.Cards.view;
using Assets.Noyau.event_in;
using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.view;
using Assets.src.Kernel.event_in;
using EventSystem;
using Log;
using Scripts;
using Scripts.event_in;
using Scripts.event_out;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Noyau.Players.controller
{
    class PlayerListener : IListener<PlayerEvent>
    {
        public void OnEvent(PlayerEvent e, string[] tags = null)
        {
            if (e is FirstTurnEvent fte)
            {
                GameManager.TurnEndable.Value = false;
                GameManager.MovementAvailable.Value = true;
                GameManager.StartOfTurn.Value = true;
                int nbRealPlayers = 0;
                foreach (Player p in PlayerView.GetPlayers())
                {
                    if (!(p is Bot))
                    {
                        nbRealPlayers++;
                    }
                }
                GameManager.PlayerTurn.Value = PlayerView.GetPlayer(GameManager.rand.Next(0, nbRealPlayers));
                GameManager.WaitingPlayer.Value = GameManager.PlayerTurn.Value;
                KernelLog.Instance.StartTurn();
                if (GameManager.PlayerTurn.Value is Bot && GameManager.LocalPlayer.Value.Id == 0)
                {
                    WaitHandler.Instance.BotChoice(2.0f,
                        new AskMovement() { PlayerId = GameManager.PlayerTurn.Value.Id });
                }

            }
            /// <summary>
            /// déroulement de l'évènement de fin de tour
            /// </summary>
            if (e is EndTurnEvent ete)
            {
                GameManager.EndOfTurn.Value = false;
                GameManager.AttackAvailable.Value = false;
                GameManager.AttackDone.Value = false;

                if (GameManager.PlayerTurn.Value == null)
                {
                    GameManager.PlayerTurn.Value = PlayerView.GetPlayer(GameManager.rand.Next(0, PlayerView.NbPlayer));
                }
                else if (GameManager.PlayerTurn.Value.HasAncestral.Value) // si le joueur a utilisé le savoir ancestral, le joueur suivant reste lui
                {
                    KernelLog.Instance.Replay(GameManager.PlayerTurn.Value);
                    if (GameManager.PlayerTurn.Value.ReplayTimes.Value <= 1)
                    {
                        GameManager.PlayerTurn.Value.HasAncestral.Value = false;
                        GameManager.PlayerTurn.Value.ReplayTimes.Value = 0;
                    }
                    else
                    {
                        GameManager.PlayerTurn.Value.ReplayTimes.Value--;
                    }
                }
                else
                {
                    GameManager.PlayerTurn.Value = PlayerView.NextPlayer(GameManager.PlayerTurn.Value);
                }

                if (GameManager.PlayerTurn.Value.HasGuardian.Value)
                    GameManager.PlayerTurn.Value.HasGuardian.Value = false;

                GameManager.TurnEndable.Value = false;
                GameManager.MovementAvailable.Value = true;
                GameManager.StartOfTurn.Value = true;
                GameManager.PickDarknessDeck.Value = false;
                GameManager.PickLightnessDeck.Value = false;
                GameManager.PickVisionDeck.Value = false;
                GameManager.WaitingPlayer.Value = GameManager.PlayerTurn.Value;

                KernelLog.Instance.StartTurn();
                if (GameManager.PlayerTurn.Value is Bot && GameManager.LocalPlayer.Value.Id == 0)
                {
                    WaitHandler.Instance.BotChoice(2.0f,
                        new AskMovement() { PlayerId = GameManager.PlayerTurn.Value.Id });
                }


            }

            /// <summary>
            /// déroulement de l'évènement de choix du déplacement à effectuer
            /// </summary>
            else if (e is AskMovement am)
            {
                GameManager.StartOfTurn.Value = false;
                GameManager.MovementAvailable.Value = false;

                // la gestion de cet événement est uniquement fait pour le client qui l'envoie
                if ((GameManager.PlayerTurn.Value is Bot && GameManager.LocalPlayer.Value.Id != GameManager.BotHandler.Value.Id)
                    || (!(GameManager.PlayerTurn.Value is Bot) && GameManager.LocalPlayer.Value != null && GameManager.LocalPlayer.Value.Id != e.PlayerId))
                {
                    return;
                }

                List<int> dicesRolls = new List<int>();

                int nbrolls = 1;

                if (GameManager.PlayerTurn.Value.HasCompass.Value)
                {
                    nbrolls++;
                }

                List<int> availableDestination = new List<int>();

                while (nbrolls > 0)
                {
                    int val = UnityEngine.Random.Range(1, 6) + UnityEngine.Random.Range(1, 4);

                    int tmpavailableDestination = -1;

                    switch (val)
                    {
                        case 2:
                        case 3:
                            tmpavailableDestination = GameManager.Board.FirstOrDefault(x => x.Value == Position.Antre).Key;
                            break;
                        case 4:
                        case 5:
                            tmpavailableDestination = GameManager.Board.FirstOrDefault(x => x.Value == Position.Porte).Key;
                            break;
                        case 6:
                            tmpavailableDestination = GameManager.Board.FirstOrDefault(x => x.Value == Position.Monastere).Key;
                            break;
                        case 7:
                            availableDestination = new List<int>() { 0, 1, 2, 3, 4, 5 };
                            nbrolls = 0;
                            break;
                        case 8:
                            tmpavailableDestination = GameManager.Board.FirstOrDefault(x => x.Value == Position.Cimetiere).Key;
                            break;
                        case 9:
                            tmpavailableDestination = GameManager.Board.FirstOrDefault(x => x.Value == Position.Foret).Key;
                            break;
                        case 10:
                            tmpavailableDestination = GameManager.Board.FirstOrDefault(x => x.Value == Position.Sanctuaire).Key;
                            break;
                    }
                    if (!availableDestination.Contains(tmpavailableDestination) && GameManager.PlayerTurn.Value.Position.Value != tmpavailableDestination)
                    {
                        if (tmpavailableDestination != -1)
                        {
                            availableDestination.Add(tmpavailableDestination);

                            if (GameManager.PlayerTurn.Value.Character.characterName.Equals("character.name.emi") && GameManager.PlayerTurn.Value.Revealed.Value && GameManager.PlayerTurn.Value.Position.Value != -1)
                            {
                                if (GameManager.PlayerTurn.Value.Position.Value % 2 == 0)
                                {
                                    availableDestination.Add((GameManager.PlayerTurn.Value.Position.Value + 7) % 6);
                                }
                                else
                                {
                                    availableDestination.Add((GameManager.PlayerTurn.Value.Position.Value + 5) % 6);
                                }
                            }
                         

                            nbrolls--;
                        }
                    }
                }
                availableDestination.Remove(GameManager.PlayerTurn.Value.Position.Value);

                EventView.Manager.Emit(new SelectMovement()
                {
                    PlayerId = GameManager.PlayerTurn.Value.Id,
                    LocationAvailable = availableDestination.ToArray()
                });

                if (GameManager.PlayerTurn.Value is Bot)
                {
                    WaitHandler.Instance.BotChoice(1.0f, new MoveOn()
                    {
                        PlayerId = GameManager.PlayerTurn.Value.Id,
                        Location = availableDestination[UnityEngine.Random.Range(0, availableDestination.Count - 1)]
                    });
                }
            }

            /// <summary>
            /// déroulement de l'évènement de déplacement
            /// </summary>
            else if (e is MoveOn mo)
            {
                Player currentPlayer = PlayerView.GetPlayer(mo.PlayerId);
                currentPlayer.Position.Value = mo.Location;

                GameManager.AttackAvailable.Value = true;
                GameManager.EndOfTurn.Value = true;

                if (!currentPlayer.HasSaber.Value || currentPlayer.getTargetablePlayers().Count == 0)
                    GameManager.TurnEndable.Value = true;

                switch (GameManager.Board[currentPlayer.Position.Value])
                {
                    case Position.Antre:
                        GameManager.PickVisionDeck.Value = true;
                        if (GameManager.PlayerTurn.Value is Bot && GameManager.LocalPlayer.Value.Id == GameManager.BotHandler.Value.Id)
                        {
                            WaitHandler.Instance.BotChoice(2.0f, new DrawCardEvent()
                            {
                                PlayerId = GameManager.PlayerTurn.Value.Id,
                                SelectedCardType = CardType.Vision
                            });
                        }
                        break;
                    case Position.Porte:
                        GameManager.PickVisionDeck.Value = true;
                        GameManager.PickLightnessDeck.Value = true;
                        GameManager.PickDarknessDeck.Value = true;
                        if (GameManager.PlayerTurn.Value is Bot && GameManager.LocalPlayer.Value.Id == GameManager.BotHandler.Value.Id)
                        {
                            int cardChoosen = UnityEngine.Random.Range(1, 3);
                            CardType cardChoosenType = CardType.Darkness;
                            switch (cardChoosen)
                            {
                                case 1:
                                    cardChoosenType = CardType.Vision;
                                    break;
                                case 2:
                                    cardChoosenType = CardType.Darkness;
                                    break;
                                case 3:
                                    cardChoosenType = CardType.Light;
                                    break;
                            }
                            WaitHandler.Instance.BotChoice(2.0f, new DrawCardEvent()
                            {
                                PlayerId = GameManager.PlayerTurn.Value.Id,
                                SelectedCardType = cardChoosenType
                            });
                        }
                        break;
                    case Position.Monastere:
                        GameManager.PickLightnessDeck.Value = true;
                        if (GameManager.PlayerTurn.Value is Bot && GameManager.LocalPlayer.Value.Id == GameManager.BotHandler.Value.Id)
                        {
                            WaitHandler.Instance.BotChoice(2.0f, new DrawCardEvent()
                            {
                                PlayerId = GameManager.PlayerTurn.Value.Id,
                                SelectedCardType = CardType.Light
                            });
                        }
                        break;
                    case Position.Cimetiere:
                        GameManager.PickDarknessDeck.Value = true;
                        if (GameManager.PlayerTurn.Value is Bot && GameManager.LocalPlayer.Value.Id == GameManager.BotHandler.Value.Id)
                        {
                            WaitHandler.Instance.BotChoice(2.0f, new DrawCardEvent()
                            {
                                PlayerId = GameManager.PlayerTurn.Value.Id,
                                SelectedCardType = CardType.Darkness
                            });
                        }
                        break;
                    case Position.Foret:
                        if (!(GameManager.LocalPlayer.Value != null && GameManager.LocalPlayer.Value.Id != e.PlayerId)
                            || !(GameManager.PlayerTurn.Value is Bot && GameManager.LocalPlayer.Value.Id != GameManager.BotHandler.Value.Id))
                            EventView.Manager.Emit(new SelectUsableCardPickedEvent(CardView.GCard.Foret.Id, false, e.PlayerId));
                        break;
                    case Position.Sanctuaire:
                        if (!(GameManager.LocalPlayer.Value != null && GameManager.LocalPlayer.Value.Id != e.PlayerId)
                            || !(GameManager.PlayerTurn.Value is Bot && GameManager.LocalPlayer.Value.Id != GameManager.BotHandler.Value.Id))
                            EventView.Manager.Emit(new SelectUsableCardPickedEvent(CardView.GCard.Sanctuaire.Id, false, e.PlayerId));
                        break;
                }
                KernelLog.Instance.MoveOn();
            }

            /// <summary>
            /// déroulement de l'évènement de pioche d'une carte
            /// </summary>
            else if (e is DrawCardEvent drawCard)
            {
                GameManager.AttackAvailable.Value = false;
                GameManager.EndOfTurn.Value = false;

                switch (drawCard.SelectedCardType)
                {
                    case CardType.Vision:
                        if (!GameManager.PickVisionDeck.Value) return;
                        break;
                    case CardType.Light:
                        if (!GameManager.PickLightnessDeck.Value) return;
                        break;
                    case CardType.Darkness:
                        if (!GameManager.PickDarknessDeck.Value) return;
                        break;
                }
                GameManager.PickDarknessDeck.Value = false;
                GameManager.PickLightnessDeck.Value = false;
                GameManager.PickVisionDeck.Value = false;


                GameManager.TurnEndable.Value = false;
                Player player = PlayerView.GetPlayer(drawCard.PlayerId);
                GameManager.WaitingPlayer.Value = player;
                Card pickedCard = null;

                Debug.Log("Joueur qui pioche : " + player.Name);
                Debug.Log("Deck selectionné : " + drawCard.SelectedCardType);

                switch (drawCard.SelectedCardType)
                {
                    case CardType.Darkness:
                        pickedCard = CardView.PickDarkness();
                        break;
                    case CardType.Light:
                        pickedCard = CardView.PickLight();
                        break;
                    case CardType.Vision:
                        pickedCard = CardView.PickVision();
                        break;
                }
                KernelLog.Instance.DrawCard(player, pickedCard.Id, pickedCard.cardType == CardType.Vision);

                Debug.Log("Carte piochée : " + pickedCard.cardLabel);
                Debug.Log("Equipement ? " + (pickedCard is EquipmentCard));

                if (pickedCard is UsableCard pickedUsableCard)
                {
                    if (!pickedUsableCard.canDismiss)
                    {
                        GameManager.TurnEndable.Value = false;
                        GameManager.AttackAvailable.Value = false;
                    }
                    if (!((GameManager.PlayerTurn.Value is Bot && GameManager.LocalPlayer.Value.Id != GameManager.BotHandler.Value.Id)
                    || (!(GameManager.PlayerTurn.Value is Bot) && GameManager.LocalPlayer.Value != null && GameManager.LocalPlayer.Value.Id != e.PlayerId)))
                    {
                        EventView.Manager.Emit(new SelectUsableCardPickedEvent(pickedUsableCard.Id, pickedUsableCard.cardType == CardType.Vision, e.PlayerId));
                    }
                }
                else if (pickedCard is EquipmentCard pickedEquipmentCard)
                {
                    if (!((GameManager.PlayerTurn.Value is Bot && GameManager.LocalPlayer.Value.Id != GameManager.BotHandler.Value.Id)
                        || (!(GameManager.PlayerTurn.Value is Bot) && GameManager.LocalPlayer.Value != null && GameManager.LocalPlayer.Value.Id != e.PlayerId)))
                    {
                        EventView.Manager.Emit(new DrawEquipmentCardEvent(player.Id, pickedEquipmentCard.Id));
                    }
                    pickedEquipmentCard.equipe(player, pickedEquipmentCard);
                    if (!player.HasSaber.Value || (player.HasSaber.Value && player.getTargetablePlayers().Count == 0))
                    {
                        GameManager.TurnEndable.Value = true;
                        GameManager.WaitingPlayer.Value = null;
                    }
                    GameManager.AttackAvailable.Value = true;
                    if (GameManager.PlayerTurn.Value is Bot
                        && GameManager.LocalPlayer.Value.Id == GameManager.BotHandler.Value.Id)
                    {
                        List<Player> targetablePlayers = GameManager.PlayerTurn.Value.getTargetablePlayers();
                        if (targetablePlayers.Count > 0 && GameManager.AttackAvailable.Value)
                        {
                            int randomPlayerId = UnityEngine.Random.Range(0, targetablePlayers.Count - 1);
                            WaitHandler.Instance.BotChoice(2.0f, new AttackPlayerEvent()
                            {
                                PlayerId = GameManager.PlayerTurn.Value.Id,
                                PlayerAttackedId = targetablePlayers[randomPlayerId].Id
                            });
                        }
                        else if (GameManager.TurnEndable.Value)
                        {
                            WaitHandler.Instance.BotChoice(3.0f, new EndTurnEvent()
                            {
                                PlayerId = GameManager.PlayerTurn.Value.Id
                            });
                        }
                    }
                }
            }
            /// <summary>
            /// déroulement de l'évènement d'utilisation d'une carte à usage unique
            /// </summary>
            else if (e is UsableCardUseEvent ecue)
            {
                Card c = CardView.GCard.cards[ecue.Cardid];
                int effect = ecue.EffectSelected;
                Player p1 = PlayerView.GetPlayer(ecue.PlayerId);
                Player p2 = PlayerView.GetPlayer(ecue.PlayerSelected);
                UsableCard uCard = c as UsableCard;

                if (!GameManager.StartOfTurn.Value)
                {
                    GameManager.AttackAvailable.Value = true;
                    if (!GameManager.PlayerTurn.Value.HasSaber.Value || (GameManager.PlayerTurn.Value.HasSaber.Value &&
                        GameManager.PlayerTurn.Value.getTargetablePlayers().Count == 0))
                    {
                        GameManager.TurnEndable.Value = true;
                        GameManager.WaitingPlayer.Value = null;
                    }
                    else
                    {
                        GameManager.TurnEndable.Value = false;
                    }
                }

                if (uCard.cardEffect[effect].targetableCondition == null || uCard.cardEffect[effect].targetableCondition(p2, p1))
                {
                    uCard.cardEffect[effect].effect(p2, p1, uCard);
                }
                GameManager.EndOfTurn.Value = true;

                if (GameManager.PlayerTurn.Value is Bot
                    && GameManager.LocalPlayer.Value.Id == GameManager.BotHandler.Value.Id)
                {
                    List<Player> targetablePlayers = GameManager.PlayerTurn.Value.getTargetablePlayers();
                    if (targetablePlayers.Count > 0 && GameManager.AttackAvailable.Value)
                    {
                        int randomPlayerId = UnityEngine.Random.Range(0, targetablePlayers.Count - 1);
                        WaitHandler.Instance.BotChoice(2.0f, new AttackPlayerEvent()
                        {
                            PlayerId = GameManager.PlayerTurn.Value.Id,
                            PlayerAttackedId = targetablePlayers[randomPlayerId].Id
                        });
                    }
                    else if (GameManager.TurnEndable.Value)
                    {
                        WaitHandler.Instance.BotChoice(3.0f, new EndTurnEvent()
                        {
                            PlayerId = GameManager.PlayerTurn.Value.Id
                        });
                    }
                }
            }

            /// <summary>
            /// déroulement de l'évènement d'attaque d'un joueur
            /// </summary>
            else if (e is AttackPlayerEvent attackPlayer)
            {
                GameManager.PickVisionDeck.Value = false;
                GameManager.PickLightnessDeck.Value = false;
                GameManager.PickDarknessDeck.Value = false;

                Player playerAttacking = PlayerView.GetPlayer(attackPlayer.PlayerId);
                Player playerAttacked = PlayerView.GetPlayer(attackPlayer.PlayerAttackedId);

                Debug.Log("Joueur attaquant : " + playerAttacking.Name);
                Debug.Log("Joueur attaqué : " + playerAttacked.Name);

                if (attackPlayer.PowerLoup)
                {
                    playerAttacked.Wounded(Math.Abs(GameManager.rand.Next(1, 6) - GameManager.rand.Next(1, 4)), playerAttacking, true);
                }
                else if (attackPlayer.PowerFranklin)
                {
                    playerAttacked.Wounded(GameManager.rand.Next(0, 6), playerAttacking, true);
                }
                else if (attackPlayer.PowerGeorges)
                {
                    playerAttacked.Wounded(GameManager.rand.Next(0, 4), playerAttacking, true);
                }
                else if (attackPlayer.PowerCharles)
                {
                    int lancer = 0;
                    if (playerAttacking.HasSaber.Value)
                        lancer = GameManager.rand.Next(1, 4);
                    else
                        lancer = Math.Abs(GameManager.rand.Next(1, 6) - GameManager.rand.Next(1, 4));

                    Debug.Log("Le lancer vaut : " + lancer);

                    Logger.Info("Wounds avant : " + playerAttacked.Wound.Value);
                    playerAttacked.Wounded(lancer + playerAttacking.BonusAttack.Value - playerAttacking.MalusAttack.Value, playerAttacking, true);
                    Logger.Info("Wounds après : " + playerAttacked.Wound.Value);
                    Logger.Info("Vie total : " + playerAttacked.Character.characterHP);
                    Logger.Info("Mort ? " + playerAttacked.Dead.Value);
                }
                else
                {
                    int lancer = 0;
                    int damages = 0;

                    if (playerAttacking.HasSaber.Value ||
                       (playerAttacking.Character.characterName == "character.name.valkyrie" && playerAttacking.Revealed.Value))
                        lancer = GameManager.rand.Next(1, 4);
                    else
                        lancer = Math.Abs(GameManager.rand.Next(1, 6) - GameManager.rand.Next(1, 4));

                    Debug.Log("Le lancer vaut : " + lancer);

                    if (playerAttacking.Character.characterName.Equals("character.name.bob"))
                    {
                        if (playerAttacked.HasGuardian.Value)
                            damages = 0;
                        else if (lancer > 0)
                            damages = Mathf.Max(0, (lancer - playerAttacked.ReductionWounds.Value < 0) ? 0 : lancer - playerAttacked.ReductionWounds.Value
                                                + playerAttacking.BonusAttack.Value - playerAttacking.MalusAttack.Value);

                        if (damages >= 2)
                        {
                            // Cas Gatling à considérer quand on aura la foi
                            damages += playerAttacked.ReductionWounds.Value;
                            playerAttacking.OnAttackingPlayer.Value = playerAttacked.Id;
                            playerAttacked.OnAttackedBy.Value = playerAttacking.Id;
                            playerAttacking.DamageDealed.Value = damages;

                        }
                        else
                        {
                            if (playerAttacking.HasGatling.Value)
                            {
                                foreach (Player p in playerAttacking.getTargetablePlayers())
                                {
                                    p.Wounded(lancer + playerAttacking.BonusAttack.Value - playerAttacking.MalusAttack.Value, playerAttacking, true);
                                }
                            }
                            else
                            {
                                Logger.Info("Wounds avant : " + playerAttacked.Wound.Value);
                                playerAttacked.Wounded(lancer + playerAttacking.BonusAttack.Value - playerAttacking.MalusAttack.Value, playerAttacking, true);
                                Logger.Info("Wounds après : " + playerAttacked.Wound.Value);
                                Logger.Info("Vie total : " + playerAttacked.Character.characterHP);
                                Logger.Info("Mort ? " + playerAttacked.Dead.Value);
                            }
                        }
                    }

                    else if (lancer == 0)
                    {
                        playerAttacked.OnAttackedBy.Value = playerAttacking.Id;
                        KernelLog.Instance.AttackFailed(playerAttacking, playerAttacked);
                    }
                    else
                    {
                        if (playerAttacking.HasGatling.Value)
                        {
                            foreach (Player p in playerAttacking.getTargetablePlayers())
                            {
                                p.Wounded(lancer + playerAttacking.BonusAttack.Value - playerAttacking.MalusAttack.Value, playerAttacking, true);
                            }
                        }
                        else
                        {
                            Logger.Info("Wounds avant : " + playerAttacked.Wound.Value);
                            playerAttacked.Wounded(lancer + playerAttacking.BonusAttack.Value - playerAttacking.MalusAttack.Value, playerAttacking, true);
                            Logger.Info("Wounds après : " + playerAttacked.Wound.Value);
                            Logger.Info("Vie total : " + playerAttacked.Character.characterHP);
                            Logger.Info("Mort ? " + playerAttacked.Dead.Value);
                        }
                    }
                    GameManager.TurnEndable.Value = true;
                    GameManager.WaitingPlayer.Value = null;

                    GameManager.AttackDone.Value = true;
                    GameManager.AttackAvailable.Value = false;

                    if (GameManager.PlayerTurn.Value is Bot
                        && GameManager.LocalPlayer.Value.Id == GameManager.BotHandler.Value.Id)
                    {
                        WaitHandler.Instance.BotChoice(1.0f, new EndTurnEvent()
                        {
                            PlayerId = GameManager.PlayerTurn.Value.Id
                        });
                    }
                }
            }

            /// <summary>
            /// déroulement de l'évènement de révélation de son personnage
            /// </summary>
            else if (e is RevealCardEvent reveal)
            {
                Player p = PlayerView.GetPlayer(reveal.PlayerId);

                p.Revealed.Value = true;

                if (p.HasSpear.Value == true && p.Character.team == CharacterTeam.Hunter)
                    p.BonusAttack.Value += 2;

                Debug.Log("Le joueur " + p.Name + " se révèle.");
            }

            /// <summary>
            /// déroulement de l'évènement de choix de si on se révèle ou non
            /// </summary>
            else if (e is RevealOrNotEvent rone)
            {
                Player player = PlayerView.GetPlayer(rone.PlayerId);
                Card card = rone.EffectCard;
                bool revealed = rone.HasRevealed;
                bool powerLoup = rone.PowerLoup;

                if (revealed)
                {
                    if (card.cardLabel == "card.darkness.darkness_rituel"
                        || card.cardLabel == "card.light.light_supreme"
                        || card.cardLabel == "card.light.light_chocolat")
                        player.Healed(player.Wound.Value);
                }
            }

            /// <summary>
            /// déroulement de l'évènement d'utilisation du pouvoir de son personnage
            /// </summary>
            else if (e is PowerUseEvent pue)
            {
                Player p = PlayerView.GetPlayer(pue.PlayerId);
                if (p.CanUsePower.Value)
                {
                    p.Character.power.power(p);
                }
            }
            /// <summary>
            /// déroulement de l'évènement de pioche d'une carte pour un bot
            /// </summary>
            else if (e is SelectUsableCardPickedEvent sucpe)
            {
                Player player = PlayerView.GetPlayer(sucpe.PlayerId);
                UsableCard card = (UsableCard)CardView.GetCard(sucpe.CardId);
                if (player is Bot
                            && GameManager.LocalPlayer.Value.Id == GameManager.BotHandler.Value.Id)
                {
                    List<(int ceIndex, Player player)> choices = new List<(int, Player)>();
                    for (int i = 0; i < card.cardEffect.Length; i++)
                    {
                        CardEffect ce = card.cardEffect[i];
                        if (ce.targetableCondition == null)
                        {
                            choices.Add((i, player));
                        }
                        else
                        {
                            foreach (Player selectedPlayer in PlayerView.GetPlayers())
                            {
                                if (ce.targetableCondition(selectedPlayer, player))
                                {
                                    choices.Add((i, selectedPlayer));
                                }
                            }
                        }
                    }
                    int index = UnityEngine.Random.Range(0, choices.Count - 1);
                    CardEffect effect = card.cardEffect[choices[index].ceIndex];
                    Player p = choices[index].player;
                    if (effect.targetableCondition == null || effect.targetableCondition(p, player))
                    {
                        WaitHandler.Instance.BotChoice(2.0f, new UsableCardUseEvent(card.Id, choices[index].ceIndex, p.Id, player.Id));
                    }
                }
            }
        }

    }

}
