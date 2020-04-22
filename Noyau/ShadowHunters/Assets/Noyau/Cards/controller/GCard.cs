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
        public List<LightCard> lightDeck;
        public List<DarknessCard> darknessDeck;

        public UsableCard Antre;
        public UsableCard Monastere;
        public UsableCard Cimetiere;

        public GCard()
        {
            Antre = CreateUsableCard("card.location.antre", CardType.Location, "card.location.antre.description", true,
                new CardEffect("card.location.antre.effect.pickvision",
                effect: (target) => 
                {
                    VisionCard c = CardView.PickVision();
                    EventView.Manager.Emit(new SelectVisionTargetEvent()
                    { 
                        PlayerId=GameManager.PlayerTurn.Value.Id,
                        cardId=c.Id 
                    });
                },
                targetableCondition:null
                ));

            Monastere = CreateUsableCard("card.location.monastere", CardType.Location, "card.location.monastere.description", true,
                new CardEffect("card.location.monastere.effect.picklight",
                effect: (target) =>
                {
                    LightCard c = CardView.PickLight();
                },
                targetableCondition: null
                ));

            Antre = CreateUsableCard("card.location.antre", CardType.Location, "card.location.antre.description", true,
                new CardEffect("card.location.antre.effect.pickdarkness",
                effect: (target) =>
                {
                    DarknessCard c = CardView.PickDarkness();
                },
                targetableCondition: null
                ));



            darknessDeck = new List<DarknessCard>()
            {
                CreateDarkness("card.darkness.darkness_araignee", "card.darkness.darkness_araignee.description",
                condition: (player) =>
                {
                    return true;
                },
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
                }),

                CreateDarkness("card.darkness.darkness_banane", "card.darkness.darkness_banane.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    bool hasEquip = false;

                    foreach (Card c in player.ListCard)
                        if (c is IEquipment)
                            hasEquip = true;

                    if (hasEquip)
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
                        player.Wounded(1);
                }),

                CreateDarkness("card.darkness.darkness_chauve_souris", "card.darkness.darkness_chauve_souris.description",
                condition: (player) =>
                {
                    return true;
                },
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
                }),

                CreateDarkness("card.darkness.darkness_dynamite", "card.darkness.darkness_dynamite.description",
                condition: (player) =>
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
                            if (p.Position == area && !p.HasAmulet.Value)
                                p.Wounded(3);
                }),

                CreateDarkness("card.darkness.darkness_hache", "card.darkness.darkness_hache.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.BonusAttack.Value++;
                }),

                CreateDarkness("card.darkness.darkness_mitrailleuse", "card.darkness.darkness_mitrailleuse.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasGatling.Value = true;
                }),

                CreateDarkness("card.darkness.darkness_poupee", "card.darkness.darkness_poupee.description",
                condition: (player) =>
                {
                    return true;
                },
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
                }),

                CreateDarkness("card.darkness.darkness_revolver", "card.darkness.darkness_revolver.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasRevolver.Value = true;
                }),

                CreateDarkness("card.darkness.darkness_rituel", "card.darkness.darkness_rituel.description",
                condition: (player) =>
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
                            EffectCard = card
                        });
                }),

                CreateDarkness("card.darkness.darkness_sabre", "card.darkness.darkness_sabre.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasSaber.Value = true;
                }),

                CreateDarkness("card.darkness.darkness_succube", "card.darkness.darkness_succube.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    List<int> choices = new List<int>();

                    foreach (Player p in PlayerView.GetPlayers())
                        if (!player.Dead.Value && p.Id != player.Id && p.ListCard.Count > 0)
                            foreach (Card c in player.ListCard)
                                if (c is IEquipment)
                                {
                                    choices.Add(player.Id);
                                    break;
                                }

                    if (choices.Count != 0)
                    {
                        EventView.Manager.Emit(new SelectStealCardEvent()
                        {
                            PlayerId = player.Id,
                            PossiblePlayerTargetId = choices.ToArray()
                        });
                    }
                }),
            };

            lightDeck = new List<LightCard>()
            {
                CreateLight("card.light.light_amulette", "card.light.light_amulette.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasAmulet.Value = true;
                }),

                CreateLight("card.light.light_ange_gardien", "card.light.light_ange_gardien.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasGuardian.Value = true;
                }),

                CreateLight("card.light.light_supreme", "card.light.light_supreme.description",
                condition: (player) =>
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
                }),

                CreateLight("card.light.light_chocolat", "card.light.light_chocolat.description",
                condition: (player) =>
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
                }),

                CreateLight("card.light.light_benediction", "card.light.light_benediction.description",
                condition: (player) =>
                {
                    return true;
                },
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
                }),

                CreateLight("card.light.light_boussole", "card.light.light_boussole.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasCompass.Value = true;
                }),

                CreateLight("card.light.light_broche", "card.light.light_broche.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasBroche.Value = true;
                }),

                CreateLight("card.light.light_crucifix", "card.light.light_crucifix.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasCrucifix.Value = true;
                }),

                CreateLight("card.light.light_eclair", "card.light.light_eclair.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    foreach (Player p in PlayerView.GetPlayers())
                        if (p.Id != player.Id)
                            p.Wounded(2);
                }),
                
                CreateLight("card.light.light_lance", "card.light.light_lance.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasSpear.Value = true;
                    if (player.Character.team == CharacterTeam.Hunter && player.Revealed.Value)
                        player.BonusAttack.Value += 2;
                }),

                CreateLight("card.light.light_miroir", "card.light.light_miroir.description",
                condition: (player) =>
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
                }),

                CreateLight("card.light.light_premiers_secours", "card.light.light_premiers_secours.description",
                condition: (player) =>
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
                }),

                CreateLight("card.light.light_savoir", "card.light.light_savoir.description",
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasAncestral.Value = true;
                }),

                CreateLight("card.light.light_toge", "card.light.light_toge.description",
                condition: (player) =>
                {
                    return true;
                },
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
                    player.Wounded(1);
                }),

                CreateVision("card.vision.vision_clairvoyante", "card.vision.vision_clairvoyante.description",
                condition: (player) =>
                {
                    return player.Character.characterHP <= 11;
                },
                effect: (player) =>
                {
                    player.Wounded(2);
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
                        player.Wounded(1);
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
                        player.Wounded(1);
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
                        player.Wounded(1);
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
                        player.Wounded(1);
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
                    player.Wounded(1);
                }),

                CreateVision("card.vision.vision_mortifere", "card.vision.vision_mortifere.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Hunter;
                },
                effect: (player) =>
                {
                    player.Wounded(1);
                }),

                CreateVision("card.vision.vision_purificatrice", "card.vision.vision_purificatrice.description",
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Shadow;
                },
                effect: (player) =>
                {
                    player.Wounded(2);
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

        public UsableCard CreateUsableCard(string cardLabel, CardType cardType, string description, bool canDismiss, params CardEffect[] cardEffect)
        {
            int id = cards.Count;
            UsableCard c = new UsableCard(cardLabel, cardType, description, id, canDismiss, cardEffect);
            cards.Add(c);
            return c;
        }
    }
}
