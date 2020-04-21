using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    // Retour de l'evenement SelectGiveOrWoundEvent
    // Renvoie l'id du joueur et si veut donner ou se blesser (si give == false, alors il choisit blessure) 
    public class GiveOrWoundEvent : PlayerEvent
    {
        public bool Give { get; set; }
    }
}
