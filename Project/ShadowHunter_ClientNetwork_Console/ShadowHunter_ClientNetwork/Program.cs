using EventSystem;
using Kernel.Settings;
using Network.events;
using Network.listener;
using Network.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowHunter_ClientNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            SettingManager.Load();
            EventView.Load();
            EventView.Manager.AddListener(new ChatListener());
            Client client = new Client();

            string msg = "";
            while ((msg = Console.ReadLine()) != "exit")
            {
                //client.Send(msg);
                EventView.Manager.Emit(new ChatMSGEvent() { MSG=msg });
            }

            client.Stop();

            Logger.Info("End");
            SettingManager.Save();
        }
    }
}
