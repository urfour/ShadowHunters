using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.src.Kernel.event_in
{
    public class PlayerLeaveEvent : PlayerEvent
    {

        public PlayerLeaveEvent() : base()
        {

        }

        public PlayerLeaveEvent(int playerId) : base()
        {
            this.PlayerId = playerId;
        }
    }
}
