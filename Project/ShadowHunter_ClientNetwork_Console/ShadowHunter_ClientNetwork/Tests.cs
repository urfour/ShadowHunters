using System;
using EventSystem;
using Network.events;
using ServerInterface.AuthEvents.event_out;
using ServerInterface.AuthEvents.event_in;
using ServerInterface.RoomEvents.event_out;
using ServerInterface.RoomEvents.event_in;
using ServerInterface.RoomEvents;

namespace ShadowHunter_ClientNetwork
{
    public class Tests
    {


        public void LaunchTests()
        {
            string msg = "";
            while ((msg = Console.ReadLine()) != "exit")
            {
                if(String.Compare(msg, "signin")==0)
                {
                    EventView.Manager.Emit(new SignInEvent() { Login = "testLogin", Password = "testPassword" });
                }

                else if (String.Compare(msg, "createRoom") == 0)
                {
                    //EventView.Manager.Emit(new CreateRoomEvent() { RoomData = new RoomData(0, "testRoom", 8, 0, false, true) });
                    EventView.Manager.Emit(new CreateRoomEvent());
                }

                else if (String.Compare(msg, "joinRoom") == 0)
                {
                    //EventView.Manager.Emit(new CreateRoomEvent() { RoomData = new RoomData(0, "testRoom", 8, 0, false, true) });
                    EventView.Manager.Emit(new JoinRoomEvent());
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
