using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Evenement qui permet de choisir un joueur à attaquer parmis une liste de joueur 
    // Renvoie l'event TakingWoundsEvent
    public class SelectPlayerTakingWoundsEvent : PlayerEvent
    {
        public int[] PossibleTargetId { get; set; }
        public bool IsPuppet { get; set; }
        public int NbWoundsTaken { get; set; }
        public int NbWoundsSelfHealed { get; set; }
    }
}
