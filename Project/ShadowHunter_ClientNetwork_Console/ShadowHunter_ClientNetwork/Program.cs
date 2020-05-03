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
            Tests tests = new Tests();

            tests.LaunchTests();

            client.Stop();

            Logger.Info("End");
            SettingManager.Save();
        }
    }
}
