using Assets.Noyau.Cards.model;
using Assets.Noyau.Cards.view;
using Assets.Noyau.Manager.view;
using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scripts.event_out;
using Assets.Noyau.Players.view;
using Scripts.event_in;

namespace Assets.Noyau.Cards.controller
{
    public class GCard
    {
        public List<Card> cards = new List<Card>();

        public List<VisionCard> visionDeck;
        public List<Card> lightDeck;
        public List<Card> darknessDeck;

        public UsableCard Foret;
        public UsableCard Sanctuaire;

        public GCard()
        {
            Foret = CreateUsableCard("card.location.foret", CardType.Location, "card.location.foret.description", true,
                new CardEffect("card.location.foret.effect.wound",
                    effect : (target, card) =>
                    {
                        target.Wounded(2, GameManager.PlayerTurn.Value, false);
                    },
                    targetableCondition: (target) =>
                    {
                        return !target.HasBroche.Value;
                    }
                ),
                new CardEffect("card.location.foret.effect.heal",
                    effect: (target, card) =>
                    {
                        target.Healed(1);
                    },
                    targetableCondition: (target) =>
                    {
                        return true;
                    }
                ));

            Sanctuaire = CreateUsableCard("card.location.sanctuaire", CardType.Location, "card.location.sanctuaire.description", true,
                new CardEffect("card.location.sanctuaire.effect.steal",
                    effect: (target, card) =>
                    {
                        List<(Card equipment, Player owner)> equipments = new List<(Card equipment, Player owner)>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != target.Id && p.ListCard.Count > 0)
                                foreach (Card c in p.ListCard)
                                    if (c is EquipmentCard)
                                        equipments.Add((c, p));

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



            darknessDeck = new List<Card>()
            {
                CreateUsableCard("card.darkness.darkness_araignee", CardType.Darkness, "card.darkness.darkness_araignee.description", false,
                new CardEffect("card.darkness.darkness_araignee.effect",
                    targetableCondition: null,
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!player.Dead.Value && p.Id != player.Id)
                                if (!player.HasAmulet.Value)
                                    players.Add(player.Id);

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
                    targetableCondition: null,
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
                    targetableCondition: null,
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!player.Dead.Value && p.Id != player.Id)
                                if (!player.HasAmulet.Value)
                                    players.Add(player.Id);

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
                    targetableCondition: null,
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!player.Dead.Value && p.Id != player.Id)
                                if (!player.HasAmulet.Value)
                                    players.Add(player.Id);

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
                    targetableCondition: null,
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!player.Dead.Value && p.Id != player.Id)
                                if (!player.HasAmulet.Value)
                                    players.Add(player.Id);

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
                    targetableCondition: null,
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
                                if (p.Position == area && !p.HasAmulet.Value)
                                    p.Wounded(3, GameManager.PlayerTurn.Value, false);
                    })),

                CreateEquipmentCard("card.darkness.darkness_hache", CardType.Darkness, "card.darkness.darkness_hache.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                {
                    player.BonusAttack.Value++;
                }),

                CreateEquipmentCard("card.darkness.darkness_hachoir", CardType.Darkness, "card.darkness.darkness_hachoir.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        player.BonusAttack.Value++;
                    }),

                CreateEquipmentCard("card.darkness.darkness_mitrailleuse", CardType.Darkness, "card.darkness.darkness_mitrailleuse.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        player.HasGatling.Value = true;
                    }),

                CreateUsableCard("card.darkness.darkness_poupee", CardType.Darkness, "card.darkness.darkness_poupee.description", false,
                new CardEffect("card.darkness.darkness_poupee.effect",
                    targetableCondition: null,
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!player.Dead.Value && p.Id != player.Id)
                                players.Add(player.Id);

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
                    effect: (player, card) =>
                    {
                        player.HasRevolver.Value = true;
                    }),

                CreateUsableCard("card.darkness.darkness_rituel", CardType.Darkness, "card.darkness.darkness_rituel.description", false,
                new CardEffect("card.darkness.darkness_rituel.effect",
                    targetableCondition: null,
                    effect: (player, card) =>
                    {
                        if (player.Revealed.Value && player.Character.team == CharacterTeam.Shadow)
                            player.Healed(player.Wound.Value);
                        else
                            EventView.Manager.Emit(new SelectRevealOrNotEvent()
                            {
                                PlayerId = player.Id,
                                EffectCard = card
                            });
                    })),

                CreateEquipmentCard("card.darkness.darkness_sabre", CardType.Darkness, "card.darkness.darkness_sabre.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        player.HasSaber.Value = true;
                    }),

                CreateUsableCard("card.darkness.darkness_succube", CardType.Darkness, "card.darkness.darkness_succube.description", false,
                new CardEffect("card.darkness.darkness_succube.effect",
                    targetableCondition: null,
                    effect: (player, card) =>
                    {
                        List<(Card equipment, Player owner)> equipments = new List<(Card equipment, Player owner)>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != player.Id && p.ListCard.Count > 0)
                                foreach (Card c in p.ListCard)
                                    if (c is EquipmentCard)
                                        equipments.Add((c, p));

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
                    targetableCondition: null,
                    effect: (player, card) =>
                    {
                        List<(Card equipment, Player owner)> equipments = new List<(Card equipment, Player owner)>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!p.Dead.Value && p.Id != player.Id && p.ListCard.Count > 0)
                                foreach (Card c in p.ListCard)
                                    if (c is EquipmentCard)
                                        equipments.Add((c, p));

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
                    effect: (player, card) =>
                    {
                        player.BonusAttack.Value++;
                    }),
            };

            lightDeck = new List<Card>()
            {
                CreateEquipmentCard("card.light.light_amulette", CardType.Light, "card.light.light_amulette.description",
                    condition: (player) =>
                    {
                        return true;
                    },
                    effect: (player, card) =>
                    {
                        player.HasAmulet.Value = true;
                    }),

                CreateUsableCard("card.light.light_ange_gardien", CardType.Light, "card.light.light_ange_gardien.description", false,
                new CardEffect("card.light.light_ange_gardien.effect",
                    targetableCondition: null,
                    effect: (player, card) =>
                    {
                        player.HasGuardian.Value = true;
                    })),

                CreateUsableCard("card.light.light_supreme", CardType.Light, "card.light.light_supreme.description", false,
                new CardEffect("card.light.light_supreme.effect",
                    targetableCondition: (player) =>
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
                    targetableCondition: (player) =>
                    {
                        return player.Character.characterName == "Allie"
                                || player.Character.characterName == "Emi"
                                || player.Character.characterName == "Metamorphe";
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
                    targetableCondition: null,
                    effect: (player, card) =>
                    {
                        List<int> players = new List<int>();

                        foreach (Player p in PlayerView.GetPlayers())
                            if (!player.Dead.Value && p.Id != player.Id)
                                players.Add(p.Id);

                        EventView.Manager.Emit(new SelectLightCardTargetEvent()
                        {
                            PlayerId = player.Id,
                            PossibleTargetId = players.ToArray(),
                            LightCard = card
                        });
                    })),

                CreateEquipmentCard("card.light.light_boussole", CardType.Light, "card.light.light_boussole.description",
                    condition: null,
                    effect: (player, card) =>
                    {
                        player.HasCompass.Value = true;
                    }),

                CreateEquipmentCard("card.light.light_broche", CardType.Light, "card.light.light_broche.description",
                    condition: null,
                    effect: (player, card) =>
                    {
                        player.HasBroche.Value = true;
                    }),

                CreateEquipmentCard("card.light.light_crucifix", CardType.Light, "card.light.light_crucifix.description",
                    condition: null,
                    effect: (player, card) =>
                    {
                        player.HasCrucifix.Value = true;
                    }),

                CreateUsableCard("card.light.light_eclair", CardType.Light, "card.light.light_eclair.description", false,
                new CardEffect("card.light.light_eclair.effect",
                    targetableCondition: null,
                    effect: (player, card) =>
                    {
                        foreach (Player p in PlayerView.GetPlayers())
                            if (p.Id != player.Id)
                                p.Wounded(2, player, false);
                    })),

                CreateUsableCard("card.light.light_eau_benite", CardType.Light, "card.light.light_eau_benite.description", false,
                new CardEffect("card.light.light_eau_benite.effect",
                    targetableCondition: null,
                    effect: (player, card) =>
                    {
                        player.Healed(2);
                    })),

                CreateEquipmentCard("card.light.light_lance", CardType.Light, "card.light.light_lance.description",
                    condition: null,
                    effect: (player, card) =>
                    {
                        player.HasSpear.Value = true;
                        if (player.Character.team == CharacterTeam.Hunter && player.Revealed.Value)
                            player.BonusAttack.Value += 2;
                    }),

                CreateUsableCard("card.light.light_miroir", CardType.Light, "card.light.light_miroir.description", false,
                new CardEffect("card.light.light_miroir.effect",
                    targetableCondition: (player) =>
                    {
                        return !player.Revealed.Value
                            && player.Character.team == CharacterTeam.Shadow
                            && player.Character.characterName != "Metamorphe";
                    },
                    effect: (player, card) =>
                {
                    //RevealCard(m_players[idPlayer]);
                    EventView.Manager.Emit(new RevealCard()
                    {
                        PlayerId = player.Id
                    });
                })),

                CreateUsableCard("card.light.light_premiers_secours", CardType.Light, "card.light.light_premiers_secours.description", false,
                new CardEffect("card.light.light_premiers_secours.effect",
                    targetableCondition: null,
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
                    targetableCondition: null,
                    effect: (player, card) =>
                {
                    player.HasAncestral.Value = true;
                })),

                CreateEquipmentCard("card.light.light_toge", CardType.Light, "card.light.light_toge.description",
                    condition: null,
                    effect: (player, card) =>
                {
                    player.HasToge.Value = true;
                    player.MalusAttack.Value++;
                    player.ReductionWounds.Value = 1;
                })
            };

            visionDeck = new List<VisionCard>()
            {
                CreateVision("card.vision.vision_destructrice", "card.vision.vision_destructrice.description", 
                condition: (player) =>
                {
                    return player.Character.characterHP >= 12;
                },
                effect: (player) =>
                {
                    player.Wounded(1, player, false);
                }),

                CreateVision("card.vision.vision_clairvoyante", "card.vision.vision_clairvoyante.description",
                condition: (player) =>
                {
                    return player.Character.characterHP <= 11;
                },
                effect: (player) =>
                {
                    player.Wounded(2, player, false);
                }),

                CreateVision("card.vision.vision_cupide", "card.vision.vision_cupide.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Neutral
                        || player.Character.team == CharacterTeam.Shadow;
                },
                effect: (player) =>
                {
                    EventView.Manager.Emit(new SelectGiveOrWoundEvent()
                    {
                        PlayerId = player.Id
                    });
                }),

                CreateVision("card.vision.vision_cupide", "card.vision.vision_cupide.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Neutral
                        || player.Character.team == CharacterTeam.Shadow;
                },
                effect: (player) =>
                {
                    EventView.Manager.Emit(new SelectGiveOrWoundEvent()
                    {
                        PlayerId = player.Id
                    });
                }),

                CreateVision("card.vision.vision_enivrante", "card.vision.vision_enivrante.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Neutral
                        || player.Character.team == CharacterTeam.Hunter;
                },
                effect: (player) =>
                {
                    EventView.Manager.Emit(new SelectGiveOrWoundEvent()
                    {
                        PlayerId = player.Id
                    });
                }),

                CreateVision("card.vision.vision_enivrante", "card.vision.vision_enivrante.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Neutral
                        || player.Character.team == CharacterTeam.Hunter;
                },
                effect: (player) =>
                {
                    EventView.Manager.Emit(new SelectGiveOrWoundEvent()
                    {
                        PlayerId = player.Id
                    });
                }),

                CreateVision("card.vision.vision_furtive", "card.vision.vision_furtive.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Shadow
                        || player.Character.team == CharacterTeam.Hunter;
                },
                effect: (player) =>
                {
                    EventView.Manager.Emit(new SelectGiveOrWoundEvent()
                    {
                        PlayerId = player.Id
                    });
                }),

                CreateVision("card.vision.vision_furtive", "card.vision.vision_furtive.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Shadow
                        || player.Character.team == CharacterTeam.Hunter;
                },
                effect: (player) =>
                {
                    EventView.Manager.Emit(new SelectGiveOrWoundEvent()
                    {
                        PlayerId = player.Id
                    });
                }),

                CreateVision("card.vision.vision_divine", "card.vision.vision_divine.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Hunter;
                },
                effect: (player) =>
                {
                    if (player.Wound.Value == 0)
                        player.Wounded(1, player, false);
                    else
                        player.Healed(1);
                }),

                CreateVision("card.vision.vision_divine", "card.vision.vision_divine.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Hunter;
                },
                effect: (player) =>
                {
                    if (player.Wound.Value == 0)
                        player.Wounded(1, player, false);
                    else
                        player.Healed(1);
                }),

                CreateVision("card.vision.vision_lugubre", "card.vision.vision_lugubre.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Shadow;
                },
                effect: (player) =>
                {
                    if (player.Wound.Value == 0)
                        player.Wounded(1, player, false);
                    else
                        player.Healed(1);
                }),

                CreateVision("card.vision.vision_reconfortante", "card.vision.vision_reconfortante.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Neutral;
                },
                effect: (player) =>
                {
                    if (player.Wound.Value == 0)
                        player.Wounded(1, player, false);
                    else
                        player.Healed(1);
                }),

                CreateVision("card.vision.vision_foudroyante", "card.vision.vision_foudroyante.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Shadow;
                },
                effect: (player) =>
                {
                    player.Wounded(1, player, false);
                }),

                CreateVision("card.vision.vision_mortifere", "card.vision.vision_mortifere.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Hunter;
                },
                effect: (player) =>
                {
                    player.Wounded(1, player, false);
                }),

                CreateVision("card.vision.vision_mortifere", "card.vision.vision_mortifere.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Hunter;
                },
                effect: (player) =>
                {
                    player.Wounded(1, player, false);
                }),

                CreateVision("card.vision.vision_purificatrice", "card.vision.vision_purificatrice.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Shadow;
                },
                effect: (player) =>
                {
                    player.Wounded(2, player, false);
                }),

                CreateVision("card.vision.vision_supreme", "card.vision.vision_supreme.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player) =>
                {
                    // Montrer la carte au joueur
                })
        };
        }

        public VisionCard CreateVision(string cardLabel, string description, VisionCardCondition condition, VisionCardEffect effect)
        {
            int id = cards.Count;
            VisionCard c = new VisionCard(cardLabel, CardType.Vision, description, id, condition, effect);
            cards.Add(c);
            visionDeck.Add(c);
            return c;
        }

        public LightCard CreateLight(string cardLabel, string description, LightCardCondition condition, LightCardEffect effect)
        {
            int id = cards.Count;
            LightCard c = new LightCard(cardLabel, CardType.Vision, description, id, condition, effect);
            cards.Add(c);
            lightDeck.Add(c);
            return c;
        }

        public DarknessCard CreateDarkness(string cardLabel, string description, DarknessCardCondition condition, DarknessCardEffect effect)
        {
            int id = cards.Count;
            DarknessCard c = new DarknessCard(cardLabel, CardType.Vision, description, id, condition, effect);
            cards.Add(c);
            darknessDeck.Add(c);
            return c;
        }

        public EquipmentCard CreateEquipmentCard(string cardLabel, CardType cardType, string description, EquipmentCondition condition, EquipmentEffect effect)
        {
            EquipmentCard c = new EquipmentCard(cardLabel, cardType, description, cards.Count, null, null, condition, effect);
            cards.Add(c);
            return c;
        }

        public UsableCard CreateUsableCard(string cardLabel, CardType cardType, string description, bool canDismiss, params CardEffect[] cardEffect)
        {
            int id = cards.Count;
            UsableCard c = new UsableCard(cardLabel, cardType, description, id, canDismiss, cardEffect);
            cards.Add(c);
            return c;
        }
    }
}
