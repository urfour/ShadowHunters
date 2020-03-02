using EventSystem;
using ServerInterface.RoomEvents;
using ServerInterface.RoomEvents.event_in;
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

        public void OnEvent(RoomEvent e, string[] tags = null)
        {
            if (e is WaitingRoomListEvent)
            {
                foreach (RoomData r in ((WaitingRoomListEvent)e).Rooms)
                {
                    if (r.IsSuppressed && Rooms.ContainsKey(r.Code))
                    {

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
            else if (e is JoindedWaitingRoomDataEvent)
            {

            }
        }

        public GRoom()
        {
            if (Instance != null) Logger.Warning("GRoom replaced");
            Instance = this;
        }
    }
}
