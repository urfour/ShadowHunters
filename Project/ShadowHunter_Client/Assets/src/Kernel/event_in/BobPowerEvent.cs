using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    /// <summary>
    /// Retour de l'evenement SelectBobPowerEvent
    /// Renvoie l'id du joueur et s'il a utilisé son pouvoir (s'il vole au lieu d'attaquer) ou non
    /// </summary>
    public class BobPowerEvent : PlayerEvent
    {
        public bool UsePower { get; set; }
    }
}
