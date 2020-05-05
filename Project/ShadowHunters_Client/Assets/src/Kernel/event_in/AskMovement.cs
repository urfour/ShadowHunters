using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.event_in
{
    /// <summary>
    /// Event de déplacement en début de tour.
    /// </summary>
    public class AskMovement : PlayerEvent
    {
        public AskMovement(int playerId) : base()
        {
            this.PlayerId = playerId;
        }

        public AskMovement() : base()
        {

        }
    }
}
