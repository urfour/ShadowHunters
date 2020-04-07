using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Retour de l'evenement SelectPlayerTakingWoundsEffectEvent
    // Renvoie l'id du joueur attaquant et l'id du joueur attaqué
    public class TakingWoundsEffectEvent : PlayerEvent
    {
        public int PlayerAttackedId { get; set; }
        public bool IsPuppet { get; set; }
        public int NbWoundsTaken { get; set; }
        public int NbWoundsSelfHealed { get; set; }
    }
}
