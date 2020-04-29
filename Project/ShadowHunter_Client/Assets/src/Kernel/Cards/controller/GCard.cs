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

        public UsableCard Foret;
        public UsableCard Sanctuaire;

        public GCard()
        {
            /// <summary>
            /// Fonction qui va instancier le lieu Forêt avec ses pouvoirs comme une carte à usage unique.
            /// </summary>
            /// <returns> Renvoie un UsableCard</returns>
            Foret = CreateUsableCard("card.location.foret", CardType.Location, "card.location.foret.description", true,
                new CardEffect("card.location.foret.effect",
                    effect : (target, card) =>
                    {
                        EventView.Manager.Emit(new SelectForestPowerEvent()
                        {
                            PlayerId = target.Id
                        });
                    },
                    targetableCondition: (target, owner) =>
                    {
                        return true;
                    }
                ));

            /// <summary>
            /// Fonction qui va instancier le lieu Sanctuaire avec ses pouvoirs comme une carte à usage unique.
            /// </summary>
            /// <returns> Renvoie un UsableCard</returns>
            Sanctuaire = CreateUsableCard("card.location.sanctuaire", CardType.Location, "card.location.sanctuaire.description", true,
                new CardEffect("card.location.sanctuaire.effect",
                    effect: (target, card) =>
                    {
                        List<(int equipment, int owner)> equipments = new List<(int equipment, int owner)>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != target.Id && p.ListCard.Count > 0)
                                foreach (Card c in p.ListCard)
                                    if (c is EquipmentCard)
                                        equipments.Add((c.Id, p.Id));

                        if (equipments.Count != 0)
                        {
                            EventView.Manager.Emit(new SelectStealCardEvent()
                            {
                                PlayerId = target.Id,
                                PossiblePlayerTargetId = equipments.ToArray()
                            });
                        }
                    },
                    targetableCondition: null
                ));


            /// <summary>
            /// Fonction qui va instancier les cartes ténèbres avec leur pouvoirs.
            /// </summary>
            /// <returns> Renvoie une liste de Card</returns>
            darknessDeck = new List<Card>()
            {
                CreateUsableCard("card.darkness.darkness_araignee", CardType.Darkness, "card.darkness.darkness_araignee.description", false,
                new CardEffect("card.darkness.darkness_araignee.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != player.Id)
                                if (!p.HasAmulet.Value)
                                    players.Add(p.Id);

                        EventView.Manager.Emit(new SelectPlayerTakingWoundsEvent()
                        {
                            PlayerId = player.Id,
                            PossibleTargetId = players.ToArray(),
                            IsPuppet = false,
                            NbWoundsTaken = 2,
                            NbWoundsSelfHealed = -2
                        });
                    })),

                CreateUsableCard("card.darkness.darkness_banane", CardType.Darkness, "card.darkness.darkness_banane.description", false,
                new CardEffect("card.darkness.darkness_banane.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        bool hasEquip = false;

                        foreach (Card c in player.ListCard)
                            if (c is EquipmentCard)
                                hasEquip = true;

                        if(hasEquip)
                        {
                            List<int> players = new List<int>();

                            foreach (Player p in PlayerView.GetPlayers())
                                if (!p.Dead.Value && p.Id != player.Id)
                                    players.Add(p.Id);

                            EventView.Manager.Emit(new SelectGiveCardEvent()
                            {
                                PlayerId = player.Id,
                                PossibleTargetId = players.ToArray()
                            });
                        }
                        else
                        {
                            player.Wounded(1, player, false);
                        }
                    })),

                CreateUsableCard("card.darkness.darkness_chauve_souris", CardType.Darkness, "card.darkness.darkness_chauve_souris.description", false,
                new CardEffect("card.darkness.darkness_chauve_souris.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != player.Id)
                                if (!p.HasAmulet.Value)
                                    players.Add(p.Id);

                        EventView.Manager.Emit(new SelectPlayerTakingWoundsEvent()
                        {
                            PlayerId = player.Id,
                            PossibleTargetId = players.ToArray(),
                            IsPuppet = false,
                            NbWoundsTaken = 2,
                            NbWoundsSelfHealed = 1
                        });
                    })),

                CreateUsableCard("card.darkness.darkness_chauve_souris", CardType.Darkness, "card.darkness.darkness_chauve_souris.description", false,
                new CardEffect("card.darkness.darkness_chauve_souris.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != player.Id)
                                if (!p.HasAmulet.Value)
                                    players.Add(p.Id);

                        EventView.Manager.Emit(new SelectPlayerTakingWoundsEvent()
                        {
                            PlayerId = player.Id,
                            PossibleTargetId = players.ToArray(),
                            IsPuppet = false,
                            NbWoundsTaken = 2,
                            NbWoundsSelfHealed = 1
                        });
                    })),

                CreateUsableCard("card.darkness.darkness_chauve_souris", CardType.Darkness, "card.darkness.darkness_chauve_souris.description", false,
                new CardEffect("card.darkness.darkness_chauve_souris.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != player.Id)
                                if (!p.HasAmulet.Value)
                                    players.Add(p.Id);

                        EventView.Manager.Emit(new SelectPlayerTakingWoundsEvent()
                        {
                            PlayerId = player.Id,
                            PossibleTargetId = players.ToArray(),
                            IsPuppet = false,
                            NbWoundsTaken = 2,
                            NbWoundsSelfHealed = 1
                        });
                    })),

                CreateUsableCard("card.darkness.darkness_dynamite", CardType.Darkness, "card.darkness.darkness_dynamite.description", false,
                new CardEffect("card.darkness.darkness_dynamite.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        int lancer = UnityEngine.Random.Range(1, 6) + UnityEngine.Random.Range(1, 4);
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
                new CardEffect("card.darkness.darkness_poupee.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != player.Id)
                                players.Add(p.Id);

                        EventView.Manager.Emit(new SelectPlayerTakingWoundsEvent()
                        {
                            PlayerId = player.Id,
                            PossibleTargetId = players.ToArray(),
                            IsPuppet = true,
                            NbWoundsTaken = 3,
                            NbWoundsSelfHealed = 0
                        });
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


                CreateUsableCard("card.darkness.darkness_rituel", CardType.Darkness, "card.darkness.darkness_rituel.description", false,
                new CardEffect("card.darkness.darkness_rituel.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        if (player.Revealed.Value && player.Character.team == CharacterTeam.Shadow)
                            player.Healed(player.Wound.Value);
                        else
                            EventView.Manager.Emit(new SelectRevealOrNotEvent()
                            {
                                PlayerId = player.Id,
                                EffectCard = card,
                                PowerDaniel = false,
                                PowerLoup = false
                            });
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
                new CardEffect("card.darkness.darkness_succube.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        List<(int equipment, int owner)> equipments = new List<(int equipment, int owner)>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != player.Id && p.ListCard.Count > 0)
                                foreach (Card c in p.ListCard)
                                    if (c is EquipmentCard)
                                        equipments.Add((c.Id, p.Id));

                        if (equipments.Count != 0)
                        {
                            EventView.Manager.Emit(new SelectStealCardEvent()
                            {
                                PlayerId = player.Id,
                                PossiblePlayerTargetId = equipments.ToArray()
                            });
                        }
                    })),

                CreateUsableCard("card.darkness.darkness_succube", CardType.Darkness, "card.darkness.darkness_succube.description", false,
                new CardEffect("card.darkness.darkness_succube.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        List<(int equipment, int owner)> equipments = new List<(int equipment, int owner)>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != player.Id && p.ListCard.Count > 0)
                                foreach (Card c in p.ListCard)
                                    if (c is EquipmentCard)
                                        equipments.Add((c.Id, p.Id));

                        if (equipments.Count != 0)
                        {
                            EventView.Manager.Emit(new SelectStealCardEvent()
                            {
                                PlayerId = player.Id,
                                PossiblePlayerTargetId = equipments.ToArray()
                            });
                        }
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
                new CardEffect("card.light.light_ange_gardien.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        player.HasGuardian.Value = true;
                    })),

                CreateUsableCard("card.light.light_supreme", CardType.Light, "card.light.light_supreme.description", false,
                new CardEffect("card.light.light_supreme.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return player.Character.team == CharacterTeam.Hunter;
                    },
                    effect: (player, card) =>
                    {
                        if (player.Revealed.Value)
                            player.Healed(player.Wound.Value);
                        else
                            EventView.Manager.Emit(new SelectRevealOrNotEvent()
                            {
                                PlayerId = player.Id,
                                EffectCard = card
                            });
                    })),

                CreateUsableCard("card.light.light_chocolat", CardType.Light, "card.light.light_chocolat.description", false,
                new CardEffect("card.light.light_chocolat.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return player.Character.characterName == "character.name.allie"
                                || player.Character.characterName == "character.name.emi"
                                || player.Character.characterName == "character.name.metamorphe";

                    },
                    effect: (player, card) =>
                    {
                        if (player.Revealed.Value)
                            player.Healed(player.Wound.Value);
                        else
                            EventView.Manager.Emit(new SelectRevealOrNotEvent()
                            {
                                PlayerId = player.Id,
                                EffectCard = card
                            });
                    })),

                CreateUsableCard("card.light.light_benediction", CardType.Light, "card.light.light_benediction.description", false,
                new CardEffect("card.light.light_benediction.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != player.Id)
                                players.Add(p.Id);

                        EventView.Manager.Emit(new SelectLightCardTargetEvent()
                        {
                            PlayerId = player.Id,
                            PossibleTargetId = players.ToArray(),
                            LightCard = card
                        });
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
                new CardEffect("card.light.light_eclair.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        foreach (Player p in PlayerView.GetPlayers())
                            if (p.Id != player.Id)
                                p.Wounded(2, player, false);
                    })),

                CreateUsableCard("card.light.light_eau_benite", CardType.Light, "card.light.light_eau_benite.description", false,
                new CardEffect("card.light.light_eau_benite.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
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
                new CardEffect("card.light.light_miroir.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return !player.Revealed.Value
                            && player.Character.team == CharacterTeam.Shadow
                            && player.Character.characterName != "Metamorphe";
                    },
                    effect: (player, card) =>
                    {
                        EventView.Manager.Emit(new RevealCardEvent()
                        {
                            PlayerId = player.Id
                        });
                    })),

                CreateUsableCard("card.light.light_premiers_secours", CardType.Light, "card.light.light_premiers_secours.description", false,
                new CardEffect("card.light.light_premiers_secours.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();
                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value)
                                players.Add(p.Id);

                        EventView.Manager.Emit(new SelectLightCardTargetEvent()
                        {
                            PlayerId = player.Id,
                            PossibleTargetId = players.ToArray(),
                            LightCard = card
                        });
                    })),

                CreateUsableCard("card.light.light_savoir", CardType.Light, "card.light.light_savoir.description", false,
                new CardEffect("card.light.light_savoir.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
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
                new CardEffect("card.vision.vision_destructrice.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.characterHP >= 12
                                || player.Character.characterName.Equals("character.name.metamorphe"))
                                && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1, player, false);
                    })),

                CreateVisionCard("card.vision.vision_clairvoyante", CardType.Vision, "card.vision.vision_clairvoyante.description", false,
                new CardEffect("card.vision.vision_clairvoyante.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.characterHP <= 11
                                || player.Character.characterName.Equals("character.name.metamorphe"))
                                && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(2, player, false);
                    })),

                CreateUsableCard("card.vision.vision_cupide", CardType.Vision, "card.vision.vision_cupide.description", false,
                new CardEffect("card.vision.vision_cupide.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Shadow)
                            && player.ListCard.Count > 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
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
                    }),
                new CardEffect("card.vision.vision_cupide.effect.wound",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Shadow)
                            && player.ListCard.Count == 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1,player,false);
                    })),


                CreateUsableCard("card.vision.vision_cupide", CardType.Vision, "card.vision.vision_cupide.description", false,
                new CardEffect("card.vision.vision_cupide.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Shadow)
                            && player.ListCard.Count > 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
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
                    }),
                new CardEffect("card.vision.vision_cupide.effect.wound",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Shadow)
                            && player.ListCard.Count == 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1,player,false);
                    })),

                CreateUsableCard("card.vision.vision_enivrante", CardType.Vision, "card.vision.vision_enivrante.description", false,
                new CardEffect("card.vision.vision_enivrante.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.ListCard.Count > 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
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
                    }),
                new CardEffect("card.vision.vision_enivrante.effect.wound",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.ListCard.Count == 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1,player,false);
                    })),


                CreateUsableCard("card.vision.vision_enivrante", CardType.Vision, "card.vision.vision_enivrante.description", false,
                new CardEffect("card.vision.vision_enivrante.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.ListCard.Count > 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
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
                    }),
                new CardEffect("card.vision.vision_enivrante.effect.wound",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.ListCard.Count == 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1,player,false);
                    })),

                CreateUsableCard("card.vision.vision_furtive", CardType.Vision, "card.vision.vision_furtive.description", false,
                new CardEffect("card.vision.vision_furtive.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Shadow
                            || player.Character.team == CharacterTeam.Hunter)
                            && player.ListCard.Count > 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
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
                    }),
                new CardEffect("card.vision.vision_furtive.effect.wound",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Shadow
                            || player.Character.team == CharacterTeam.Hunter)
                            && player.ListCard.Count == 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1,player,false);
                    })),


                CreateUsableCard("card.vision.vision_furtive", CardType.Vision, "card.vision.vision_furtive.description", false,
                new CardEffect("card.vision.vision_furtive.effect.give",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Shadow
                            || player.Character.team == CharacterTeam.Hunter)
                            && player.ListCard.Count > 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
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
                    }),
                new CardEffect("card.vision.vision_furtive.effect.wound",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Shadow
                            || player.Character.team == CharacterTeam.Hunter)
                            && player.ListCard.Count == 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1,player,false);
                    })),

                CreateUsableCard("card.vision.vision_divine", CardType.Vision, "card.vision.vision_divine.description", false,
                new CardEffect("card.vision.vision_divine.effect.wound",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Wound.Value == 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1, player, false);
                    }),
                new CardEffect("card.vision.vision_divine.effect.heal",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Wound.Value != 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Healed(1);
                    })),


                CreateUsableCard("card.vision.vision_divine", CardType.Vision, "card.vision.vision_divine.description", false,
                new CardEffect("card.vision.vision_divine.effect.wound",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Wound.Value == 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1, player, false);
                    }),
                new CardEffect("card.vision.vision_divine.effect.heal",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Wound.Value != 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Healed(1);
                    })),

                CreateVisionCard("card.vision.vision_lugubre", CardType.Vision, "card.vision.vision_lugubre.description", false,
                new CardEffect("card.vision.vision_lugubre.effect.wound",
                    targetableCondition: (player, owner) =>
                    {
                        return player.Character.team == CharacterTeam.Shadow
                            && player.Wound.Value == 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1, player, false);
                    }),
                new CardEffect("card.vision.vision_lugubre.effect.heal",
                    targetableCondition: (player, owner) =>
                    {
                        return player.Character.team == CharacterTeam.Shadow
                            && player.Wound.Value != 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Healed(1);
                    })),

                CreateVisionCard("card.vision.vision_reconfortante", CardType.Vision, "card.vision.vision_reconfortante.description", false,
                new CardEffect("card.vision.vision_reconfortante.effect.wound",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Wound.Value == 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1, player, false);
                    }),
                new CardEffect("card.vision.vision_reconfortante.effect.heal",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Neutral
                            || player.Character.characterName == "character.name.metamorphe")
                            && player.Wound.Value != 0
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Healed(1);
                    })),

                CreateVisionCard("card.vision.vision_foudroyante", CardType.Vision, "card.vision.vision_foudroyante.description", false,
                new CardEffect("card.vision.vision_foudroyante.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return player.Character.team == CharacterTeam.Shadow
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1, player, false);
                    })),

                CreateVisionCard("card.vision.vision_mortifere", CardType.Vision, "card.vision.vision_mortifere.description", false,
                new CardEffect("card.vision.vision_mortifere.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1, player, false);
                    })),

                CreateVisionCard("card.vision.vision_mortifere", CardType.Vision, "card.vision.vision_mortifere.description", false,
                new CardEffect("card.vision.vision_mortifere.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return (player.Character.team == CharacterTeam.Hunter
                            || player.Character.characterName == "character.name.metamorphe")
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(1, player, false);
                    })),

                CreateVisionCard("card.vision.vision_purificatrice", CardType.Vision, "card.vision.vision_purificatrice.description", false,
                new CardEffect("card.vision.vision_purificatrice.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return player.Character.team == CharacterTeam.Shadow
                            && player == owner;
                    },
                    effect: (player, card) =>
                    {
                        player.Wounded(2, player, false);
                    })),

                CreateVisionCard("card.vision.vision_supreme", CardType.Vision, "card.vision.vision_supreme.description", false,
                new CardEffect("card.vision.vision_supreme.effect",
                    targetableCondition: (player, owner) =>
                    {
                        return owner == player;
                    },
                    effect: (player, card) =>
                    {
                        // Montrer la carte au joueur
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


        public UsableCard CreateVisionCard(string cardLabel, CardType cardType, string description, bool canDismiss, params CardEffect[] cardEffect)
        {
            List<CardEffect> effects = new List<CardEffect>(cardEffect);
            effects.Add(new CardEffect(cardLabel + ".nothing_happen",
                effect: (target, card) => { },
                targetableCondition: (target, owner) => { return !effects[0].targetableCondition(target) || target.Character.characterName.Equals("character.name.metamorphe"); }
                ));

            UsableCard auxilaire = CreateUsableCard(cardLabel, cardType, description, canDismiss, effects.ToArray());
            int id = cards.Count;
            UsableCard vision = CreateUsableCard(cardLabel + ".select_target", cardType, description, canDismiss,
                new CardEffect("card.vision.effect.send&" + cardLabel,
                effect: (target, card) =>
                {
                    EventView.Manager.Emit(new SelectUsableCardPickedEvent(auxilaire.Id, true, target.Id));
                },
                targetableCondition: (target, owner) =>
                {
                    return target != owner;
                }
                ));
            cards.Add(vision);
            return vision;
        }
    }
}
