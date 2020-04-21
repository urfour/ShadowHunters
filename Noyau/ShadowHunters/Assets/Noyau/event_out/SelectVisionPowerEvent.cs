using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Event qui permet au joueur de choisir à qui donner la carte vision
    // Permet au joueur ciblé d'utiliser son pouvoir si celui-ci est le métamorphe
    // Retourne l'event VisionCardEffectEvent

    public class SelectVisionPowerEvent : PlayerEvent
    {
        public int[] PossiblePlayerTargetId { get; set; }
        public VisionCard VisionCard { get; set; }
    }
}
