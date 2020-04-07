using EventExemple.Kernel.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventExemple.Scripts.Players.event_out
{
    // Evenement qui permet de choisir un joueur à attaquer parmis une liste de joueur
    public class SelectAttackTargetEvent : PlayerEvent
    {
        public int[] PossibleTargetId { get; set; } 
    }
}
