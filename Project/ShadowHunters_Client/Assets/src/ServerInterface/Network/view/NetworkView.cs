using Network.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.Network
{
    public class NetworkView
    {
        private static Client client;
        public static void Connect()
        {
            client= new Client();
        }

        public static void Disconnect()
        {
            client.Stop();
        }
    }
}
