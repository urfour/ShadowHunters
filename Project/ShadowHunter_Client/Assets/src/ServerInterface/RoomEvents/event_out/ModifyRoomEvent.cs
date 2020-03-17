using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.RoomEvents
{
    class ModifyRoomEvent : RoomEvent
    {

        public ModifyRoomEvent()
        {

        }

        public ModifyRoomEvent(RoomData roomData) : base (roomData)
        {

        }
    }
}
