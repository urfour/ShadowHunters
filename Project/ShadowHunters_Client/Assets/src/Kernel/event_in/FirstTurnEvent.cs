using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.event_in
{
    /// <summary>
    /// Event de début de partie
    /// </summary>
    public class FirstTurnEvent : PlayerEvent
    {
        public FirstTurnEvent(int playerId) : base()
        {
            this.PlayerId = playerId;
        }

        public FirstTurnEvent() : base()
        {

        }
    }
}