using Assets.Noyau.Cards.model;
using EventSystem;
using Scripts.event_out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Cards.controller
{
    public static class GVisionCard
    {
        public static VisionCard VisionDestructrice = new VisionCard
            (
                condition: (player) =>
                {
                    return player.Character.characterHP >= 12;
                },
                effect: (player) =>
                {
                    player.Wounded(1);
                }
            );

        public static VisionCard VisionClairvoyante = new VisionCard
            (
                condition: (player) =>
                {
                    return player.Character.characterHP <= 11;
                },
                effect: (player) =>
                {
                    player.Wounded(2);
                }
            );

        public static VisionCard VisionCupide = new VisionCard
            (
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
                }
            );

        public static VisionCard VisionEnivrante = new VisionCard
            (
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
                }
            );

        public static VisionCard VisionFurtive = new VisionCard
            (
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
                }
            );





        public static VisionCard VisionDivine = new VisionCard
            (
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
                }
            );
        
        public static VisionCard VisionLugubre = new VisionCard
             (
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
                }
             );
        
        public static VisionCard VisionReconfortante = new VisionCard
             (
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
                }
             );

        public static VisionCard VisionFoudroyante = new VisionCard
             (
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Shadow;
                },
                effect: (player) =>
                {
                    player.Wounded(1);
                }
             );

        public static VisionCard VisionMortifere = new VisionCard
             (
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Hunter;
                },
                effect: (player) =>
                {
                    player.Wounded(1);
                }
             );

        public static VisionCard VisionPurificatrice = new VisionCard
             (
                condition: (player) =>
                {
                    return player.Character.team == CharacterTeam.Shadow;
                },
                effect: (player) =>
                {
                    player.Wounded(2);
                }
             );

        public static VisionCard VisionSupreme = new VisionCard
             (
                condition: (player) =>
                {
                    return true;
                },
                effect: (player) =>
                {
                    // Montrer la carte au joueur
                }
             );
    }
}
