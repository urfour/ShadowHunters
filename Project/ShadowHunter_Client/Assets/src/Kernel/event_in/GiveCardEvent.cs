using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    /// <summary>
    /// Retour de l'evenement SelectGiveCardEvent
    /// Renvoie l'id du joueur qui donne, l'id du joueur qui recoie et la carte donnée
    /// </summary>
    public class GiveCardEvent : PlayerEvent
    {
        public int PlayerGivedId { get; set; }
        public int CardId { get; set; }
    }
}
