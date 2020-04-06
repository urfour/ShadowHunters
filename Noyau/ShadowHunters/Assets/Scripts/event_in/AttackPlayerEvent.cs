using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventExemple.Kernel.Players.event_out
{
    // Evenement qui renvoie un joueur parmi une liste de joueur
    public class AttackPlayerEvent : PlayerEvent
    {
        public int playerAttackingId { get; set; }
        public int playerAttackedId { get; set; }
    }
}
