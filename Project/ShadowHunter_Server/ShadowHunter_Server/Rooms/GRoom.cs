using EventSystem;
using Network.controller;
using ServerInterface.AuthEvents;
using ServerInterface.RoomEvents;
using ServerInterface.RoomEvents.event_in;
using ServerInterface.RoomEvents.event_out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShadowHunter_Server.Accounts;

namespace ShadowHunter_Server.Rooms
{
     class GRoom : IListener<RoomEvent>
    {
        public static GRoom Instance { get; private set; } = null;

        public Dictionary<int, Room> Rooms { get; private set; } 
            = new Dictionary<int, Room>();
        public Random rand = new Random();
        GAccount GAccount { get; set; }

        public void OnEvent(RoomEvent e, string[] tags = null)
        {

            // créé et configure une nouvelle salle, puis y ajoute le joueur
            if (e is CreateRoomEvent cre)
            {
                Room newRoom = new Room(cre.RoomData);
                int code = rand.Next(0, 100000);
                int whilesafe = 100000;
                while (Rooms.ContainsKey(code) && whilesafe > 0)
                {
                    code = rand.Next(0, 100000);
                    whilesafe--;
                }

                newRoom.Data.Code = code;
                Rooms.Add(code, newRoom);
                cre.GetSender().JoinRoom(newRoom);
                newRoom.Data.CurrentNbPlayer = 1;
                newRoom.Data.IsLaunched = false;
                newRoom.Data.IsSuppressed = false;
                newRoom.Data.Players = new string[newRoom.Data.MaxNbPlayer];
                newRoom.Data.Players[0] = 
                    GAccount.Accounts[cre.GetSender()].Login;
                newRoom.Data.Host= GAccount.Accounts[cre.GetSender()].Login;
                // TODO : prévenir le joueur qu'il a été ajouté à la salle,
                // et broadcast de RoomDataEvent aux joueurs qui ne sont dans
                // aucune salle

                /* public int Code { get; set; }
            public int CurrentNbPlayer { get; set; } = 0;
            public bool IsSuppressed { get; set; } = false;
            public bool IsLaunched { get; set; } = false; */
            }


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
            /*if (e is RoomDataEvent rde)
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
            }*/
        }
        public static void Init()
        {
            new GRoom();
            Console.Write("GRoom OK");
            EventView.Manager.AddListener(Instance, true);
            GAccount.Init();
        }


        private GRoom()
        {
            if (Instance != null) Logger.Warning("GRoom replaced");
            Instance = this;
        }
    }
}
