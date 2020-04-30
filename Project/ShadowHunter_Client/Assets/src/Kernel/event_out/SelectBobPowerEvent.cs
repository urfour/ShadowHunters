using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    /// <summary>
    /// Event qui donne le choix à Bob d'utiliser son pouvoir 
    /// (voler un équipement au lieu d'attaquer) ou non
    /// Retourne l'event BobPowerEvent
    /// </summary>
    public class SelectBobPowerEvent : PlayerEvent
    {
        public SelectBobPowerEvent(int playerId) : base()
        {
            this.PlayerId = playerId;
        }

        public SelectBobPowerEvent() : base()
        {

        }
    }
}
