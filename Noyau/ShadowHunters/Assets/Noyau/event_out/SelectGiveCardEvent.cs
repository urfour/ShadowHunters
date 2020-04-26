using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    /// <summary>
    /// Event qui permet de choisir un joueur à qui donner une carte parmi une liste de joueurs
    /// </summary>
    public class SelectGiveCardEvent : PlayerEvent
    {
        public int[] PossibleTargetId { get; set; } 
    }
}
