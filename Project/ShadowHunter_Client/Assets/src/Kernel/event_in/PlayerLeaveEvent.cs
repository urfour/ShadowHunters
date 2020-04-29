using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.src.Kernel.event_in
{
    class PlayerLeaveEvent : PlayerEvent
    {

        public PlayerLeaveEvent()
        {

        }

        public PlayerLeaveEvent(int playerId)
        {
            this.PlayerId = playerId;
        }
    }
}
