using ServerInterface.AuthEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.RoomEvents.event_out
{
    class KickRoomEvent : RoomEvent
    {
        public Account Kicked { get; set; }

        public KickRoomEvent(Account kicked, RoomData room) : base(room)
        {
            this.Kicked = kicked;
        }

        public KickRoomEvent()
        {

        }
    }
}
