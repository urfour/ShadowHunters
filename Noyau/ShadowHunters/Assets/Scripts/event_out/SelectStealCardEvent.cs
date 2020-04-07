using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventExemple.Kernel.Players.event_out
{
    // Evenement qui permet de choisir un joueur à volé parmis une liste de joueur
    public class SelectStealCardEvent : PlayerEvent
    {
        public int[] PossiblePlayerTargetId { get; set; } 
    }
}
