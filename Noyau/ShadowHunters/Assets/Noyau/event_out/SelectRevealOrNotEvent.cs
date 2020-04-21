using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Evenement qui permet au joueur de choisir s'il veut se révéler ou pas 
    // Attribut : id du joueur et carte qui fait effet
    // Renvoie l'event RevealOrNotEvent
    public class SelectRevealOrNotEvent : PlayerEvent
    {
        public Card EffectCard { get; set; }
    }
}
