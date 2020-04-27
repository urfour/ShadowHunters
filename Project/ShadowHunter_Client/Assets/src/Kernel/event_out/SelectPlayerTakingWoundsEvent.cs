using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    /// <summary>
    /// Event qui permet de choisir un joueur à attaquer parmi une liste de joueurs
    /// Attributs: nb de blessures infligées, de soins sur soi, si la carte est la poupée démoniaque
    /// Renvoie l'event TakingWoundsEvent
    /// </summary>
    public class SelectPlayerTakingWoundsEvent : PlayerEvent
    {
        public int[] PossibleTargetId { get; set; }
        // Dans certains cas on connait directement le joueur qu'on attaque
        public int TargetID { get; set; } = -1;
        public bool IsPuppet { get; set; }
        public int NbWoundsTaken { get; set; }
        public int NbWoundsSelfHealed { get; set; }
    }
}
