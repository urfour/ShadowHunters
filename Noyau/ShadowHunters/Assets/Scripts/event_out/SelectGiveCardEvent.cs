using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Evenement qui permet de choisir un joueur à qui donner une carte parmis une liste de joueur
    public class SelectGiveCardEvent : PlayerEvent
    {
        public int[] PossibleTargetId { get; set; } 
    }
}
