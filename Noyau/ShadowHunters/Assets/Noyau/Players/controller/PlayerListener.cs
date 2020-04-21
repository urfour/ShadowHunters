using Assets.Noyau.Manager.view;
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
                Player currentPlayer = m_players[nte.PlayerId];
                currentPlayer.RollTheDices.Value = false;

                if (currentPlayer.Character.characterType == CharacterType.Emi
                    || currentPlayer.Character.characterType == CharacterType.Franklin
                    || currentPlayer.Character.characterType == CharacterType.Georges)
                {
                    currentPlayer.CanUsePower.Value = false;
                }

                List<Position> position = new List<Position>();

                if (currentPlayer.HasCompass.Value)
                {
                    int lancer01 = UnityEngine.Random.Range(1, 6);
                    int lancer02 = UnityEngine.Random.Range(1, 4);
                    int lancer11 = UnityEngine.Random.Range(1, 6);
                    int lancer12 = UnityEngine.Random.Range(1, 4);

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

                    while (position.Count >= 0)
                    {
                        lancer01 = UnityEngine.Random.Range(1, 6);
                        lancer02 = UnityEngine.Random.Range(1, 4);

                        switch (lancer01 + lancer02)
                        {
                            case 2:
                            case 3:
                                position.Add(Position.Antre);
                                break;
                            case 4:
                            case 5:
                                position.Add(Position.Porte);
                                break;
                            case 6:
                                position.Add(Position.Monastere);
                                break;
                            case 7:
                                position.Add(Position.Antre);
                                position.Add(Position.Porte);
                                position.Add(Position.Monastere);
                                position.Add(Position.Cimetiere);
                                position.Add(Position.Foret);
                                position.Add(Position.Foret);
                                break;
                            case 8:
                                position.Add(Position.Cimetiere);
                                break;
                            case 9:
                                position.Add(Position.Foret);
                                break;
                            case 10:
                                position.Add(Position.Sanctuaire);
                                break;
                        }
                        if (position.Contains(currentPlayer.Position))
                        {
                            currentPlayer.Position = position[0];
                        }
                        else
                            position.Remove(currentPlayer.Position);

                    }
                    EventView.Manager.Emit(new SelectMovement()
                    {
                        PlayerId = currentPlayer.Id,
                        D6Dice = lancer01,
                        D4Dice = lancer02,
                        LocationAvailable = position.ToArray()
                    });
                }

            }
            else if (e is SelectedDiceEvent sde)
            {
                Player currentPlayer = m_players[sde.PlayerId];
                List<Position> position = new List<Position>();

                while (position.Count >= 0)
                {
                    switch (sde.D4Dice + sde.D6Dice)
                    {
                        case 2:
                        case 3:
                            position.Add(Position.Antre);
                            break;
                        case 4:
                        case 5:
                            position.Add(Position.Porte);
                            break;
                        case 6:
                            position.Add(Position.Monastere);
                            break;
                        case 7:
                            position.Add(Position.Antre);
                            position.Add(Position.Porte);
                            position.Add(Position.Monastere);
                            position.Add(Position.Cimetiere);
                            position.Add(Position.Foret);
                            position.Add(Position.Sanctuaire);
                            break;
                        case 8:
                            position.Add(Position.Cimetiere);
                            break;
                        case 9:
                            position.Add(Position.Foret);
                            break;
                        case 10:
                            position.Add(Position.Sanctuaire);
                            break;
                    }
                    if (position.Contains(currentPlayer.Position))
                    {
                        currentPlayer.Position = position[0];
                    }
                    else
                        position.Remove(currentPlayer.Position);

                }
                EventView.Manager.Emit(new SelectMovement()
                {
                    PlayerId = currentPlayer.Id,
                    D6Dice = sde.D6Dice,
                    D4Dice = sde.D4Dice,
                    LocationAvailable = position.ToArray()
                });
            }
            else if (e is MoveOn mo)
            {
                Player currentPlayer = m_players[mo.PlayerId];
                currentPlayer.Position = mo.Location;
                gameBoard.setPositionOfAt(currentPlayer.Id, mo.Location);

                currentPlayer.AttackPlayer.Value = true;
                if (currentPlayer.HasSaber.Value)
                    currentPlayer.EndTurn.Value = false;
                else
                    currentPlayer.EndTurn.Value = true;

                switch (currentPlayer.Position)
                {
                    case Position.Antre:
                        currentPlayer.DrawLightCard.Value = true;
                        break;
                    case Position.Porte:
                        currentPlayer.DrawLightCard.Value = true;
                        currentPlayer.DrawDarknessCard.Value = true;
                        currentPlayer.DrawVisionCard.Value = true;
                        break;
                    case Position.Monastere:
                        currentPlayer.DrawVisionCard.Value = true;
                        break;
                    case Position.Cimetiere:
                        currentPlayer.DrawDarknessCard.Value = true;
                        break;
                    case Position.Foret:
                        currentPlayer.ForestHeal.Value = true;
                        currentPlayer.ForestWounds.Value = true;
                        break;
                    case Position.Sanctuaire:
                        List<int> target2 = new List<int>();
                        foreach (Player p in m_players)
                            if (!p.IsDead() && p.Id != currentPlayer.Id && p.ListCard.Count > 0)
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

                foreach (Player p in m_players)
                {
                    if (!p.IsDead())
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
                {
                    wounds = 2;
                }
                else
                {
                    wounds = -1;
                }

                EventView.Manager.Emit(new SelectPlayerTakingWoundsEvent()
                {
                    PlayerId = fste.PlayerId,
                    PossibleTargetId = target.ToArray(),
                    IsPuppet = false,
                    NbWoundsTaken = wounds,
                    NbWoundsSelfHealed = 0
                });
            }
            else if (e is PowerUsedEvent powerUsed)
            {
                PlayerCardPower(m_players[powerUsed.PlayerId]);
            }
            else if (e is PowerNotUsedEvent powerNotUsed)
            {
                DontUsePower(m_players[powerNotUsed.PlayerId]);
            }
            else if (e is DrawCardEvent drawCard)
            {
                Player player = m_players[drawCard.PlayerId];

                switch (drawCard.SelectedCardType)
                {
                    case CardType.Darkness:
                        Debug.Log("Le joueur " + player.Name + " choisit de piocher une carte Ténèbres.");
                        DarknessCard darknessCard = gameBoard.DrawCard(CardType.Darkness) as DarknessCard;

                        if (darknessCard.isEquipement)
                        {
                            player.AddCard(darknessCard);
                            Debug.Log("La carte " + darknessCard.cardName + " a été ajoutée à la main du joueur "
                                + player.Name + ".");
                        }

                        DarknessCardPower(darknessCard, player.Id);

                        if (!darknessCard.isEquipement)
                            gameBoard.AddDiscard(darknessCard, CardType.Darkness);

                        break;
                    case CardType.Light:

                        Debug.Log("Le joueur " + player.Name + " pioche une carte Lumière.");

                        LightCard lightCard = gameBoard.DrawCard(CardType.Light) as LightCard;

                        if (lightCard.isEquipement)
                        {
                            player.AddCard(lightCard);
                            Debug.Log("La carte " + lightCard.cardName + " a été ajoutée à la main du joueur "
                                + player.Name + ".");
                        }

                        LightCardPower(lightCard, player.Id);

                        if (!lightCard.isEquipement)
                            gameBoard.AddDiscard(lightCard, CardType.Light);

                        break;
                    case CardType.Vision:

                        Debug.Log("Le joueur " + player.Name + " choisit de piocher une carte Vision.");

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
                Player playerAttacking = m_players[attackPlayer.PlayerId];
                Player playerAttacked = m_players[attackPlayer.PlayerAttackedId];

                Debug.Log("Joueur " + playerAttacking.Id + " (" + playerAttacking.Character.characterName
                            + ") attaque joueur " + playerAttacked.Id + " (" + playerAttacked.Character.characterName + ")");

                int lancer1 = UnityEngine.Random.Range(1, 6);
                int lancer2 = UnityEngine.Random.Range(1, 4);
                int lancerTotal = (playerAttacking.HasSaber.Value == true) ? lancer2 : Mathf.Abs(lancer1 - lancer2);

                if (attackPlayer.PowerFranklin)
                    lancerTotal = lancer1;
                else if (attackPlayer.PowerGeorges)
                    lancerTotal = lancer2;

                Debug.Log("Le lancer vaut : " + lancerTotal);

                if (lancerTotal == 0)
                    Debug.Log("Le lancer vaut 0, vous n'attaquez pas.");
                else
                {
                    Debug.Log("Vous choisissez d'attaquer le joueur " + playerAttacked.Name + ".");

                    int dommageTotal = lancerTotal + playerAttacking.BonusAttack.Value - playerAttacking.MalusAttack.Value;

                    // Si Bob est révélé et inflige 2 dégats ou plus, il peut voler une arme 
                    if (playerAttacking.Character.characterType == CharacterType.Bob
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
                        playerAttacked.Wounded(dommageTotal);

                        // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
                        if (playerAttacking.Character.characterType == CharacterType.Vampire
                            && playerAttacking.Revealed.Value
                            && dommageTotal > 0)
                            PlayerCardPower(playerAttacking);

                        // On vérifie si le joueur attaqué est mort
                        CheckPlayerDeath(playerAttacked.Id);

                        // Le Loup-garou peut contre attaquer
                        if (playerAttacked.Character.characterType == CharacterType.LoupGarou
                            && playerAttacked.Revealed.Value)
                        {
                            playerAttacked.CanUsePower.Value = true;
                        }

                        // Charles peut attaquer de nouveau
                        if (playerAttacking.Character.characterType == CharacterType.Charles
                            && playerAttacking.Revealed.Value)
                        {
                            playerAttacking.CanUsePower.Value = true;
                        }
                    }
                }
            }
            else if (e is StealCardEvent stealTarget)
            {
                Player playerStealing = m_players[stealTarget.PlayerId];
                Player playerStealed = m_players[stealTarget.PlayerStealedId];
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
                    Debug.LogError("Erreur : la carte choisie n'est pas un équipement et ne devrait pas être là.");
                }

                Debug.Log("La carte " + stealedCard + " a été volée au joueur "
                    + playerStealed.Name + " par le joueur " + playerStealing.Name + " !");
            }
            else if (e is GiveCardEvent giveCard)
            {
                Player playerGiving = m_players[giveCard.PlayerId];
                Player playerGived = m_players[giveCard.PlayerGivedId];
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
                    Debug.LogError("Erreur : la carte choisie n'est pas un équipement et ne devrait pas être là.");

                Debug.Log("La carte " + givedCard + " a été donnée au joueur "
                    + playerGived.Name + " par le joueur " + playerGiving.Name + " !");

                playerGiving.PrintCards();
                playerGived.PrintCards();
            }
            else if (e is TakingWoundsEffectEvent takingWounds)
            {
                Player playerAttacking = m_players[takingWounds.PlayerId];
                Player playerAttacked = m_players[takingWounds.PlayerAttackedId];
                bool isPuppet = takingWounds.IsPuppet;
                int nbWoundsTaken = takingWounds.NbWoundsTaken;
                int nbWoundsSelfHealed = takingWounds.NbWoundsSelfHealed;

                if (isPuppet)
                {
                    int lancer = UnityEngine.Random.Range(1, 6);
                    Debug.Log("Le lancer donne " + lancer + ".");
                    if (lancer <= 4)
                    {
                        playerAttacked.Wounded(nbWoundsTaken);
                        CheckPlayerDeath(playerAttacked.Id);
                    }
                    else
                    {
                        playerAttacking.Wounded(nbWoundsTaken);
                        CheckPlayerDeath(playerAttacking.Id);
                    }
                }
                else
                {

                    if (nbWoundsSelfHealed < 0)
                    {
                        playerAttacking.Wounded(-nbWoundsSelfHealed);
                        CheckPlayerDeath(playerAttacking.Id);
                    }
                    else
                        playerAttacking.Healed(nbWoundsSelfHealed);
                    if (nbWoundsTaken < 0)
                    {
                        playerAttacked.Healed(-nbWoundsTaken);
                    }
                    else
                    {
                        playerAttacked.Wounded(nbWoundsTaken);
                        CheckPlayerDeath(playerAttacked.Id);
                    }
                }
            }
            else if (e is RevealOrNotEvent revealOrNot)
            {
                Player player = m_players[revealOrNot.PlayerId];
                bool hasRevealed = revealOrNot.HasRevealed;
                Card effectCard = revealOrNot.EffectCard;

                if (effectCard is DarknessCard
                    && hasRevealed
                    && player.Team == CharacterTeam.Shadow)
                {
                    player.Healed(player.Wound.Value);
                    Debug.Log("Le joueur " + player.Name + " se soigne complètement");
                }
                else if (effectCard is LightCard effectLightCard)
                {
                    if (effectLightCard.lightEffect == LightEffect.Supreme
                        && hasRevealed
                        && player.Team == CharacterTeam.Hunter)
                    {
                        player.Healed(player.Wound.Value);
                        Debug.Log("Le joueur " + player.Name + " se soigne complètement");
                    }
                    else if (effectLightCard.lightEffect == LightEffect.Chocolat
                                && hasRevealed
                                && (player.Character.characterType == CharacterType.Allie
                                    || player.Character.characterType == CharacterType.Emi
                                    || player.Character.characterType == CharacterType.Metamorphe))
                    {
                        player.Healed(player.Wound.Value);
                        Debug.Log("Le joueur " + player.Name + " se soigne complètement");
                    }
                }
                else
                    Debug.Log("Rien ne se passe.");
            }
            else if (e is LightCardEffectEvent lcEffect)
            {
                Player player = m_players[lcEffect.PlayerId];
                Player playerChoosed = m_players[lcEffect.PlayerChoosenId];
                LightCard lightCard = lcEffect.LightCard;

                if (lightCard.lightEffect == LightEffect.Benediction)
                {
                    Debug.Log("Vous choisissez de soigner le joueur " + playerChoosed.Name + ".");
                    playerChoosed.Healed(UnityEngine.Random.Range(1, 6));
                }
                else if (lightCard.lightEffect == LightEffect.Benediction)
                {
                    Debug.Log("Vous choisissez d'infliger 7 blessures au joueur " + playerChoosed.Name + ".");
                    playerChoosed.SetWound(7);
                }
            }
            else if (e is VisionCardEffectEvent vcEffect)
            {

                Player playerGiving = m_players[vcEffect.PlayerId];
                Player playerGived = m_players[vcEffect.TargetId];
                VisionCard pickedCard = vcEffect.VisionCard;
                bool metaPower = vcEffect.MetamorphePower;

                Debug.Log("La carte Vision a été donnée au joueur " + playerGived.Name + ".");
                Debug.Log("Joueur " + playerGived.Name + " :\n" +
                            playerGived.Character.characterName + "\n" +
                            playerGived.Character.team + "\n" +
                            playerGived.Life + "\n" +
                            metaPower);
                Debug.Log("Effet carte :" +
                            "\neffectOnShadow" + pickedCard.visionEffect.effectOnShadow +
                            "\neffectOnHunter" + pickedCard.visionEffect.effectOnHunter +
                            "\neffectOnNeutral" + pickedCard.visionEffect.effectOnNeutral +
                            "\neffectOnHighHP" + pickedCard.visionEffect.effectOnHighHP +
                            "\neffectOnLowHP" + pickedCard.visionEffect.effectOnLowHP);

                CharacterTeam team = playerGived.Team;
                /*
                if (playerGived.Character.characterType == CharacterType.Metamorphe)
                {
                    // A enlever plus tard
                    usePowerButton.gameObject.SetActive(true);
                    dontUsePowerButton.gameObject.SetActive(true);

                    playerGived.CanUsePower.Value = true;
                    playerGived.CanNotUsePower.Value = true;

                    m_pickedVisionCard = pickedCard;
                }
                */
                // Cartes applicables en fonction des équipes ?
                if ((team == CharacterTeam.Shadow && pickedCard.visionEffect.effectOnShadow && !metaPower)
                    || (team == CharacterTeam.Hunter && pickedCard.visionEffect.effectOnHunter)
                    || (team == CharacterTeam.Neutral && pickedCard.visionEffect.effectOnNeutral)
                    || (team == CharacterTeam.Shadow && !pickedCard.visionEffect.effectOnShadow && metaPower))
                {
                    // Cas des cartes infligeant des Blessures
                    if (pickedCard.visionEffect.effectTakeWounds)
                    {
                        playerGived.Wounded(pickedCard.visionEffect.nbWounds);
                        CheckPlayerDeath(playerGived.Id);
                    }
                    // Cas des cartes soignant des Blessures
                    else if (pickedCard.visionEffect.effectHealingOneWound)
                    {
                        if (playerGived.Wound.Value == 0)
                        {
                            playerGived.Wounded(1);
                            CheckPlayerDeath(playerGived.Id);
                        }
                        else
                        {
                            playerGived.Healed(1);
                            CheckPlayerDeath(playerGived.Id);
                        }
                    }
                    // Cas des cartes volant une carte équipement ou infligeant des Blessures
                    else if (pickedCard.visionEffect.effectGivingEquipementCard)
                    {
                        if (playerGived.ListCard.Count == 0)
                        {
                            Debug.Log("Vous ne possédez pas de carte équipement.");
                            playerGived.Wounded(1);
                        }
                        else
                        {
                            Debug.Log("Voulez-vous donner une carte équipement ou subir une Blessure ?");

                            EventView.Manager.Emit(new SelectGiveOrWoundEvent()
                            {
                                PlayerId = playerGived.Id
                            });
                        }
                    }
                }
                // Cas des cartes applicables en fonction des points de vie
                else if (pickedCard.visionEffect.effectOnLowHP && CheckLowHPCharacters(playerGived.Character.characterName))
                {
                    playerGived.Wounded(1);
                    CheckPlayerDeath(playerGived.Id);
                }
                else if (pickedCard.visionEffect.effectOnHighHP && CheckHighHPCharacters(playerGived.Character.characterName))
                {
                    playerGived.Wounded(2);
                    CheckPlayerDeath(playerGived.Id);
                }
                // Cas de la carte Vision Suprême
                else if (pickedCard.visionEffect.effectSupremeVision)
                    //TODO montrer la carte personnage
                    Debug.Log("C'est une carte Vision Suprême !");
                else
                    Debug.Log("Rien ne se passe.");
            }
            else if (e is GiveOrWoundEvent giveOrWound)
            {
                Player player = m_players[giveOrWound.PlayerId];
                bool give = giveOrWound.Give;

                if (give)
                {
                    Debug.Log("Vous choisissez de donner une carte équipement.");
                    GiveEquipmentCard(player.Id);
                }
                else
                {
                    Debug.Log("Vous choisissez de subir 1 Blessure.");
                    player.Wounded(1);
                    CheckPlayerDeath(player.Id);
                }
            }
            else if (e is BobPowerEvent bobPower)
            {
                Player playerBob = m_players[bobPower.PlayerId];
                Player playerBobed = m_players[PlayerAttacked.Value];
                int bobDamages = m_damageBob;
                bool usePower = bobPower.UsePower;

                if (usePower)
                {
                    StealEquipmentCard(playerBob.Id, playerBobed.Id);
                }
                else
                {
                    AttackCorrespondingPlayer(playerBob.Id, playerBobed.Id, bobDamages);
                }
            }
            else if (e is RevealCard reveal)
            {
                RevealCard(m_players[reveal.PlayerId]);
            }
            else if (e is TestEvent test)
            {
                Player psing = m_players[test.PlayerId];


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
