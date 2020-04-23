using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Evenement qui permet de choisir un joueur à attaquer parmis une liste de joueur
    public class SelectAttackTargetEvent : PlayerEvent
    {
        public int[] PossibleTargetId { get; set; } = null;
        // Dans certains cas on connait directement le joueur qu'on attaque
        public int TargetID { get; set; } = -1;
        public bool PowerFranklin { get; set; } = false;
        public bool PowerGeorges { get; set; } = false;
        public bool PowerLoup { get; set; } = false;
        public bool PowerCharles { get; set; } = false;
    }
}
