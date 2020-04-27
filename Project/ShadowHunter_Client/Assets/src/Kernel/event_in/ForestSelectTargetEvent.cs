using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    /// <summary>
    /// Evenement qui permet de choisir un joueur parmis une liste de joueurs
    /// et soit de le soigner de 1 soit de le blesser de 2 (il peut se choisir lui-même dans les 2 cas)
    /// </summary>
    
    // WARNING !!! si un perso dans la liste a la broche de chance
    // WARNING !!! ne peut pas prendre de dégats mais peut toujours etre soigné
    // WARNING !!! ne peut s'implémenter que dans l'UI
    public class ForestSelectTargetEvent : PlayerEvent
    {
        public bool Hurt { get; set; }
    }
}
