using EventSystem;
using ServerInterface.AuthEvents;
using ServerInterface.RoomEvents;
using ServerInterface.RoomEvents.event_in;
using ServerInterface.RoomEvents.event_out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.MainMenuUI.SearchGame
{
    class GRoom : IListener<RoomEvent>
    {
        public static GRoom Instance { get; private set; } = null;

        public Dictionary<int, Room> Rooms { get; private set; } = new Dictionary<int, Room>();
        public Room JoinedRoom { get; private set; } = new Room();

        public void OnEvent(RoomEvent e, string[] tags = null)
        {
            /*
            if (e is WaitingRoomListEvent)
            {
                foreach (RoomData r in ((WaitingRoomListEvent)e).Rooms)
                {
                    if (r.IsSuppressed && Rooms.ContainsKey(r.Code))
                    {
                        // todo
                    }
                    else if (Rooms.ContainsKey(r.Code))
                    {
                        Rooms[r.Code].ModifData(r);
                    }
                    else
                    {
                        Rooms.Add(r.Code, new Room(r));
                    }
                }
            }
            */
            if (e is RoomDataEvent rde)
            {
                if (rde.RoomData.IsSuppressed)
                {
                    // todo
                }
                if (JoinedRoom.Code.Value == rde.RoomData.Code)
                {
                    JoinedRoom.ModifData(rde.RoomData);
                }
                if (Rooms.ContainsKey(rde.RoomData.Code))
                {
                    Rooms[rde.RoomData.Code].ModifData(rde.RoomData);
                }
                else
                {
                    Rooms.Add(rde.RoomData.Code, new Room(rde.RoomData));
                }
            }
            else if (e is RoomCreatedEvent rce)
            {
                // TODO gestion serveur
                RoomData r = rce.RoomData;
                JoinedRoom.ModifData(r);
                Rooms.Add(r.Code, JoinedRoom);
            }
        }

        public static void Init()
        {
            new GRoom();
            EventView.Manager.AddListener(Instance, true);
        }
        
        private GRoom()
        {
            if (Instance != null) Logger.Warning("GRoom replaced");
            Instance = this;
        }
    }
}
