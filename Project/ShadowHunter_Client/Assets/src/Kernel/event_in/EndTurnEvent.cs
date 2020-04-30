using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    /// <summary>
    /// Event du bouton de fin de tour qui va permettre de choisir le prochain joueur
    /// </summary>
    public class EndTurnEvent : PlayerEvent
    {
        public EndTurnEvent(int playerId) : base()
        {
            this.PlayerId = playerId;
        }

        public EndTurnEvent() : base()
        {
        }
    }
}