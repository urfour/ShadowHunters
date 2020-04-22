using Assets.Noyau.Cards.controller;
using Assets.Noyau.Cards.model;
using Assets.Noyau.event_in;
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
                {
                    GameManager.PlayerTurn.Value.HasAncestral.Value = false;
                }
                else
                    GameManager.PlayerTurn.Value = PlayerView.NextPlayer(GameManager.PlayerTurn.Value);
                

                if (GameManager.PlayerTurn.Value.HasGuardian.Value)
                {
                    GameManager.PlayerTurn.Value.HasGuardian.Value = false;
                }


                EventView.Manager.Emit(new SelectedNextPlayer()
                {
                    PlayerId = GameManager.PlayerTurn.Value.Id
                });
            }
            else if (e is NewTurnEvent nte)
            {
                GameManager.MovementAvailable.Value = true;
                //currentPlayer.RollTheDices.Value = false;

                /*
                if (currentPlayer.Character.characterName == "Emi"
                    || currentPlayer.Character.characterName == "Franklin"
                    || currentPlayer.Character.characterName == "Georges")
                {
                    currentPlayer.CanUsePower.Value = false;
                }
                */
                /*
                List<Position> availableDestination = new List<Position>();

                if (currentPlayer.HasCompass.Value)
                {
                    int lancer01 = UnityEngine.Random.Range(1, 6);
                    int lancer02 = UnityEngine.Random.Range(1, 4);
                    int lancer11 = UnityEngine.Random.Range(1, 6);
                    int lancer12 = UnityEngine.Random.Range(1, 4);

                    while (lancer11 + lancer12 == lancer01 + lancer02)
                    {
                        lancer11 = UnityEngine.Random.Range(1, 6);
                        lancer12 = UnityEngine.Random.Range(1, 4);
                    }

                    EventView.Manager.Emit(new SelectDiceThrow()
                    {
                        PlayerId = currentPlayer.Id,
                        D6Dice1 = lancer01,
                        D4Dice1 = lancer02,
                        D6Dice2 = lancer11,
                        D4Dice2 = lancer12,
                    });
                }
                else
                {
                    int lancer01 = UnityEngine.Random.Range(1, 6);
                    int lancer02 = UnityEngine.Random.Range(1, 4);

                    while (availableDestination.Count >= 0)
                    {
                        lancer01 = UnityEngine.Random.Range(1, 6);
                        lancer02 = UnityEngine.Random.Range(1, 4);

                        switch (lancer01 + lancer02)
                        {
                            case 2:
                            case 3:
                                availableDestination.Add(Position.Antre);
                                break;
                            case 4:
                            case 5:
                                availableDestination.Add(Position.Porte);
                                break;
                            case 6:
                                availableDestination.Add(Position.Monastere);
                                break;
                            case 7:
                                availableDestination.Add(Position.Antre);
                                availableDestination.Add(Position.Porte);
                                availableDestination.Add(Position.Monastere);
                                availableDestination.Add(Position.Cimetiere);
                                availableDestination.Add(Position.Foret);
                                availableDestination.Add(Position.Foret);
                                break;
                            case 8:
                                availableDestination.Add(Position.Cimetiere);
                                break;
                            case 9:
                                availableDestination.Add(Position.Foret);
                                break;
                            case 10:
                                availableDestination.Add(Position.Sanctuaire);
                                break;
                        }
                        if (availableDestination.Contains(currentPlayer.Position))
                        {
                            currentPlayer.Position = availableDestination[0];
                        }
                        else
                            availableDestination.Remove(currentPlayer.Position);

                    }
                    EventView.Manager.Emit(new SelectMovement()
                    {
                        PlayerId = currentPlayer.Id,
                        D6Dice = lancer01,
                        D4Dice = lancer02,
                        LocationAvailable = availableDestination.ToArray()
                    });
                }
                */
            }
            else if (e is AskMovement am)
            {
                List<int> dicesRolls = new List<int>();

                int nbrolls = 1;

                if (GameManager.PlayerTurn.Value.HasCompass.Value)
                {
                    nbrolls++;
                }

                List<Position> availableDestination = new List<Position>();
                while (nbrolls > 0)
                {
                    int val = UnityEngine.Random.Range(1, 6) + UnityEngine.Random.Range(1, 4);
                    Position tmpavailableDestination = Position.None;
                    switch (val)
                    {
                        case 2:
                        case 3:
                            tmpavailableDestination = Position.Antre;
                            break;
                        case 4:
                        case 5:
                            tmpavailableDestination = Position.Porte;
                            break;
                        case 6:
                            tmpavailableDestination = Position.Monastere;
                            break;
                        case 7:
                            availableDestination = new List<Position>()
                            {
                                Position.Antre,
                                Position.Porte,
                                Position.Monastere,
                                Position.Cimetiere,
                                Position.Foret,
                                Position.Sanctuaire
                            };
                            nbrolls = 0;
                            break;
                        case 8:
                            tmpavailableDestination = Position.Cimetiere;
                            break;
                        case 9:
                            tmpavailableDestination = Position.Foret;
                            break;
                        case 10:
                            tmpavailableDestination = Position.Sanctuaire;
                            break;
                    }
                    if (!availableDestination.Contains(tmpavailableDestination) && GameManager.PlayerTurn.Value.Position != tmpavailableDestination)
                    {
                        availableDestination.Add(tmpavailableDestination);
                        nbrolls--;
                    }
                }
                availableDestination.Remove(GameManager.PlayerTurn.Value.Position);

                EventView.Manager.Emit(new SelectMovement()
                {
                    LocationAvailable = availableDestination.ToArray()
                });
            }
            else if (e is MoveOn mo)
            {
                Player currentPlayer = PlayerView.GetPlayer(mo.PlayerId);
                currentPlayer.Position = mo.Location;

                /*
                gameBoard.setPositionOfAt(currentPlayer.Id, mo.Location);

                currentPlayer.AttackPlayer.Value = true;
                if (currentPlayer.HasSaber.Value)
                    currentPlayer.EndTurn.Value = false;
                else
                    currentPlayer.EndTurn.Value = true;
                    */

                switch (currentPlayer.Position)
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
                        currentPlayer.ForestHeal.Value = true;
                        currentPlayer.ForestWounds.Value = true;
                        break;
                    case Position.Sanctuaire:
                        List<int> target2 = new List<int>();
                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != currentPlayer.Id && p.ListCard.Count > 0)
                                target2.Add(p.Id);

                        EventView.Manager.Emit(new SelectStealCardEvent()
                        {
                            PlayerId = currentPlayer.Id,
                            PossiblePlayerTargetId = target2.ToArray()
                        });
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
            else if (e is DrawCardEvent drawCard)
            {
                Player player = PlayerView.GetPlayer(drawCard.PlayerId);

                switch (drawCard.SelectedCardType)
                {
                    case CardType.Darkness:
                        //Debug.Log("Le joueur " + player.Name + " choisit de piocher une carte Ténèbres.");
                        DarknessCard darknessCard = gameBoard.DrawCard(CardType.Darkness) as DarknessCard;

                        if (darknessCard.isEquipement)
                        {
                            player.AddCard(darknessCard);
                            //Debug.Log("La carte " + darknessCard.cardName + " a été ajoutée à la main du joueur "
                            //    + player.Name + ".");
                        }

                        DarknessCardPower(darknessCard, player.Id);

                        if (!darknessCard.isEquipement)
                            gameBoard.AddDiscard(darknessCard, CardType.Darkness);

                        break;
                    case CardType.Light:

                        //Debug.Log("Le joueur " + player.Name + " pioche une carte Lumière.");

                        LightCard lightCard = gameBoard.DrawCard(CardType.Light) as LightCard;

                        if (lightCard.isEquipement)
                        {
                            player.AddCard(lightCard);
                            //Debug.Log("La carte " + lightCard.cardName + " a été ajoutée à la main du joueur "
                            //    + player.Name + ".");
                        }

                        LightCardPower(lightCard, player.Id);

                        if (!lightCard.isEquipement)
                            gameBoard.AddDiscard(lightCard, CardType.Light);

                        break;
                    case CardType.Vision:

                        //Debug.Log("Le joueur " + player.Name + " choisit de piocher une carte Vision.");

                        VisionCard visionCard = gameBoard.DrawCard(CardType.Vision) as VisionCard;

                        VisionCardPower(visionCard, player.Id);

                        gameBoard.AddDiscard(visionCard, CardType.Vision);

                        break;
                }
            }
            else if (e is AttackEvent attack)
            {
                AttackCorrespondingPlayer(attack.PlayerId);
            }
            else if (e is AttackPlayerEvent attackPlayer)
            {
                Player playerAttacking = PlayerView.GetPlayer(attackPlayer.PlayerId);
                Player playerAttacked = PlayerView.GetPlayer(attackPlayer.PlayerAttackedId);

                //Debug.Log("Joueur " + playerAttacking.Id + " (" + playerAttacking.Character.characterName
                //            + ") attaque joueur " + playerAttacked.Id + " (" + playerAttacked.Character.characterName + ")");

                int lancer1 = UnityEngine.Random.Range(1, 6);
                int lancer2 = UnityEngine.Random.Range(1, 4);
                int lancerTotal = (playerAttacking.HasSaber.Value == true) ? lancer2 : Math.Abs(lancer1 - lancer2);

                if (attackPlayer.PowerFranklin)
                    lancerTotal = lancer1;
                else if (attackPlayer.PowerGeorges)
                    lancerTotal = lancer2;

                //Debug.Log("Le lancer vaut : " + lancerTotal);

                if (lancerTotal == 0)
                    Debug.Log("Le lancer vaut 0, vous n'attaquez pas.");
                else
                {
                    //Debug.Log("Vous choisissez d'attaquer le joueur " + playerAttacked.Name + ".");

                    int dommageTotal = lancerTotal + playerAttacking.BonusAttack.Value - playerAttacking.MalusAttack.Value;

                    // Si Bob est révélé et inflige 2 dégats ou plus, il peut voler une arme 
                    if (playerAttacking.Character.characterName == "Bob"
                        && playerAttacking.Revealed.Value
                        && dommageTotal >= 2)
                    {
                        m_damageBob = dommageTotal;
                        PlayerAttacked.Value = playerAttacked.Id;
                        PlayerCardPower(playerAttacking);
                    }
                    else
                    {
                        // Le joueur attaqué se prend des dégats
                        playerAttacked.Wounded(dommageTotal,playerAttacking,true);

                        // Le Loup-garou peut contre attaquer
                        if (playerAttacked.Character.characterName == "LoupGarou"
                            && playerAttacked.Revealed.Value)
                        {
                            playerAttacked.CanUsePower.Value = true;
                        }

                        // Charles peut attaquer de nouveau
                        if (playerAttacking.Character.characterName == "Charles"
                            && playerAttacking.Revealed.Value)
                        {
                            playerAttacking.CanUsePower.Value = true;
                        }
                    }
                }
            }
            else if (e is StealCardEvent stealTarget)
            {
                Player playerStealing = PlayerView.GetPlayer(stealTarget.PlayerId);
                Player playerStealed = PlayerView.GetPlayer(stealTarget.PlayerStealedId);
                string stealedCard = stealTarget.CardStealedName;

                int indexCard = playerStealed.HasCard(stealedCard);
                playerStealing.AddCard(playerStealed.ListCard[indexCard]);

                if (playerStealed.ListCard[indexCard].isEquipement)
                {
                    if (playerStealed.ListCard[indexCard].cardType == CardType.Darkness)
                    {
                        DarknessCardPower(playerStealed.ListCard[indexCard] as DarknessCard, playerStealing.Id);
                        LooseEquipmentCard(playerStealed.Id, indexCard, 0);
                    }
                    else if (playerStealed.ListCard[indexCard].cardType == CardType.Light)
                    {
                        LightCardPower(playerStealed.ListCard[indexCard] as LightCard, playerStealing.Id);
                        LooseEquipmentCard(playerStealed.Id, indexCard, 1);
                    }
                }
                else
                {
                    //Debug.LogError("Erreur : la carte choisie n'est pas un équipement et ne devrait pas être là.");
                }

                //Debug.Log("La carte " + stealedCard + " a été volée au joueur "
                //    + playerStealed.Name + " par le joueur " + playerStealing.Name + " !");
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

                if (playerGiving.ListCard[indexCard].isEquipement)
                {
                    if (playerGiving.ListCard[indexCard].cardType == CardType.Darkness)
                    {
                        DarknessCardPower(playerGiving.ListCard[indexCard] as DarknessCard, playerGived.Id);
                        LooseEquipmentCard(playerGiving.Id, indexCard, 0);
                    }
                    else if (playerGiving.ListCard[indexCard].cardType == CardType.Light)
                    {
                        LightCardPower(playerGiving.ListCard[indexCard] as LightCard, playerGived.Id);
                        LooseEquipmentCard(playerGiving.Id, indexCard, 1);
                    }
                }
                else
                    //Debug.LogError("Erreur : la carte choisie n'est pas un équipement et ne devrait pas être là.");

                //Debug.Log("La carte " + givedCard + " a été donnée au joueur "
                //    + playerGived.Name + " par le joueur " + playerGiving.Name + " !");

                playerGiving.PrintCards();
                playerGived.PrintCards();
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
                    //Debug.Log("Le lancer donne " + lancer + ".");
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
            else if (e is RevealOrNotEvent revealOrNot)
            {
                Player player = PlayerView.GetPlayer(revealOrNot.PlayerId);
                bool hasRevealed = revealOrNot.HasRevealed;
                Card effectCard = revealOrNot.EffectCard;

                if (effectCard is DarknessCard
                    && hasRevealed
                    && player.Character.team == CharacterTeam.Shadow)
                {
                    player.Healed(player.Wound.Value);
                    //Debug.Log("Le joueur " + player.Name + " se soigne complètement");
                }
                else if (effectCard is LightCard effectLightCard)
                {
                    if (effectLightCard.lightEffect == LightEffect.Supreme
                        && hasRevealed
                        && player.Character.team == CharacterTeam.Hunter)
                    {
                        player.Healed(player.Wound.Value);
                        //Debug.Log("Le joueur " + player.Name + " se soigne complètement");
                    }
                    else if (effectLightCard.lightEffect == LightEffect.Chocolat
                                && hasRevealed
                                && (player.Character.characterName == "Allie"
                                    || player.Character.characterName == "Emi"
                                    || player.Character.characterName == "Metamorphe"))
                    {
                        player.Healed(player.Wound.Value);
                        //Debug.Log("Le joueur " + player.Name + " se soigne complètement");
                    }
                }
                else
                    //Debug.Log("Rien ne se passe.");
            }
            else if (e is LightCardEffectEvent lcEffect)
            {
                Player player = PlayerView.GetPlayer(lcEffect.PlayerId);
                Player playerChoosed = PlayerView.GetPlayer(lcEffect.PlayerChoosenId);
                LightCard lightCard = lcEffect.LightCard;

                if (lightCard.lightEffect == LightEffect.Benediction)
                {
                    //Debug.Log("Vous choisissez de soigner le joueur " + playerChoosed.Name + ".");
                    playerChoosed.Healed(UnityEngine.Random.Range(1, 6));
                }
                else if (lightCard.lightEffect == LightEffect.Benediction)
                {
                    //Debug.Log("Vous choisissez d'infliger 7 blessures au joueur " + playerChoosed.Name + ".");
                    playerChoosed.Wound.Value = 7;
                }
            }
            else if (e is VisionCardEffectEvent vcEffect)
            {

                Player playerGiving = PlayerView.GetPlayer(vcEffect.PlayerId);
                Player playerGived = PlayerView.GetPlayer(vcEffect.TargetId);
                VisionCard pickedCard = vcEffect.VisionCard;
                bool metaPower = vcEffect.MetamorphePower;

                // Utilisation de GCard
                if (pickedCard.condition(playerGived) || metaPower)
                    pickedCard.effect(playerGived);



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
                }
            }
            else if (e is GiveOrWoundEvent giveOrWound)
            {
                Player player = PlayerView.GetPlayer(giveOrWound.PlayerId);
                bool give = giveOrWound.Give;

                if (give)
                    GiveEquipmentCard(player.Id);
                else
                    player.Wounded(1,player,false);
            }
            else if (e is BobPowerEvent bobPower)
            {
                Player playerBob = PlayerView.GetPlayer(bobPower.PlayerId);
                Player playerBobed = PlayerView.GetPlayer(PlayerAttacked.Value);
                int bobDamages = m_damageBob;
                bool usePower = bobPower.UsePower;

                if (usePower)
                    StealEquipmentCard(playerBob.Id, playerBobed.Id);
                else
                    AttackCorrespondingPlayer(playerBob.Id, playerBobed.Id, bobDamages);
            }
            else if (e is RevealCard reveal)
            {
                RevealCard(PlayerView.GetPlayer(reveal.PlayerId));
            }
            else if (e is TestEvent test)
            {
                Player psing = PlayerView.GetPlayer(test.PlayerId);


                for (int i = 0; i < 2; i++)
                {
                    DarknessCard darknessCard = gameBoard.DrawCard(CardType.Darkness) as DarknessCard;
                    LightCard lightCard = gameBoard.DrawCard(CardType.Light) as LightCard;

                    psing.AddCard(darknessCard);
                    psing.AddCard(lightCard);
                }

                //psing.PrintCards();

                GiveEquipmentCard(psing.Id);

                //psing.PrintCards();
            }
        }
    }
}
