using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventExemple.Kernel.Players.event_in
{
    // Retour de l'evenement SelectStealCardEvent
    // Renvoie l'id du joueur qui vole, l'id du joueur vol� et la carte vol�e
    public class StealCardEvent : PlayerEvent
    {
        public int playerStealedId { get; set; }
        public string cardStealedName { get; set; }
    }
}
