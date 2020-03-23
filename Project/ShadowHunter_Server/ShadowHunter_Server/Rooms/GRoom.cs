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

        // on instancie ici le gestionnaire de comptes pour y avoir accès
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
                // prévenir le joueur qu'il a été ajouté à la salle
                cre.GetSender().Send(new JoinRoomEvent() { RoomData = newRoom.Data });
                // broadcast de RoomDataEvent
                EventView.Manager.Emit(new RoomDataEvent() { RoomData = newRoom.Data });


                /* public int Code { get; set; }
            public int CurrentNbPlayer { get; set; } = 0;
            public bool IsSuppressed { get; set; } = false;
            public bool IsLaunched { get; set; } = false; */
            }

            if (e is JoinRoomEvent jre)
            {
                // on ne peut rejoindre une salle que s'il reste de la place

            }
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
