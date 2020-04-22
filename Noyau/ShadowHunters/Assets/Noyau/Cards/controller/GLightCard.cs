using Assets.Noyau.Cards.model;
using Assets.Noyau.Players.view;
using EventSystem;
using Scripts.event_out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Cards.controller
{
    public static class GLightCard
    {
        public static LightCard Amulette = new LightCard
            (
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasAmulet.Value = true;
                }
            );

        public static LightCard AngeGardien = new LightCard
            (
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasGuardian.Value = true;
                }
            );

        public static LightCard Supreme = new LightCard
            (
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    if (player.Revealed.Value && player.Character.team == CharacterTeam.Hunter)
                        player.Healed(player.Wound.Value);
                    else
                        EventView.Manager.Emit(new SelectRevealOrNotEvent()
                        {
                            PlayerId = player.Id,
                            EffectCard = card
                        });
                }
            );

        public static LightCard Chocolat = new LightCard
            (
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    if (player.Revealed.Value
                        && (player.Character.characterName == "Allie"
                            || player.Character.characterName == "Emi"
                            || player.Character.characterName == "Metamorphe"))
                        player.Healed(player.Wound.Value);
                    else
                        EventView.Manager.Emit(new SelectRevealOrNotEvent()
                        {
                            PlayerId = player.Id,
                            EffectCard = card
                        });
                }
            );

        public static LightCard Benediction = new LightCard
            (
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
                }
            );

        public static LightCard Boussole = new LightCard
            (
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasCompass.Value = true;
                }
            );

        public static LightCard Broche = new LightCard
            (
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasBroche.Value = true;
                }
            );

        public static LightCard Crucifix = new LightCard
            (
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasCrucifix.Value = true;
                }
            );

        public static LightCard Eclair = new LightCard
            (
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    foreach (Player p in PlayerView.GetPlayers())
                        if (p.Id != player.Id)
                            p.Wounded(2);
                }
            );            

        public static LightCard Lance = new LightCard
            (
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasSpear.Value = true;
                    if (player.Character.team == CharacterTeam.Hunter && player.Revealed.Value)
                        player.BonusAttack.Value += 2;
                }
            );

        public static LightCard Miroir = new LightCard
            (
                condition: (player) =>
                {
                    return !player.Revealed.Value
                        && player.Character.team == CharacterTeam.Shadow
                        && player.Character.characterName != "Metamorphe";
                },
                effect: (player, card) =>
                {
                    RevealCard(m_players[idPlayer]);
                }
            );

        public static LightCard PremiersSecours = new LightCard
            (
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
                }
            );

        public static LightCard Savoir = new LightCard
            (
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasAncestral.Value = true;
                }
            );

        public static LightCard Toge = new LightCard
            (
                condition: (player) =>
                {
                    return true;
                },
                effect: (player, card) =>
                {
                    player.HasToge.Value = true;
                    player.MalusAttack.Value++;
                    player.ReductionWounds.Value = 1;
                }
            );
    }
}
