using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.src.Kernel.event_in
{
    class PowerUseEvent : PlayerEvent
    {

        public PowerUseEvent(int playerId) : base(playerId)
        {

        }

        public PowerUseEvent() : base()
        {

        }
    }
}
