using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    /// <summary>
    /// Retour de l'evenement SelectGiveOrWoundEvent
    /// Renvoie l'id du joueur et si veut donner ou se blesser 
    /// (si Give == false, alors il choisit blessure) 
    /// </summary>
    public class GiveOrWoundEvent : PlayerEvent
    {
        public bool Give { get; set; }
    }
}
