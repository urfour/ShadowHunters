using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.RoomEvents.event_out
{
    public class CreateRoomEvent : RoomEvent
    {
        //public RoomData RoomData { get; set; }

        public CreateRoomEvent()
        {

        }

        public CreateRoomEvent(RoomData roomData)
        {
            this.RoomData = roomData;
        }
    }
}

/*
 
    création d'une partie : 
    CLIENT HOST : emit CreateRoomEvent
    SERVER : LISTEN CreateRoomEvent 
        {
           emit to host RoomCreatedEvent
           broadcast\host RoomDataEvent
        }
     
     */