using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Retour de l'evenement SelectGiveCardEvent
    // Renvoie l'id du joueur qui donne, l'id du joueur qui recoie et la carte donnée
    public class GiveCardEvent : PlayerEvent
    {
        public int PlayerGivedId { get; set; }
        public string CardGivedName { get; set; }
    }
}
