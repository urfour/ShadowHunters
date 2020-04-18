using System;
using EventSystem;
using Network.events;
using ServerInterface.AuthEvents;
using ServerInterface.RoomEvents;

namespace ShadowHunter_ClientNetwork
{
    public class Tests
    {
        int kode;
        string user = "testLogin";

        public void LaunchTests()
        {
            string msg = "";
            while ((msg = Console.ReadLine()) != "exit")
            {
                if(String.Compare(msg, "signin")==0)
                {
                    user = Console.ReadLine();
                    EventView.Manager.Emit(new SignInEvent() { Login = user, Password = "testPassword" });
                }

                else if (String.Compare(msg, "createRoom") == 0) // test OK
                {
                    EventView.Manager.Emit(new CreateRoomEvent() { RoomData = new RoomData(0,"testRoom", 8, 0, false, true) });
                    //EventView.Manager.Emit(new CreateRoomEvent());
                }

                else if (String.Compare(msg, "join") == 0) // OK
                {
                    kode = Int32.Parse(Console.ReadLine());
                    EventView.Manager.Emit(new JoinRoomEvent() { RoomData = new RoomData(kode, "testRoom", 8, 0, false, true) });
                }

                else if (String.Compare(msg, "ready") == 0) // OK
                {
                    kode = Int32.Parse(Console.ReadLine());
                    EventView.Manager.Emit(new ReadyEvent() { RoomData = new RoomData(kode, "testRoom", 8, 0, false, true) });
                }

                else if (String.Compare(msg, "leave") == 0) //ok
                {
                    EventView.Manager.Emit(new LeaveRoomEvent() { RoomData = new RoomData(kode, "testRoom", 8, 0, false, true) });
                }

                else if (String.Compare(msg, "start") == 0) //message.room.invalid.start.recquire_all_players_ready
                {
                    kode = Int32.Parse(Console.ReadLine());
                    EventView.Manager.Emit(new StartGameEvent() { RoomData = new RoomData(kode, "testRoom", 8, 0, false, true) });
                }

                else
                {
                    EventView.Manager.Emit(new ChatMSGEvent() { MSG = msg });
                }

            }
        }

        public Tests()
        {
        }
    }
}
