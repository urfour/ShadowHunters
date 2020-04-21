using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Retour de l'evenement SelectRevealOrNotEvent
    // Attribut : id du joueur, carte qui fait effet et boolean qui dit s'il s'est révélé
    public class RevealOrNotEvent : PlayerEvent
    {
        public Card EffectCard { get; set; }
        public bool HasRevealed { get; set; }
    }
}
