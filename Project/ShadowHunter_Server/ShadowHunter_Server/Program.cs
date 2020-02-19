using EventSystem;
using Kernel.Settings;
using Network.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowHunter_Client
{
    class Program
    {
        public static Server Server { get; private set; }

        static void Main(string[] args)
        {
            EventView.Load();
            SettingManager.Load();
            Server = new Server();

            Console.ReadLine();
            Server.Stop();
            Console.ReadLine();

            SettingManager.Save();
        }
    }
}
