using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Retour de l'evenement SelectAttackTargetEvent
    // Renvoie l'id du joueur attaquant, l'id du joueur attaqué et 2 booléans pour savoir si l'event vient du pouvoir d'un personnage
    public class AttackPlayerEvent : PlayerEvent
    {
        public int PlayerAttackedId { get; set; }
        public bool PowerFranklin { get; set; }
        public bool PowerGeorges { get; set; }
    }
}
