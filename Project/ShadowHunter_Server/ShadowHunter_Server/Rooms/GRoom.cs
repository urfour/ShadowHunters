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
                cre.GetSender().Send(new RoomJoinedEvent() { RoomData = newRoom.Data });
                // broadcast de RoomDataEvent
                EventView.Manager.Emit(new RoomDataEvent() { RoomData = newRoom.Data });

            }

            if (e is JoinRoomEvent jre)
            {

                // on ne peut rejoindre une salle que s'il reste de la place
                if (jre.RoomData.CurrentNbPlayer < jre.RoomData.MaxNbPlayer)
                {
                    // vérification du mot de passe (s'il y en a un)
                    if ((Rooms[jre.code].Data.HasPassword == false) ||
                            ((Rooms[jre.code].Data.HasPassword == true) && 
                                (String.Compare(jre.password, Rooms[jre.code].Data.Password) == 0)))
                    {
                        // on ajoute le joueur à la salle et on actualise les infos de la salle
                        jre.GetSender().JoinRoom(Rooms[jre.code]);
                        Rooms[jre.code].Data.CurrentNbPlayer++;
                        Rooms[jre.code].Data.Players[Rooms[jre.code].Data.CurrentNbPlayer] =
                            GAccount.Accounts[jre.GetSender()].Login;
                        // prévenir le joueur qu'il a été ajouté à la salle
                        jre.GetSender().Send(new RoomJoinedEvent() 
                            { RoomData = Rooms[jre.code].Data });
                        // broadcast des nouvelles infos de la salle
                        EventView.Manager.Emit(new RoomDataEvent() 
                            { RoomData = Rooms[jre.code].Data });
                    }

                    // si le mot de passe est faux
                    else
                    {
                        jre.GetSender().Send(new RoomFailureEvent()
                        { Msg = "room.wrong_password" });
                    }

                }

                // si la salle est pleine
                else
                {
                    jre.GetSender().Send(new RoomFailureEvent()
                        { Msg = "room.full" });
                }

            }

            if (e is KickRoomEvent kre)
            {
                if (Rooms[kre.RoomData.Code].Data.Players.Contains(kre.Kicked.Login))
                {
                    // un joueur ne peut pas s'expulser lui-même
                    if (GAccount.Accounts[kre.GetSender()] == kre.Kicked)
                    {
                        kre.GetSender().Send(new RoomFailureEvent()
                            { Msg = "room.cant_kick_yourself" });
                    }

                    else
                    {
                        // on cherche grâce à son compte le client à expulser,
                        // puis on l'expulse
                        // (la requête LINQ est le seul moyen pour faire une
                        // recherche inversée dans un dictionnaire)
                        GAccount.Accounts.First(x => x.Value == kre.Kicked).Key.LeaveRoom();
                        Rooms[kre.RoomData.Code].Data.CurrentNbPlayer--;
                    }
                }

                else
                {
                    kre.GetSender().Send(new RoomFailureEvent()
                        { Msg = "room.kicked_player_not_in_room" });
                }
            }

            if (e is LeaveRoomEvent)
            {

            }

            if (e is ModifyRoomEvent)
            {

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
