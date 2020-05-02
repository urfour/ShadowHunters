using Assets.Noyau.Cards.controller;
using Assets.Noyau.Cards.model;
using Assets.Noyau.Cards.view;
using Assets.Noyau.event_in;
using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.view;
using Assets.src.Kernel.event_in;
using EventSystem;
using Scripts;
using Scripts.event_in;
using Scripts.event_out;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Noyau.Players.controller
{
    class PlayerListener : IListener<PlayerEvent>
    {
        public void OnEvent(PlayerEvent e, string[] tags = null)
        {
            if (e is EndTurnEvent ete)
            {

                GameManager.AttackAvailable.Value = false;

                if (GameManager.PlayerTurn.Value == null)
                {
                    //GameManager.PlayerTurn.Value = PlayerView.GetPlayer(PlayerView.NbPlayer -1);
                    GameManager.PlayerTurn.Value = PlayerView.GetPlayer(GameManager.rand.Next(0, PlayerView.NbPlayer));
                }
                else if (GameManager.PlayerTurn.Value.HasAncestral.Value) // si le joueur a utilisé le savoir ancestral, le joueur suivant reste lui
                {
                    GameManager.PlayerTurn.Value.HasAncestral.Value = false;
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
                /*
                EventView.Manager.Emit(new SelectedNextPlayer()
                {
                    PlayerId = GameManager.PlayerTurn.Value.Id
                });
                */
            }
            else if (e is AskMovement am)
            {
                GameManager.StartOfTurn.Value = false;
                GameManager.MovementAvailable.Value = false;

                // la gestion de cet événement est uniquement fait pour le client qui l'envoie
                if (GameManager.LocalPlayer.Value != null && GameManager.LocalPlayer.Value.Id != e.PlayerId) 
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
                            availableDestination = new List<int>() {0, 1, 2, 3, 4, 5};
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
                                    availableDestination.Add(GameManager.PlayerTurn.Value.Position.Value + 1);
                                else
                                    availableDestination.Add(GameManager.PlayerTurn.Value.Position.Value - 1);
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
            }
            else if (e is MoveOn mo)
            {
                Player currentPlayer = PlayerView.GetPlayer(mo.PlayerId);
                currentPlayer.Position.Value = mo.Location;

                GameManager.AttackAvailable.Value = true;

                if (!currentPlayer.HasSaber.Value || currentPlayer.getTargetablePlayers().Count == 0)
                    GameManager.TurnEndable.Value = true;

                switch (GameManager.Board[currentPlayer.Position.Value])
                {
                    case Position.Antre:
                        GameManager.PickVisionDeck.Value = true;
                        break;
                    case Position.Porte:
                        GameManager.PickVisionDeck.Value = true;
                        GameManager.PickLightnessDeck.Value = true;
                        GameManager.PickDarknessDeck.Value = true;
                        break;
                    case Position.Monastere:
                        GameManager.PickLightnessDeck.Value = true;
                        break;
                    case Position.Cimetiere:
                        GameManager.PickDarknessDeck.Value = true;
                        break;
                    case Position.Foret:
                        if (!(GameManager.LocalPlayer.Value != null && GameManager.LocalPlayer.Value.Id != e.PlayerId))
                            EventView.Manager.Emit(new SelectUsableCardPickedEvent(CardView.GCard.Foret.Id, false, e.PlayerId));
                        break;
                    case Position.Sanctuaire:
                        if (!(GameManager.LocalPlayer.Value != null && GameManager.LocalPlayer.Value.Id != e.PlayerId))
                            EventView.Manager.Emit(new SelectUsableCardPickedEvent(CardView.GCard.Sanctuaire.Id, false, e.PlayerId));
                        break;
                }
            }
            /*
            else if (e is ForestSelectTargetEvent fste)
            {
                // la gestion de cet événement est uniquement fait pour le client qui l'envoie
                if (GameManager.LocalPlayer.Value != null && GameManager.LocalPlayer.Value.Id != e.PlayerId)
                {
                    return;
                }
                List<int> target = new List<int>();

                foreach (Player p in PlayerView.GetPlayers())
                {
                    if (!p.Dead.Value)
                    {
                        if (fste.Hurt)
                            if (!p.HasBroche.Value)
                                target.Add(p.Id);
                            else
                                continue;
                        else
                            target.Add(p.Id);
                    }
                }
                int wounds;
                if (fste.Hurt)
                    wounds = 2;
                else
                    wounds = -1;

                EventView.Manager.Emit(new SelectPlayerTakingWoundsEvent()
                {
                    PlayerId = fste.PlayerId,
                    PossibleTargetId = target.ToArray(),
                    IsPuppet = false,
                    NbWoundsTaken = wounds,
                    NbWoundsSelfHealed = 0
                });
            }
            */
            else if (e is DrawCardEvent drawCard/* && GameManager.PlayerTurn.Value.Id == e.PlayerId*/)
            {
                // la gestion de cet événement est uniquement fait pour le client qui l'envoie
                GameManager.PickDarknessDeck.Value = false;
                GameManager.PickLightnessDeck.Value = false;
                GameManager.PickVisionDeck.Value = false;


                GameManager.TurnEndable.Value = false;
                Player player = PlayerView.GetPlayer(drawCard.PlayerId);
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

                Debug.Log("Carte piochée : " + pickedCard.cardLabel);
                Debug.Log("Equipement ? " + (pickedCard is EquipmentCard));

                if (pickedCard is UsableCard pickedUsableCard)
                {
                    if (!pickedUsableCard.canDismiss)
                    {
                        GameManager.TurnEndable.Value = false;
                    }
                    if (!(GameManager.LocalPlayer.Value != null && GameManager.LocalPlayer.Value.Id != e.PlayerId))
                        EventView.Manager.Emit(new SelectUsableCardPickedEvent(pickedUsableCard.Id, pickedUsableCard.cardType == CardType.Vision, e.PlayerId));
                }
                else if (pickedCard is EquipmentCard pickedEquipmentCard)
                {
                    if (!(GameManager.LocalPlayer.Value != null && GameManager.LocalPlayer.Value.Id != e.PlayerId))
                        EventView.Manager.Emit(new DrawEquipmentCardEvent(player.Id, pickedEquipmentCard.Id));
                    pickedEquipmentCard.equipe(player, pickedEquipmentCard);
                    if(!player.HasSaber.Value)
                        GameManager.TurnEndable.Value = true;
                }
            }
            else if (e is UsableCardUseEvent ecue)
            {
                Card c = CardView.GCard.cards[ecue.Cardid];
                int effect = ecue.EffectSelected;
                Player p1 = PlayerView.GetPlayer(ecue.PlayerId);
                Player p2 = PlayerView.GetPlayer(ecue.PlayerSelected);

                if (!GameManager.StartOfTurn.Value)
                {
                    if (!p1.HasSaber.Value || p1 != GameManager.PlayerTurn.Value || (p1.HasSaber.Value && p1.getTargetablePlayers().Count == 0))
                        GameManager.TurnEndable.Value = true;
                }

                UsableCard uCard = c as UsableCard;
                /*
                if (uCard.cardType != CardType.Vision)
                    p2 = p1;
                */

                if (uCard.cardEffect[effect].targetableCondition == null || uCard.cardEffect[effect].targetableCondition(p2, p1))
                {
                    Debug.Log("L'effet s'active !");
                    uCard.cardEffect[effect].effect(p2, p1, uCard);
                }
                else
                    Debug.Log("L'effet ne s'active pas !");

            }
            /*
            else if (e is AttackEvent attack)
            {
                Debug.Log("ID attaquant : " + attack.PlayerId);
                Player player = PlayerView.GetPlayer(attack.PlayerId);

                Debug.Log("Joueur attaquant : " + player.Name);

                List<Player> targetablePlayers = player.getTargetablePlayers();

                Debug.Log("Nombres joueurs attaquables : " + targetablePlayers.Count);
                foreach (Player p in targetablePlayers)
                    Debug.Log("Joueurs attaquables : " + p.Name);

                if (targetablePlayers.Count != 0)
                {
                    Debug.Log("Gatling ? " + player.HasGatling.Value);

                    if (!player.HasGatling.Value)
                    {
                        List<int> playersId = new List<int>();
                        foreach (Player p in targetablePlayers)
                            if (!p.Dead.Value && p.Id != player.Id)
                                playersId.Add(p.Id);

                        Debug.Log("Envoi de SelectAttackTargetEvent");

                        EventView.Manager.Emit(new SelectAttackTargetEvent()
                        {
                            PlayerId = player.Id,
                            PossibleTargetId = playersId.ToArray(),
                            PowerFranklin = false,
                            PowerGeorges = false
                        });
                    }
                    else
                    {
                        int lancer = 0;

                        if (player.HasSaber.Value)
                            lancer = GameManager.rand.Next(1, 4);
                        else
                            lancer = Math.Abs(GameManager.rand.Next(1, 6) - GameManager.rand.Next(1, 4));

                        Debug.Log("On attaque tout le monde !");

                        Debug.Log("Lancer : " + lancer);

                        if (lancer != 0)
                            foreach (Player p in targetablePlayers)
                            {
                                Debug.Log("Degat de " + p.Name + " avant : " + p.Wound.Value);
                                p.Wounded(lancer + player.BonusAttack.Value - player.MalusAttack.Value, player, true);
                                Debug.Log("Degat de " + p.Name + " avant : " + p.Wound.Value);
                            }
                    }
                }
                else
                {
                    Debug.Log("Il n'y a personne à attaquer !");
                }
            }
            */
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
                    playerAttacked.Wounded(lancer + playerAttacking.BonusAttack.Value - playerAttacking.MalusAttack.Value, playerAttacking, false);
                    Logger.Info("Wounds après : " + playerAttacked.Wound.Value);
                    Logger.Info("Vie total : " + playerAttacked.Character.characterHP);
                    Logger.Info("Mort ? " + playerAttacked.Dead.Value);
                }
                else
                {
                    int lancer = 0;
                    int damages = 0;

                    if (playerAttacking.HasSaber.Value)
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
                            // activer l'effet

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

                    GameManager.AttackAvailable.Value = false;
                }
            }
            /*
            else if (e is StealCardEvent stealTarget)
            {
                Player playerStealing = PlayerView.GetPlayer(stealTarget.PlayerId);
                Player playerStealed = PlayerView.GetPlayer(stealTarget.PlayerStealedId);
                Card card = CardView.GetCard(stealTarget.CardId);

                Debug.Log("Joueur voleur : " + playerStealing.Name);
                Debug.Log("Joueur volé : " + playerStealed.Name);
                Debug.Log("Carte volé : " + card.cardLabel);
                
                if (card is EquipmentCard pickedCard)
                {
                    playerStealing.PrintCards();
                    playerStealed.PrintCards();
                    pickedCard.equipe(playerStealing, pickedCard);
                    pickedCard.unequipe(playerStealed, pickedCard);
                    playerStealing.PrintCards();
                    playerStealed.PrintCards();
                }
                else
                    Debug.Log("La carte volée n'est pas un equipement.");
            }
            else if (e is GiveCardEvent giveCard)
            {
                Player playerGiving = PlayerView.GetPlayer(giveCard.PlayerId);
                Player playerGived = PlayerView.GetPlayer(giveCard.PlayerGivedId);
                Card card = CardView.GetCard(giveCard.CardId);


                Debug.Log("Joueur donneur : " + playerGiving.Name);
                Debug.Log("Joueur receveur : " + playerGived.Name);
                Debug.Log("Carte donnée : " + card.cardLabel);

                if (card is EquipmentCard pickedCard)
                {
                    playerGiving.PrintCards();
                    playerGived.PrintCards();
                    pickedCard.equipe(playerGived, pickedCard);
                    pickedCard.unequipe(playerGiving, pickedCard);
                    playerGiving.PrintCards();
                    playerGived.PrintCards();
                }
            }
            */
            /*
            else if (e is TakingWoundsEffectEvent takingWounds)
            {
                Player playerAttacking = PlayerView.GetPlayer(takingWounds.PlayerId);
                Player playerAttacked = PlayerView.GetPlayer(takingWounds.PlayerAttackedId);
                bool isPuppet = takingWounds.IsPuppet;
                int nbWoundsTaken = takingWounds.NbWoundsTaken;
                int nbWoundsSelfHealed = takingWounds.NbWoundsSelfHealed;

                Debug.Log("Joueur qui wound : " + playerAttacking.Name);
                Debug.Log("Joueur qui est wound : " + playerAttacked.Name);
                Debug.Log("Poupee : " + isPuppet);
                Debug.Log("Degats : " + nbWoundsTaken);
                Debug.Log("Soins : " + nbWoundsSelfHealed);

                Debug.Log("Blessure " + playerAttacking.Name + " avant : " + playerAttacking.Wound.Value);
                Debug.Log("Blessure " + playerAttacked.Name + " avant : " + playerAttacked.Wound.Value);

                if (isPuppet)
                {
                    int lancer = UnityEngine.Random.Range(1, 6);
                    Debug.Log("Lancer : " + lancer);
                    if (lancer <= 4)
                        playerAttacked.Wounded(nbWoundsTaken,playerAttacking,false);
                    else
                        playerAttacking.Wounded(nbWoundsTaken,playerAttacking,false);
                }
                else
                {
                    if (nbWoundsSelfHealed < 0)
                        playerAttacking.Wounded(-nbWoundsSelfHealed,playerAttacking,false);
                    else
                        playerAttacking.Healed(nbWoundsSelfHealed);

                    if (nbWoundsTaken < 0)
                        playerAttacked.Healed(-nbWoundsTaken);
                    else
                        playerAttacked.Wounded(nbWoundsTaken,playerAttacking,false);
                }
                Debug.Log("Blessure " + playerAttacking.Name + " après : " + playerAttacking.Wound.Value);
                Debug.Log("Blessure " + playerAttacked.Name + " après : " + playerAttacked.Wound.Value);
            }
            */
            /*
            else if (e is LightCardEffectEvent lcEffect)
            {
                Player player = PlayerView.GetPlayer(lcEffect.PlayerId);
                Player playerChoosed = PlayerView.GetPlayer(lcEffect.PlayerChoosenId);
                UsableCard lightCard = lcEffect.LightCard as UsableCard;

                if (lightCard.cardLabel == "card.light.light_benediction")
                {
                    playerChoosed.Healed(UnityEngine.Random.Range(1, 6));
                }
                else if (lightCard.cardLabel == "card.light.light_premiers_secours")
                {
                    playerChoosed.Wound.Value = 7;
                }
            }
            else if (e is GiveOrWoundEvent giveOrWound)
            {
                Player player = PlayerView.GetPlayer(giveOrWound.PlayerId);
                bool give = giveOrWound.Give;

                if (give)
                {
                    if (player.ListCard.Count == 0)
                    {
                        return;
                    }

                    List<int> choices = new List<int>();
                    foreach (Player p in PlayerView.GetPlayers())
                        if (!p.Dead.Value && p.Id != player.Id)
                            choices.Add(p.Id);

                    if (choices.Count != 0)
                    {
                        EventView.Manager.Emit(new SelectGiveCardEvent()
                        {
                            PlayerId = player.Id,
                            PossibleTargetId = choices.ToArray()
                        });
                    }
                }
                else
                    player.Wounded(1,player,false);
            }
            */
            else if (e is RevealCardEvent reveal)
            {
                Player p = PlayerView.GetPlayer(reveal.PlayerId);

                p.Revealed.Value = true;

                if (p.HasSpear.Value == true && p.Character.team == CharacterTeam.Hunter)
                    p.BonusAttack.Value += 2;

                Debug.Log("Le joueur " + p.Name + " se révèle.");
            }
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
            else if (e is PowerUseEvent pue)
            {
                Player p = PlayerView.GetPlayer(pue.PlayerId);
                if (p.CanUsePower.Value)
                {
                    p.Character.power.power(p);
                }
            }
            /*
            else if (e is BobPowerEvent bpe)
            {
                bool UsePower = bpe.UsePower;

                if (UsePower)
                {
                    int BobId = bpe.PlayerId;
                    Player Bob = PlayerView.GetPlayer(BobId);

                    // On commence par annuler les dommages que la cible a subit
                    Player cible = PlayerView.GetPlayer(Bob.OnAttackingPlayer.Value);

                    cible.Healed(Bob.DamageDealed.Value);

                    // On lui vole ensuite un équipement
                    EventView.Manager.Emit(new SelectStealCardFromPlayerEvent()
                    {
                        PlayerId = BobId,
                        PlayerStealedId = Bob.OnAttackingPlayer.Value
                    });
                }
            }
            
            else if (e is TestEvent te)
            {

                Debug.Log("ID : " + te.PlayerId);

                Player player = PlayerView.GetPlayer(te.PlayerId);

                Debug.Log("Joueur attaquant : " + player.Name);

                List<int> listTarget = new List<int>();
                foreach (Player p in PlayerView.GetPlayers())
                    if (!p.Dead.Value && p.Id != player.Id)
                        listTarget.Add(p.Id);

                if (listTarget.Count != 0)
                    EventView.Manager.Emit(new SelectPlayerTakingWoundsEvent()
                    {
                        PlayerId = player.Id,
                        PossibleTargetId = listTarget.ToArray(),
                        IsPuppet = true,
                        NbWoundsTaken = 3,
                        NbWoundsSelfHealed = 0
                    });
                else
                    Debug.Log("Pas de joueur à blesser.");
            }
            */
        }
    }
}
