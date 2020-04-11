using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    // Retour de l'evenement SelectBobPowerEvent
    // Renvoie l'id du joueur et s'il a utiliser son pouvoir (s'il vole au lieu d'attaquer) ou non
    public class BobPowerEvent : PlayerEvent
    {
        public bool UsePower { get; set; }
    }
}
