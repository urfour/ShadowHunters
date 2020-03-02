using Assets.Scripts.MainMenuUI.SearchGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.RoomEvents.event_in
{
    class WaitingRoomListEvent : RoomEvent
    {
        public RoomData[] Rooms { get; set; }

        public WaitingRoomListEvent()
        {

        }

        public WaitingRoomListEvent(RoomData[] rooms)
        {
            this.Rooms = rooms;
        }
    }
}
