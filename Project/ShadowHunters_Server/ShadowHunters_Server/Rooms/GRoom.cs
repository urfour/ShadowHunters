using EventSystem;
using Network.controller;
using ServerInterface.AuthEvents;
using ServerInterface.RoomEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShadowHunter_Server.Accounts;
using Network.model;
using System.Threading;

namespace ShadowHunter_Server.Rooms
{
    public class GRoom : IListener<RoomEvent>
    {
        public static GRoom Instance { get; private set; } = null;

        public Room Global = new Room();

        public Dictionary<int, Room> Rooms { get; private set; }
            = new Dictionary<int, Room>();
        private Mutex Rooms_Mutex = new Mutex();

        public Random rand = new Random();

        // on instancie ici le gestionnaire de comptes pour y avoir accès
        //GAccount GAccount { get; set; }

        public void OnEvent(RoomEvent e, string[] tags = null)
        {
            Console.WriteLine("Evènement de gestion de salle reçu.");

            if (e is CreateRoomEvent cre)
            {
                //Room room = Rooms[cre.RoomData.Code];
                RoomData r = cre.RoomData;
                int code = rand.Next(10000, 100000);
                int whilesafe = 100000;
                while (Rooms.ContainsKey(code) && whilesafe > 0)
                {
                    code = rand.Next(10000, 100000);
                    whilesafe--;
                }
                r.Code = code;
                r.CurrentNbPlayer = 1;
                r.Players = new string[r.MaxNbPlayer];
                r.ReadyPlayers = new bool[r.MaxNbPlayer];
                r.Players[0] = cre.GetSender().Account.Login;
                //r.Host = GAccount.Instance.LoggedAccount.Login;
                Room room = new Room(r);
                Rooms.Add(code, room);
                Global.BroadCast(null, new RoomDataEvent() { RoomData = r });
                e.GetSender().JoinRoom(room);
                e.GetSender().Send(new RoomJoinedEvent() { RoomData = r });
            }
            else if (e is JoinRoomEvent jre)
            {
                Rooms_Mutex.WaitOne();
                if (Rooms.ContainsKey(jre.RoomData.Code))
                {
                    Room r = Rooms[jre.RoomData.Code];
                    Rooms_Mutex.ReleaseMutex();
                    r.RoomData_Mutex.WaitOne();
                    if (e.GetSender().Room != null)
                    {
                        e.GetSender().Send(new RoomFailureEvent() { Msg = "message.room.invalid.join.please_leave_your_room_before" });
                    }
                    else
                    {
                        if (r.Data.CurrentNbPlayer < r.Data.MaxNbPlayer)
                        {
                            if (r.Data.Players.Contains(e.GetSender().Account.Login))
                            {
                                e.GetSender().Send(new RoomFailureEvent() { Msg = "message.room.invalid.join.you_are_already_in_this_room" });
                            }
                            else
                            {
                                r.Data.Players[r.Data.CurrentNbPlayer] = e.GetSender().Account.Login;
                                r.Data.CurrentNbPlayer++;
                                Global.BroadCast(null, new RoomDataEvent() { RoomData = r.Data });
                                e.GetSender().JoinRoom(r);
                                e.GetSender().Send(new RoomJoinedEvent() { RoomData = r.Data });
                            }
                        }
                        else
                        {
                            EventView.Manager.Emit(new RoomFailureEvent() { Msg = "message.room.invalid.join.room_is_full" });
                        }
                    }
                    r.RoomData_Mutex.ReleaseMutex();
                }
                else
                {
                    Rooms_Mutex.ReleaseMutex();
                    e.GetSender().Send(new RoomFailureEvent() { Msg = "message.room.invalid.room_code_dont_exist&" + jre.RoomData.Code });
                }
            }
            else if (e is LeaveRoomEvent lre)
            {
                RemovePlayerFromRoom(e.GetSender(), lre.RoomData);
            }
            else if (e is ReadyEvent re)
            {
                Rooms_Mutex.WaitOne();
                if (!Rooms.ContainsKey(re.RoomData.Code))
                {
                    Rooms_Mutex.ReleaseMutex();
                    e.GetSender().Send(new RoomFailureEvent() { Msg = "message.room.invalid.room_code_dont_exist&" + re.RoomData.Code });
                }
                else if (e.GetSender().Room == null)
                {
                    Rooms_Mutex.ReleaseMutex();
                    e.GetSender().Send(new RoomFailureEvent() { Msg = "message.room.invalid.you_are_not_in_a_room" });
                }
                else
                {
                    Room room = Rooms[re.RoomData.Code];
                    Rooms_Mutex.ReleaseMutex();
                    room.RoomData_Mutex.WaitOne();
                    RoomData r = room.Data;
                    string login = e.GetSender().Account.Login;
                    for (int i = 0; i < r.CurrentNbPlayer; i++)
                    {
                        if (r.Players[i].CompareTo(login) == 0)
                        {
                            r.ReadyPlayers[i] = !r.ReadyPlayers[i];
                            break;
                        }
                    }
                    Global.BroadCast(null, new RoomDataEvent() { RoomData = r });
                    room.RoomData_Mutex.ReleaseMutex();
                }
            }
            else if (e is StartGameEvent sge)
            {
                Rooms_Mutex.WaitOne();
                if (!Rooms.ContainsKey(sge.RoomData.Code))
                {
                    Rooms_Mutex.ReleaseMutex();
                    e.GetSender().Send(new RoomFailureEvent() { Msg = "message.room.invalid.room_code_dont_exist&" + sge.RoomData.Code });
                }
                else
                {
                    Room room = Rooms[sge.RoomData.Code];
                    Rooms_Mutex.ReleaseMutex();
                    room.RoomData_Mutex.WaitOne();
                    RoomData r = room.Data;
                    bool ready = true;
                    for (int i = 0; i < r.MaxNbPlayer; i++)
                    {
                        if (!r.ReadyPlayers[i])
                        {
                            ready = false;
                            break;
                        }
                    }
                    if (ready)
                    {
                        r.IsLaunched = true;
                        Global.BroadCast(null, new RoomDataEvent() { RoomData = r });
                    }
                    else
                    {
                        e.GetSender().Send(new RoomFailureEvent() { Msg = "message.room.invalid.start.recquire_all_players_ready" });
                    }
                    room.RoomData_Mutex.ReleaseMutex();
                }
            }


        }


        public static void Init()
        {
            new GRoom();
            Console.WriteLine("GRoom OK");
            EventView.Manager.AddListener(Instance);
        }


        public GRoom()
        {
            if (Instance != null) Logger.Warning("GRoom replaced");
            Instance = this;
        }

        public void AddClient(Client c)
        {
            Global.AddClient(c);
            c.OnDisconnect.AddListener((sender) =>
            {
                this.OnClientDisconnect(c);
            });
            foreach (Room r in Rooms.Values)
            {
                c.Send(new RoomDataEvent() { RoomData = r.Data });
            }
        }

        public void RemoveClient(Client c)
        {
            Global.RemoveClient(c);
        }

        public void OnClientDisconnect(Client c)
        {
            Global.RemoveClient(c);
            if (c.Room != null)
            {
                RemovePlayerFromRoom(c, c.Room.Data, false);
            }
            c.LeaveRoom();
        }


        public void RemovePlayerFromRoom(Client c, RoomData r, bool notifyClient = true, string leaveMessage = null)
        {
            if (r == null) return;
            Rooms_Mutex.WaitOne();
            if (!Rooms.ContainsKey(r.Code))
            {
                Rooms_Mutex.ReleaseMutex();
                if (notifyClient) c.Send(new RoomFailureEvent() { Msg = "message.room.invalid.room_code_dont_exist&" + r.Code });
            }
            else if (c.Room == null)
            {
                Rooms_Mutex.ReleaseMutex();
                if (notifyClient) c.Send(new RoomFailureEvent() { Msg = "message.room.invalid.you_are_not_in_a_room" });
            }
            else
            {
                r = c.Room.Data;
                Rooms_Mutex.ReleaseMutex();
                c.Room.RoomData_Mutex.WaitOne();

                bool found = false;
                r.CurrentNbPlayer--;
                if (r.CurrentNbPlayer == 0)
                {
                    r.IsSuppressed = true;
                    Rooms.Remove(r.Code);
                }
                else
                {
                    for (int i = 0; i < r.CurrentNbPlayer; i++)
                    {
                        if (found)
                        {
                            r.Players[i] = r.Players[i + 1];
                            r.ReadyPlayers[i] = r.ReadyPlayers[i + 1];
                        }
                        else
                        {
                            if (r.Players[i] == c.Account.Login)
                            {
                                found = true;
                                r.Players[i] = r.Players[i + 1];
                                r.ReadyPlayers[i] = r.ReadyPlayers[i + 1];
                            }
                        }
                    }
                    r.ReadyPlayers[r.CurrentNbPlayer] = false;
                    r.Players[r.CurrentNbPlayer] = null;
                }
                Global.BroadCast(null, new RoomDataEvent() { RoomData = r });
                Room room = c.Room;
                c.LeaveRoom();
                if (notifyClient) c.Send(new RoomLeavedEvent() { RoomData = r, Msg = leaveMessage });
                room.RoomData_Mutex.ReleaseMutex();
            }
        }
    }
}
