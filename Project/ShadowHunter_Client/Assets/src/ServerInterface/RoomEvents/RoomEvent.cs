using Network.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.RoomEvents
{
    class RoomEvent : ServerOnlyEvent
    {
        public RoomData RoomData { get; set; }

        public RoomEvent(RoomData roomData, string msg = null) : base(msg)
        {
            this.RoomData = roomData;
        }

        public RoomEvent()
        {

        }
    }
}
