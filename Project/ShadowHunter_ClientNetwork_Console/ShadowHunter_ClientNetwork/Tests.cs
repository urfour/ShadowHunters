using System;
using EventSystem;
using Network.events;

namespace ShadowHunter_ClientNetwork
{
    public class Tests
    {
        public void LaunchTests()
        {

            string msg = "";
            while ((msg = Console.ReadLine()) != "exit")
            {
                //client.Send(msg);
                EventView.Manager.Emit(new ChatMSGEvent() { MSG = msg });
            }
        }

        public Tests()
        {
        }
    }
}
