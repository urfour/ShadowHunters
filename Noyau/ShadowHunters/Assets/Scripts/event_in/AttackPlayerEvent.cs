using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventExemple.Kernel.Players.event_out
{
    // Retour de l'evenement SelectAttackTargetEvent
    // Renvoie l'id du joueur attaquant et l'id du joueur attaqué
    public class AttackPlayerEvent : PlayerEvent
    {
        public int PlayerAttackedId { get; set; }
    }
}
