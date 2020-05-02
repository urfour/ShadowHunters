using Assets.Noyau.Cards.model;
using Assets.Noyau.Manager.view;
using EventSystem;
using System.Collections.Generic;
using Scripts.event_out;
using Assets.Noyau.Players.view;
using Scripts.event_in;

namespace Assets.Noyau.Cards.controller
{
    /// <summary>
    /// Classe qui va instancier toute les cartes du jeu.
    /// </summary>

    public class GCard
    {
        public List<Card> cards = new List<Card>();

        public List<Card> visionDeck;
        public List<Card> lightDeck;
        public List<Card> darknessDeck;

        public List<Card> visionDiscard;
        public List<Card> lightDiscard;
        public List<Card> darknessDiscard;

        public UsableCard Foret;
        public UsableCard Sanctuaire;

        public UsableCard GeorgesPower;
        public UsableCard FranklinPower;
        public UsableCard BobPower;

        public GCard()
        {
            /// <summary>
            /// Fonction qui va instancier le lieu Forêt avec ses pouvoirs comme une carte à usage unique.
            /// </summary>
            /// <returns> Renvoie un UsableCard</returns>
            Foret = CreateUsableCard("card.location.foret", CardType.Location, "card.location.foret.description", true,
                new CardEffect("card.location.foret.effect.args.wound&2",
                    effect : (target, owner, card) =>
                    {
                        target.Wounded(2, owner, false);
                    },
                    targetableCondition: (target, owner) =>
                    {
                        return !target.HasBroche.Value
                                && !target.Dead.Value;
                    }
                ),
                new CardEffect("card.location.foret.effect.args.heal&1",
                    effect: (target, owner, card) =>
                    {
                        target.Healed(1);
                    },
                    targetableCondition: (target, owner) =>
                    {
                        return !target.Dead.Value;
                    }
                ));

            /// <summary>
            /// Fonction qui va instancier le lieu Sanctuaire avec ses pouvoirs comme une carte à usage unique.
            /// </summary>
            /// <returns> Renvoie un UsableCard</returns>
            Sanctuaire = CreateUsableCard("card.location.sanctuaire", CardType.Location, "card.location.sanctuaire.description", true,
                new CardEffect("card.location.sanctuaire.effect.steal",
                    effect: (target, owner, card) =>
                    {
                        EquipmentCard c = target.ListCard[GameManager.rand.Next(0, target.ListCard.Count - 1)] as EquipmentCard;

                        c.equipe(owner, c);
                        c.unequipe(target, c);
                    },
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner
                            && !player.Dead.Value
                            && player.ListCard.Count > 0;
                    }),
                new CardEffect("card.location.sanctuaire.nothing_happen",
                    targetableCondition: (player, owner) =>
                    {
                        foreach (Player p in PlayerView.GetPlayers())
                            if (p.ListCard.Count > 0)
                                return false;
                        return true;
                    },
                    effect: (player, owner, card) =>
                    {
                    }));

            GeorgesPower = CreateUsableCard("character.name.georges", CardType.Light, "character.name.georges.description", false,
                new CardEffect("character.name.georges.power",
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner && !player.Dead.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        int lancer = GameManager.rand.Next(1, 4);
                        player.Wounded(lancer, owner, false);
                    }));

            FranklinPower = CreateUsableCard("character.name.franklin", CardType.Light, "character.name.franklin.description", false,
                new CardEffect("character.name.franklin.power",
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner && !player.Dead.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        int lancer = GameManager.rand.Next(1, 6);
                        player.Wounded(lancer, owner, false);
                    }));

            BobPower = CreateUsableCard("character.name.bob", CardType.Darkness, "character.name.bob.description", false,
                new CardEffect("character.name.bob.power.steal",
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner && !player.Dead.Value && player.Id == owner.OnAttackingPlayer.Value && player.ListCard.Count > 0;
                    },
                    effect: (target, owner, card) =>
                    {
                        EquipmentCard c = target.ListCard[GameManager.rand.Next(0, target.ListCard.Count - 1)] as EquipmentCard;

                        c.equipe(owner, c);
                        c.unequipe(target, c);
                    }),
                new CardEffect("character.name.bob.power.attack",
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner && !player.Dead.Value && owner.Id == player.OnAttackedBy.Value;
                    },
                    effect: (target, owner, card) =>
                    {
                        target.Wounded(owner.DamageDealed.Value, owner, true);
                    }));

            darknessDiscard = new List<Card>();
            lightDiscard = new List<Card>();
            visionDiscard = new List<Card>();

            /// <summary>
            /// Fonction qui va instancier les cartes ténèbres avec leur pouvoirs.
            /// </summary>
            /// <returns> Renvoie une liste de Card</returns>
            darknessDeck = new List<Card>()
            {
                CreateUsableCard("card.darkness.darkness_araignee", CardType.Darkness, "card.darkness.darkness_araignee.description", false,
                new CardEffect("card.darkness.darkness_araignee.effect.args.wound_wound&2&2",
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner
                            && !player.Dead.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        if (!player.HasAmulet.Value)
                            player.Wounded(2,owner,false);
                        if (!owner.HasAmulet.Value)
                            owner.Wounded(2,owner,false);
                    })),

                CreateUsableCard("card.darkness.darkness_banane", CardType.Darkness, "card.darkness.darkness_banane.description", false,
                new CardEffect("card.darkness.darkness_banane.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return owner.ListCard.Count != 0
                            && player != owner
                            && !player.Dead.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        EquipmentCard c = owner.ListCard[GameManager.rand.Next(0, owner.ListCard.Count-1)] as EquipmentCard;
                        c.equipe(player, c);
                        c.unequipe(owner, c);
                    }),
                new CardEffect("card.darkness.darkness_banane.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return player.ListCard.Count == 0
                            && player == owner
                            && !player.Dead.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                            player.Wounded(1, player, false);
                    })),

                CreateUsableCard("card.darkness.darkness_chauve_souris", CardType.Darkness, "card.darkness.darkness_chauve_souris.description", false,
                new CardEffect("card.darkness.darkness_chauve_souris.effect.args.wound_heal&2&1",
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner
                            && !player.Dead.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        if (!player.HasAmulet.Value)
                            player.Wounded(2, owner, false);
                        owner.Healed(1);
                    })),

                CreateUsableCard("card.darkness.darkness_chauve_souris", CardType.Darkness, "card.darkness.darkness_chauve_souris.description", false,
                new CardEffect("card.darkness.darkness_chauve_souris.effect.args.wound_heal&2&1",
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner
                            && !player.Dead.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        if (!player.HasAmulet.Value)
                            player.Wounded(2, owner, false);
                        owner.Healed(1);
                    })),

                CreateUsableCard("card.darkness.darkness_chauve_souris", CardType.Darkness, "card.darkness.darkness_chauve_souris.description", false,
                new CardEffect("card.darkness.darkness_chauve_souris.effect.args.wound_heal&2&1",
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner
                            && !player.Dead.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        if (!player.HasAmulet.Value)
                            player.Wounded(2, owner, false);
                        owner.Healed(1);
                    })),

                CreateUsableCard("card.darkness.darkness_dynamite", CardType.Darkness, "card.darkness.darkness_dynamite.description", false,
                new CardEffect("card.darkness.darkness_dynamite.effect.args.wound_position&3",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        int lancer = GameManager.rand.Next(1, 6) + GameManager.rand.Next(1, 4);
                        Position area = Position.None;

                        switch (lancer)
                        {
                            case 2:
                            case 3:
                                area = Position.Antre;
                                break;
                            case 4:
                            case 5:
                                area = Position.Porte;
                                break;
                            case 6:
                                area = Position.Monastere;
                                break;
                            case 7:
                                break;
                            case 8:
                                area = Position.Cimetiere;
                                break;
                            case 9:
                                area = Position.Foret;
                                break;
                            case 10:
                                area = Position.Sanctuaire;
                                break;
                        }

                        if (lancer != 7)
                            foreach (Player p in PlayerView.GetPlayers())
                                if (p.Position.Value != -1 && GameManager.Board[p.Position.Value] == area && !p.HasAmulet.Value)
                                    p.Wounded(3, player, false);
                    })),

                CreateEquipmentCard("card.darkness.darkness_hache", CardType.Darkness, "card.darkness.darkness_hache.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    addeffect: (player, card) =>
                    {
                        player.BonusAttack.Value++;
                    },
                    rmeffect: (player, card) =>
                    {
                        player.BonusAttack.Value--;
                    }),


                CreateEquipmentCard("card.darkness.darkness_hachoir", CardType.Darkness, "card.darkness.darkness_hachoir.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    addeffect: (player, card) =>
                    {
                        player.BonusAttack.Value++;
                    },
                    rmeffect: (player, card) =>
                    {
                        player.BonusAttack.Value--;
                    }),


                CreateEquipmentCard("card.darkness.darkness_mitrailleuse", CardType.Darkness, "card.darkness.darkness_mitrailleuse.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    addeffect: (player, card) =>
                    {
                        player.HasGatling.Value = true;
                    },
                    rmeffect: (player, card) =>
                    {
                        player.HasGatling.Value = false;
                    }),


                CreateUsableCard("card.darkness.darkness_poupee", CardType.Darkness, "card.darkness.darkness_poupee.description", false,
                new CardEffect("card.darkness.darkness_poupee.effect.args.wound&3",
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner
                            && !player.Dead.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        if (GameManager.rand.Next(1, 6) <= 4)
                            player.Wounded(3,owner,false);
                        else
                            owner.Wounded(3,owner,false);
                    })),

                CreateEquipmentCard("card.darkness.darkness_revolver", CardType.Darkness, "card.darkness.darkness_revolver.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    addeffect: (player, card) =>
                    {
                        player.HasRevolver.Value = true;
                    },
                    rmeffect: (player, card) =>
                    {
                        player.HasRevolver.Value = false;
                    }),


                CreateUsableCard("card.darkness.darkness_rituel", CardType.Darkness, "card.darkness.darkness_rituel.description", false, true,
                new CardEffect("card.darkness.darkness_rituel.effect.args.heal",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner
                            && player.Revealed.Value
                            && player.Character.team == CharacterTeam.Shadow;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Healed(player.Wound.Value);
                    }),
                new CardEffect("card.darkness.darkness_rituel.effect.reveal",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner
                            && !player.Revealed.Value
                            && player.Character.team == CharacterTeam.Shadow;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Revealed.Value = true;
                        player.Healed(player.Wound.Value);
                    }),
                new CardEffect("card.darkness.darkness_rituel.nothing_happen",
                    // on peut ne rien faire uniquement si on n'est pas un Shadow qui s'est révélé
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner
                            && !(player.Revealed.Value && player.Character.team == CharacterTeam.Shadow);
                    },
                    effect: (player, owner, card) =>
                    {
                    })),

                CreateEquipmentCard("card.darkness.darkness_sabre", CardType.Darkness, "card.darkness.darkness_sabre.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    addeffect: (player, card) =>
                    {
                        player.HasSaber.Value = true;
                    },
                    rmeffect: (player, card) =>
                    {
                        player.HasSaber.Value = false;
                    }),


                CreateUsableCard("card.darkness.darkness_succube", CardType.Darkness, "card.darkness.darkness_succube.description", false,
                new CardEffect("card.darkness.darkness_succube.effect.steal",
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner
                            && !player.Dead.Value
                            && player.ListCard.Count > 0;
                    },
                    effect: (player, owner, card) =>
                    {
                        EquipmentCard c = player.ListCard[GameManager.rand.Next(0, player.ListCard.Count-1)] as EquipmentCard;

                        c.equipe(owner, c);
                        c.unequipe(player, c);
                    }),
                new CardEffect("card.darkness.darkness_succube.nothing_happen",
                    targetableCondition: (player, owner) =>
                    {
                        foreach (Player p in PlayerView.GetPlayers())
                            if (p != owner && p.ListCard.Count > 0)
                                return false;
                        return true;
                    },
                    effect: (player, owner, card) =>
                    {
                    })),

                CreateUsableCard("card.darkness.darkness_succube", CardType.Darkness, "card.darkness.darkness_succube.description", false,
                new CardEffect("card.darkness.darkness_succube.effect.steal",
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner
                            && !player.Dead.Value
                            && player.ListCard.Count > 0;
                    },
                    effect: (player, owner, card) =>
                    {
                        EquipmentCard c = player.ListCard[GameManager.rand.Next(0, player.ListCard.Count-1)] as EquipmentCard;

                        c.equipe(owner, c);
                        c.unequipe(player, c);
                    }),
                new CardEffect("card.darkness.darkness_succube.nothing_happen",
                    targetableCondition: (player, owner) =>
                    {
                        foreach (Player p in PlayerView.GetPlayers())
                            if (p != owner && p.ListCard.Count > 0)
                                return false;
                        return true;
                    },
                    effect: (player, owner, card) =>
                    {
                    })),

                CreateEquipmentCard("card.darkness.darkness_tronconneuse", CardType.Darkness, "card.darkness.darkness_tronconneuse.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    addeffect: (player, card) =>
                    {
                        player.BonusAttack.Value++;
                    },
                    rmeffect: (player, card) =>
                    {
                        player.BonusAttack.Value--;
                    }),

            };
            
            /// <summary>
            /// Fonction qui va instancier les cartes lumières avec leur pouvoirs.
            /// </summary>
            /// <returns> Renvoie une liste de Card</returns>
            lightDeck = new List<Card>()
            {
                CreateEquipmentCard("card.light.light_amulette", CardType.Light, "card.light.light_amulette.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    addeffect: (player, card) =>
                    {
                        player.HasAmulet.Value = true;
                    },
                    rmeffect: (player, card) =>
                    {
                        player.HasAmulet.Value = false;
                    }),

                CreateUsableCard("card.light.light_ange_gardien", CardType.Light, "card.light.light_ange_gardien.description", false,
                new CardEffect("card.light.light_ange_gardien.effect.get_item",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.HasGuardian.Value = true;
                    })),

                CreateUsableCard("card.light.light_supreme", CardType.Light, "card.light.light_supreme.description", false, true,
                new CardEffect("card.light.light_supreme.effect.reveal",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner
                            && player.Character.team == CharacterTeam.Hunter
                            && !player.Revealed.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Revealed.Value = true;
                        player.Healed(player.Wound.Value);
                    }),
                new CardEffect("card.light.light_supreme.effect.args.heal&max",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner
                            && player.Character.team == CharacterTeam.Hunter
                            && player.Revealed.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Healed(player.Wound.Value);
                    }),
                new CardEffect("card.light.light_supreme.nothing_happen",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner
                            && !(player.Character.team == CharacterTeam.Hunter && player.Revealed.Value);
                    },
                    effect: (player, owner, card) =>
                    {
                    })),

                CreateUsableCard("card.light.light_chocolat", CardType.Light, "card.light.light_chocolat.description", false, true,
                new CardEffect("card.light.light_chocolat.effect.args.reveal_heal",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner
                            && (player.Character.characterName == "character.name.allie"
                            || player.Character.characterName == "character.name.emi"
                            || player.Character.characterName == "character.name.metamorphe")
                            && !player.Revealed.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Revealed.Value = true;
                        player.Healed(player.Wound.Value);
                    }),
                new CardEffect("card.light.light_chocolat.effect.args.heal",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner
                            && (player.Character.characterName == "character.name.allie"
                            || player.Character.characterName == "character.name.emi"
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Revealed.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Healed(player.Wound.Value);
                    }),
                new CardEffect("card.light.light_chocolat.nothing_happen",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner
                            && !((player.Character.characterName == "character.name.allie"
                            || player.Character.characterName == "character.name.emi"
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Revealed.Value);
                    },
                    effect: (player, owner, card) =>{
                    })),

                CreateUsableCard("card.light.light_benediction", CardType.Light, "card.light.light_benediction.description", false,
                new CardEffect("card.light.light_benediction.effect.args.heal",
                    targetableCondition: (player, owner) =>
                    {
                        return player != owner
                            && !player.Dead.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Healed(GameManager.rand.Next(1, 6));
                    })),

                CreateEquipmentCard("card.light.light_boussole", CardType.Light, "card.light.light_boussole.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    addeffect: (player, card) =>
                    {
                        player.HasCompass.Value = true;
                    },
                    rmeffect: (player, card) =>
                    {
                        player.HasCompass.Value = false;
                    }),


                CreateEquipmentCard("card.light.light_broche", CardType.Light, "card.light.light_broche.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    addeffect: (player, card) =>
                    {
                        player.HasBroche.Value = true;
                    },
                    rmeffect: (player, card) =>
                    {
                        player.HasBroche.Value = false;
                    }),


                CreateEquipmentCard("card.light.light_crucifix", CardType.Light, "card.light.light_crucifix.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    addeffect: (player, card) =>
                    {
                        player.HasCrucifix.Value = true;
                    },
                    rmeffect: (player, card) =>
                    {
                        player.HasCrucifix.Value = false;
                    }),


                CreateUsableCard("card.light.light_eclair", CardType.Light, "card.light.light_eclair.description", false,
                new CardEffect("card.light.light_eclair.effect.args.wound_all&2",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        foreach (Player p in PlayerView.GetPlayers())
                            if (p.Id != player.Id)
                                p.Wounded(2, player, false);
                    })),

                CreateUsableCard("card.light.light_eau_benite", CardType.Light, "card.light.light_eau_benite.description", false,
                new CardEffect("card.light.light_eau_benite.effect.args.heal&2",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Healed(2);
                    })),

                CreateEquipmentCard("card.light.light_lance", CardType.Light, "card.light.light_lance.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    addeffect: (player, card) =>
                    {
                        player.HasSpear.Value = true;
                        if (player.Character.team == CharacterTeam.Hunter && player.Revealed.Value)
                            player.BonusAttack.Value += 2;
                    },
                    rmeffect: (player, card) =>
                    {
                        player.HasSpear.Value = false;
                        if (player.Character.team == CharacterTeam.Hunter && player.Revealed.Value)
                            player.BonusAttack.Value -= 2;
                    }),


                CreateUsableCard("card.light.light_miroir", CardType.Light, "card.light.light_miroir.description", false,
                new CardEffect("card.light.light_miroir.effect.reveal",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner
                            && !player.Revealed.Value
                            && player.Character.team == CharacterTeam.Shadow
                            && player.Character.characterName != "character.name.metamorphe";
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Revealed.Value = true;

                        if (player.HasSpear.Value == true && player.Character.team == CharacterTeam.Hunter)
                            player.BonusAttack.Value += 2;
                    }),
                new CardEffect("card.light.light_miroir.nothing_happen",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner
                            && (player.Revealed.Value
                            || player.Character.team != CharacterTeam.Shadow
                            || player.Character.characterName == "character.name.metamorphe");
                    },
                    effect: (player, owner, card) =>
                    {
                    })),

                CreateUsableCard("card.light.light_premiers_secours", CardType.Light, "card.light.light_premiers_secours.description", false,
                new CardEffect("card.light.light_premiers_secours.effect.args.wound_at&7",
                    targetableCondition: (player, owner) =>
                    {
                        return !player.Dead.Value;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wound.Value = 7;
                    })),

                CreateUsableCard("card.light.light_savoir", CardType.Light, "card.light.light_savoir.description", false,
                new CardEffect("card.light.light_savoir.effect.get_item",
                    targetableCondition: (player, owner) =>
                    {
                        return player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.HasAncestral.Value = true;
                    })),

                CreateEquipmentCard("card.light.light_toge", CardType.Light, "card.light.light_toge.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    addeffect: (player, card) =>
                    {
                        player.HasToge.Value = true;
                        player.MalusAttack.Value++;
                        player.ReductionWounds.Value = 1;
                    },
                    rmeffect: (player, card) =>
                    {
                        player.HasToge.Value = false;
                        player.MalusAttack.Value--;
                        player.ReductionWounds.Value = 0;
                    })

            };

            /// <summary>
            /// Fonction qui va instancier les cartes visions avec leur pouvoirs.
            /// </summary>
            /// <returns> Renvoie une liste de Card</returns>
            visionDeck = new List<Card>()
            {
                CreateVisionCard("card.vision.vision_destructrice", CardType.Vision, "card.vision.vision_destructrice.description", false,
                new CardEffect("card.vision.vision_destructrice.effect.args.wound&2",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.characterHP >= 12
                                || player.Character.characterName.Equals("character.name.metamorphe"))
                                && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(2, player, false);
                    })),

                CreateVisionCard("card.vision.vision_clairvoyante", CardType.Vision, "card.vision.vision_clairvoyante.description", false,
                new CardEffect("card.vision.vision_clairvoyante.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.characterHP <= 11
                                || player.Character.characterName.Equals("character.name.metamorphe"))
                                && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1, player, false);
                    })),

                CreateVisionCard("card.vision.vision_cupide", CardType.Vision, "card.vision.vision_cupide.description", false,
                new CardEffect("card.vision.vision_cupide.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Shadow)
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1,player,false);
                    }),
                new CardEffect("card.vision.vision_cupide.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Shadow)
                            && player.ListCard.Count > 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        EquipmentCard c = player.ListCard[GameManager.rand.Next(0, player.ListCard.Count - 1)] as EquipmentCard;

                        c.equipe(owner, c);
                        c.unequipe(player, c);
                    })),

                CreateVisionCard("card.vision.vision_cupide", CardType.Vision, "card.vision.vision_cupide.description", false,
                new CardEffect("card.vision.vision_cupide.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Shadow)
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1,player,false);
                    }),
                new CardEffect("card.vision.vision_cupide.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Shadow)
                            && player.ListCard.Count > 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        EquipmentCard c = owner.ListCard[GameManager.rand.Next(0, owner.ListCard.Count-1)] as EquipmentCard;
                        c.equipe(player, c);
                        c.unequipe(owner, c);
                    })),

                CreateVisionCard("card.vision.vision_enivrante", CardType.Vision, "card.vision.vision_enivrante.description", false,
                new CardEffect("card.vision.vision_enivrante.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1,player,false);
                    }),
                new CardEffect("card.vision.vision_enivrante.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.ListCard.Count > 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        EquipmentCard c = owner.ListCard[GameManager.rand.Next(0, owner.ListCard.Count-1)] as EquipmentCard;
                        c.equipe(player, c);
                        c.unequipe(owner, c);
                    })),

                CreateVisionCard("card.vision.vision_enivrante", CardType.Vision, "card.vision.vision_enivrante.description", false,
                new CardEffect("card.vision.vision_enivrante.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1,player,false);
                    }),
                new CardEffect("card.vision.vision_enivrante.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.ListCard.Count > 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        EquipmentCard c = owner.ListCard[GameManager.rand.Next(0, owner.ListCard.Count-1)] as EquipmentCard;
                        c.equipe(player, c);
                        c.unequipe(owner, c);
                    })),

                CreateVisionCard("card.vision.vision_furtive", CardType.Vision, "card.vision.vision_furtive.description", false,
                new CardEffect("card.vision.vision_furtive.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Shadow
                            || player.Character.team == CharacterTeam.Hunter)
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1,player,false);
                    }),
                new CardEffect("card.vision.vision_furtive.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Shadow
                            || player.Character.team == CharacterTeam.Hunter)
                            && player.ListCard.Count > 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        EquipmentCard c = owner.ListCard[GameManager.rand.Next(0, owner.ListCard.Count-1)] as EquipmentCard;
                        c.equipe(player, c);
                        c.unequipe(owner, c);
                    })),

                CreateVisionCard("card.vision.vision_furtive", CardType.Vision, "card.vision.vision_furtive.description", false,
                new CardEffect("card.vision.vision_furtive.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Shadow
                            || player.Character.team == CharacterTeam.Hunter)
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1,player,false);
                    }),
                new CardEffect("card.vision.vision_furtive.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Shadow
                            || player.Character.team == CharacterTeam.Hunter)
                            && player.ListCard.Count > 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        EquipmentCard c = owner.ListCard[GameManager.rand.Next(0, owner.ListCard.Count-1)] as EquipmentCard;
                        c.equipe(player, c);
                        c.unequipe(owner, c);
                    })),

                CreateVisionCard("card.vision.vision_divine", CardType.Vision, "card.vision.vision_divine.description", false,
                new CardEffect("card.vision.vision_divine.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Wound.Value == 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1, player, false);
                    }),
                new CardEffect("card.vision.vision_divine.effect.args.heal&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Wound.Value != 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Healed(1);
                    })),


                CreateVisionCard("card.vision.vision_divine", CardType.Vision, "card.vision.vision_divine.description", false,
                new CardEffect("card.vision.vision_divine.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Wound.Value == 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1, player, false);
                    }),
                new CardEffect("card.vision.vision_divine.effect.args.heal&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Wound.Value != 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Healed(1);
                    })),

                CreateVisionCard("card.vision.vision_lugubre", CardType.Vision, "card.vision.vision_lugubre.description", false,
                new CardEffect("card.vision.vision_lugubre.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return player.Character.team == CharacterTeam.Shadow
                            && player.Wound.Value == 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1, player, false);
                    }),
                new CardEffect("card.vision.vision_lugubre.effect.args.heal&1",
                    targetableCondition: (player, owner) =>
                    {
                        return player.Character.team == CharacterTeam.Shadow
                            && player.Wound.Value != 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Healed(1);
                    })),

                CreateVisionCard("card.vision.vision_reconfortante", CardType.Vision, "card.vision.vision_reconfortante.description", false,
                new CardEffect("card.vision.vision_reconfortante.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Wound.Value == 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1, player, false);
                    }),
                new CardEffect("card.vision.vision_reconfortante.effect.args.heal&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Wound.Value != 0
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Healed(1);
                    })),

                CreateVisionCard("card.vision.vision_foudroyante", CardType.Vision, "card.vision.vision_foudroyante.description", false,
                new CardEffect("card.vision.vision_foudroyante.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return player.Character.team == CharacterTeam.Shadow
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1, player, false);
                    })),

                CreateVisionCard("card.vision.vision_mortifere", CardType.Vision, "card.vision.vision_mortifere.description", false,
                new CardEffect("card.vision.vision_mortifere.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1, player, false);
                    })),

                CreateVisionCard("card.vision.vision_mortifere", CardType.Vision, "card.vision.vision_mortifere.description", false,
                new CardEffect("card.vision.vision_mortifere.effect.args.wound&1",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(1, player, false);
                    })),

                CreateVisionCard("card.vision.vision_purificatrice", CardType.Vision, "card.vision.vision_purificatrice.description", false,
                new CardEffect("card.vision.vision_purificatrice.effect.args.wound&2",
                    targetableCondition: (player, owner) =>
                    {
                        return player.Character.team == CharacterTeam.Shadow
                            && player == owner;
                    },
                    effect: (player, owner, card) =>
                    {
                        player.Wounded(2, player, false);
                    })),

                CreateVisionCard("card.vision.vision_supreme", CardType.Vision, "card.vision.vision_supreme.description", false,
                new CardEffect("card.vision.vision_supreme.effect.show_card",
                    targetableCondition: (player, owner) =>
                    {
                        return owner == player;
                    },
                    effect: (player, owner, card) =>
                    {
                        if (GameManager.LocalPlayer.Value == player)
                        {
                            EventView.Manager.Emit(new ShowCharacterCardEvent(player.Character.characterName, player.Id));
                        }
                    }))
            };
        }

        /// <summary>
        /// Fonction qui va instancier une carte équipement.
        /// </summary>
        /// <param name="cardLabel">Label de la carte</param>
        /// <param name="cardType">Type de la carte</param>
        /// <param name="description">Sa description</param>
        /// <param name="condition">Condition d'usage de la carte</param>
        /// <param name="effect">L'effet de la carte</param>
        /// <returns> Renvoie une EquipmentCard</returns>
        public EquipmentCard CreateEquipmentCard(string cardLabel, CardType cardType, string description, EquipmentCondition condition, EquipmentAddEffect addeffect, EquipmentRemoveEffect rmeffect)
        {
            EquipmentCard c = new EquipmentCard(cardLabel, cardType, description, cards.Count,
                equipe: (player, card) =>
                {
                    player.AddCard(card);
                    if (card.condition(player))
                        card.addEffect(player, card);
                }, 
                unequipe: (player, card) =>
                {
                    player.RemoveCard(player.HasCard(card.cardLabel));
                    card.rmEffect(player, card);
                }, 
                condition, addeffect, rmeffect);
            cards.Add(c);
            return c;
        }

        /// <summary>
        /// Fonction qui va instancier une carte à usage unique.
        /// </summary>
        /// <param name="cardLabel">Label de la carte</param>
        /// <param name="cardType">Type de la carte</param>
        /// <param name="description">Sa description</param>
        /// <param name="canDismiss">Booléen pour dire si la carte peut être défaussée</param>
        /// <param name="cardEffect">Tableau d'effets de la carte</param>
        /// <returns> Renvoie une EquipmentCard</returns>
        public UsableCard CreateUsableCard(string cardLabel, CardType cardType, string description, bool canDismiss, params CardEffect[] cardEffect)
        {
            int id = cards.Count;
            UsableCard c = new UsableCard(cardLabel, cardType, description, id, canDismiss, cardEffect);
            cards.Add(c);
            return c;
        }

        /// <summary>
        /// Fonction qui va instancier une carte à usage unique.
        /// </summary>
        /// <param name="cardLabel">Label de la carte</param>
        /// <param name="cardType">Type de la carte</param>
        /// <param name="description">Sa description</param>
        /// <param name="canDismiss">Booléen pour dire si la carte peut être défaussée</param>
        /// <param name="hiddenChoices">Booléen pour dire si le choix peut être dissimulé</param>
        /// <param name="cardEffect">Tableau d'effets de la carte</param>
        /// <returns> Renvoie une EquipmentCard</returns>
        public UsableCard CreateUsableCard(string cardLabel, CardType cardType, string description, bool canDismiss, bool hiddenChoices, params CardEffect[] cardEffect)
        {
            int id = cards.Count;
            UsableCard c = new UsableCard(cardLabel, cardType, description, id, canDismiss, hiddenChoices, cardEffect);
            cards.Add(c);
            return c;
        }

        /// <summary>
        /// Créé deux carte vision :
        ///     Celle que pioche le joueur courant
        ///     Celle qui est envoyé par la première au joueur ciblé.
        /// </summary>
        /// <param name="cardLabel"></param>
        /// <param name="cardType"></param>
        /// <param name="description"></param>
        /// <param name="canDismiss"></param>
        /// <param name="cardEffect"></param>
        /// <returns>La carte vision à envoyer au joueur qui a pioché</returns>
        public UsableCard CreateVisionCard(string cardLabel, CardType cardType, string description, bool canDismiss, params CardEffect[] cardEffect)
        {
            List<CardEffect> effects = new List<CardEffect>(cardEffect);
            int effectscount = effects.Count;
            effects.Add(new CardEffect(cardLabel + ".nothing_happen",
                effect: (target, player, card) => { },
                targetableCondition: (target, owner) => 
                {
                    bool nothing_available = true;
                    for (int i = 0; i < effectscount; i++)
                    {
                        if ((!effects[i].targetableCondition(target, owner) || target.Character.characterName.Equals("character.name.metamorphe")) && owner == target && !target.Dead.Value)
                        {

                        }
                        else
                        {
                            nothing_available = false;
                            break;
                        }
                    }
                    return nothing_available;
                }
                ));
            int id = cards.Count;
            UsableCard auxilaire = new UsableCard(cardLabel, cardType, description, id, canDismiss, effects.ToArray());
            cards.Add(auxilaire);

            id = cards.Count;
            UsableCard vision = new UsableCard(cardLabel, cardType, description, id, canDismiss,
                new CardEffect("card.vision.effect.send.&" + cardLabel,
                effect: (target, player, card) =>
                {
                    GameManager.TurnEndable.Value = false;
                    EventView.Manager.Emit(new SelectUsableCardPickedEvent(auxilaire.Id, true, target.Id));
                },
                targetableCondition: (target, owner) =>
                {
                    return target != owner
                        && !target.Dead.Value;
                }
                ));
            cards.Add(vision);
            return vision;
        }
    }
}
