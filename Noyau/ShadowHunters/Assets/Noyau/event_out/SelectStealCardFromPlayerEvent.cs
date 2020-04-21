using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Evenement qui permet de choisir une carte à volé parmis les cartes d'un joueur
    public class SelectStealCardFromPlayerEvent : PlayerEvent
    {
        public int PlayerStealedId { get; set; } 
    }
}
