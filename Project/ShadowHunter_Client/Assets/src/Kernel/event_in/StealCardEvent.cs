using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    /// <summary>
    /// Retour des evenements SelectStealCardEvent et SelectStealCardFromPlayerEvent
    /// Renvoie l'id du joueur qui vole, l'id du joueur volé et la carte volée
    /// </summary>
    public class StealCardEvent : PlayerEvent
    {
        public int PlayerStealedId { get; set; }
        public int CardId { get; set; }
    }
}
