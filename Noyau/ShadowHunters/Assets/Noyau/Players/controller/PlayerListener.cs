using Assets.Noyau.Cards.controller;
using Assets.Noyau.Cards.model;
using Assets.Noyau.Cards.view;
using Assets.Noyau.event_in;
using Assets.Noyau.event_out;
using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.model;
using Assets.Noyau.Players.view;
using EventSystem;
using Scripts;
using Scripts.event_in;
using Scripts.event_out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assets.Noyau.Manager.view.GameManager;

namespace Assets.Noyau.Players.controller
{
    class PlayerListener : IListener<PlayerEvent>
    {
        public void OnEvent(PlayerEvent e, string[] tags = null)
        {
            if (e is EndTurnEvent ete)
            {
                if (GameManager.PlayerTurn.Value == null)
                    GameManager.PlayerTurn.Value = PlayerView.GetPlayer(UnityEngine.Random.Range(0, PlayerView.NbPlayer));

                else if (GameManager.PlayerTurn.Value.HasAncestral.Value) // si le joueur a utilisé le savoir ancestral, le joueur suivant reste lui
                    GameManager.PlayerTurn.Value.HasAncestral.Value = false;

                else
                    GameManager.PlayerTurn.Value = PlayerView.NextPlayer(GameManager.PlayerTurn.Value);
                

                if (GameManager.PlayerTurn.Value.HasGuardian.Value)
                    GameManager.PlayerTurn.Value.HasGuardian.Value = false;


                EventView.Manager.Emit(new SelectedNextPlayer()
                {
                    PlayerId = GameManager.PlayerTurn.Value.Id
                });
            }
            else if (e is NewTurnEvent nte)
            {
                GameManager.MovementAvailable.Value = true;
                GameManager.StartOfTurn.Value = true;
            }
            else if (e is AskMovement am)
            {
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
                        availableDestination.Add(tmpavailableDestination);
                        nbrolls--;
                    }
                }
                availableDestination.Remove(GameManager.PlayerTurn.Value.Position.Value);

                EventView.Manager.Emit(new SelectMovement()
                {
                    LocationAvailable = availableDestination.ToArray()
                });
            }
            else if (e is MoveOn mo)
            {
                Player currentPlayer = PlayerView.GetPlayer(mo.PlayerId);
                currentPlayer.Position.Value = mo.Location;

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
                        EventView.Manager.Emit(new SelectUsableCardPickedEvent(CardView.GCard.Foret.Id, false));
                        break;
                    case Position.Sanctuaire:
                        EventView.Manager.Emit(new SelectUsableCardPickedEvent(CardView.GCard.Sanctuaire.Id, false));
                        break;
                }
            }
            else if (e is ForestSelectTargetEvent fste)
            {
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
            else if (e is DrawCardEvent drawCard && GameManager.PlayerTurn.Value.Id == e.PlayerId)
            {
                Player player = PlayerView.GetPlayer(drawCard.PlayerId);
                Card pickedCard = null;

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

                if (pickedCard is UsableCard pickedUsableCard)
                {
                    EventView.Manager.Emit(new SelectUsableCardPickedEvent(pickedUsableCard.Id, pickedCard.cardType == CardType.Vision));
                }
                else if (pickedCard is EquipmentCard pickedEquipmentCard)
                {
                    EventView.Manager.Emit(new DrawEquipmentCardEvent(pickedEquipmentCard.Id));
                    pickedEquipmentCard.equipe(player);
                }
            }
            else if (e is AttackEvent attack)
            {
                Player player = PlayerView.GetPlayer(attack.PlayerId);

                List<Player> targetablePlayers = player.getTargetablePlayers();

                if(targetablePlayers.Count != 0)
                {
                    if (!player.HasGatling.Value)
                    {
                        List<int> playersId = new List<int>();
                        foreach (Player p in PlayerView.GetPlayers())
                            playersId.Add(p.Id);

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
                            lancer = UnityEngine.Random.Range(1, 4);
                        else
                            lancer = UnityEngine.Random.Range(1, 6) - UnityEngine.Random.Range(1, 4);
                        
                        if (lancer != 0)
                            foreach (Player p in PlayerView.GetPlayers())
                                p.Wounded(lancer + player.BonusAttack.Value - player.MalusAttack.Value, player, true);
                    }
                }
            }
            else if (e is AttackPlayerEvent attackPlayer)
            {
                Player playerAttacking = PlayerView.GetPlayer(attackPlayer.PlayerId);
                Player playerAttacked = PlayerView.GetPlayer(attackPlayer.PlayerAttackedId);

                int lancer = 0;

                if (playerAttacking.HasSaber.Value)
                    lancer = UnityEngine.Random.Range(1, 4);
                else
                    lancer = UnityEngine.Random.Range(1, 6) - UnityEngine.Random.Range(1, 4);

                //Debug.Log("Le lancer vaut : " + lancerTotal);

                if (lancer != 0)
                    playerAttacked.Wounded(lancer + playerAttacking.BonusAttack.Value - playerAttacking.MalusAttack.Value, playerAttacking, true);
            }
            else if (e is StealCardEvent stealTarget)
            {
                Player playerStealing = PlayerView.GetPlayer(stealTarget.PlayerId);
                Player playerStealed = PlayerView.GetPlayer(stealTarget.PlayerStealedId);
                string stealedCard = stealTarget.CardStealedName;

                int indexCard = playerStealed.HasCard(stealedCard);
                playerStealing.AddCard(playerStealed.ListCard[indexCard]);

                if (playerStealed.ListCard[indexCard] is EquipmentCard pickedCard)
                {
                    pickedCard.equipe(playerStealing);
                    pickedCard.unequipe(playerStealed);
                }
            }
            else if (e is GiveCardEvent giveCard)
            {
                Player playerGiving = PlayerView.GetPlayer(giveCard.PlayerId);
                Player playerGived = PlayerView.GetPlayer(giveCard.PlayerGivedId);
                string givedCard = giveCard.CardGivedName;

                int indexCard = playerGiving.HasCard(givedCard);
                playerGived.AddCard(playerGiving.ListCard[indexCard]);

                playerGiving.PrintCards();
                playerGived.PrintCards();

                if (playerGiving.ListCard[indexCard] is EquipmentCard pickedCard)
                {
                    pickedCard.equipe(playerGived);
                    pickedCard.unequipe(playerGiving);
                }
            }
            else if (e is TakingWoundsEffectEvent takingWounds)
            {
                Player playerAttacking = PlayerView.GetPlayer(takingWounds.PlayerId);
                Player playerAttacked = PlayerView.GetPlayer(takingWounds.PlayerAttackedId);
                bool isPuppet = takingWounds.IsPuppet;
                int nbWoundsTaken = takingWounds.NbWoundsTaken;
                int nbWoundsSelfHealed = takingWounds.NbWoundsSelfHealed;

                if (isPuppet)
                {
                    int lancer = UnityEngine.Random.Range(1, 6);
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
            }
            else if (e is LightCardEffectEvent lcEffect)
            {
                Player player = PlayerView.GetPlayer(lcEffect.PlayerId);
                Player playerChoosed = PlayerView.GetPlayer(lcEffect.PlayerChoosenId);
                UsableCard lightCard = lcEffect.LightCard as UsableCard;

                foreach(CardEffect effect in lightCard.cardEffect)
                {
                    if (effect.targetableCondition(playerChoosed))
                        effect.effect(playerChoosed, lightCard);
                }

                /*if (lightCard.cardLabel == "card.light.light_benediction")
                {
                    playerChoosed.Healed(UnityEngine.Random.Range(1, 6));
                }
                else if (lightCard.cardLabel == "card.light.light_premiers_secours")
                {
                    playerChoosed.Wound.Value = 7;
                }*/
            }
            else if (e is VisionCardEffectEvent vcEffect)
            {
                Player playerGiving = PlayerView.GetPlayer(vcEffect.PlayerId);
                Player playerGived = PlayerView.GetPlayer(vcEffect.TargetId);
                UsableCard visionCard = vcEffect.VisionCard as UsableCard;

                foreach (CardEffect effect in visionCard.cardEffect)
                {
                    if (effect.targetableCondition(playerGived))
                        effect.effect(playerGived, visionCard);
                }
                /*
                CharacterTeam team = playerGived.Character.team;

                // Cartes applicables en fonction des équipes ?
                if ((team == CharacterTeam.Shadow && pickedCard.visionEffect.effectOnShadow && !metaPower)
                    || (team == CharacterTeam.Hunter && pickedCard.visionEffect.effectOnHunter)
                    || (team == CharacterTeam.Neutral && pickedCard.visionEffect.effectOnNeutral)
                    || (team == CharacterTeam.Shadow && !pickedCard.visionEffect.effectOnShadow && metaPower))
                {
                    // Cas des cartes infligeant des Blessures
                    if (pickedCard.visionEffect.effectTakeWounds)
                        playerGived.Wounded(pickedCard.visionEffect.nbWounds,playerGiving,false);

                    // Cas des cartes soignant des Blessures
                    else if (pickedCard.visionEffect.effectHealingOneWound)
                    {
                        if (playerGived.Wound.Value == 0)
                            playerGived.Wounded(1,playerGiving,false);
                        else
                            playerGived.Healed(1);
                    }
                    // Cas des cartes volant une carte équipement ou infligeant des Blessures
                    else if (pickedCard.visionEffect.effectGivingEquipementCard)
                    {
                        if (playerGived.ListCard.Count == 0)
                        {
                            //Debug.Log("Vous ne possédez pas de carte équipement.");
                            playerGived.Wounded(1,playerGiving,false);
                        }
                        else
                        {
                            //Debug.Log("Voulez-vous donner une carte équipement ou subir une Blessure ?");

                            EventView.Manager.Emit(new SelectGiveOrWoundEvent()
                            {
                                PlayerId = playerGived.Id
                            });
                        }
                    }
                }
                // Cas des cartes applicables en fonction des points de vie
                //else if (pickedCard.visionEffect.effectOnLowHP && CheckLowHPCharacters(playerGived.Character.characterName))
                else if (pickedCard.visionEffect.effectOnLowHP && pickedCard.condition(playerGived))
                            playerGived.Wounded(1,playerGiving,false);
                else if (pickedCard.visionEffect.effectOnHighHP && pickedCard.condition(playerGived))
                    playerGived.Wounded(2,playerGiving,false);

                // Cas de la carte Vision Suprême
                else if (pickedCard.visionEffect.effectSupremeVision)
                {
                    //TODO montrer la carte personnage
                    //Debug.Log("C'est une carte Vision Suprême !");
                }
                else
                {
                    //Debug.Log("Rien ne se passe.");
                }*/
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
            else if (e is RevealCard reveal)
            {
                Player p = PlayerView.GetPlayer(reveal.PlayerId);

                p.Revealed.Value = true;

                if (p.HasSpear.Value == true && p.Character.team == CharacterTeam.Hunter)
                    p.BonusAttack.Value += 2;
            }
            else if (e is BobPowerEvent bpe)
            {
                Player playerBob = PlayerView.GetPlayer(bpe.PlayerId);
                
                
                Player playerBobed = PlayerView.GetPlayer(GameManager.PlayerAttackedByBob);
                int bobDamages = GameManager.DamageDoneByBob;

                bool usePower = bpe.UsePower;

                if (usePower)
                {
                    bool hasEquip = false;
                    foreach (Card card in playerBobed.ListCard)
                        if (card is EquipmentCard)
                            hasEquip = true;

                    if (hasEquip)
                    {
                        EventView.Manager.Emit(new SelectStealCardFromPlayerEvent()
                        {
                            PlayerId = playerBob.Id,
                            PlayerStealedId = playerBobed.Id
                        });
                    }
                }
                else
                {
                    playerBobed.Wounded(bobDamages, playerBob, true);
                }
            }
        }
    }
}
