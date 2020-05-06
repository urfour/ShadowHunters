using Network.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network.controller
{
    class GClient
    {
        public static GClient Instance { get; private set; }
        private Mutex ConnectedClientsMutex = new Mutex();
        private Dictionary<string, Client> ConnectedClients { get; set; } = new Dictionary<string, Client>();


        public Client AddNewTCPClient(TcpClient client)
        {
            Client c = new Client(client);
            ConnectedClients.Add(client.Client.RemoteEndPoint.ToString(), c);
            Logger.Info("[GCLIENT] : ADD " + client.Client.RemoteEndPoint.ToString());
            return c;
        }

        public void RemoveTCPClient(TcpClient client)
        {
            ConnectedClientsMutex.WaitOne();
            try
            {
              ConnectedClients.Remove(client.Client.RemoteEndPoint.ToString());
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            ConnectedClientsMutex.ReleaseMutex();
        }

        public void Stop()
        {
            foreach(Client c in ConnectedClients.Values)
            {
                c.Stop();
            }
            ConnectedClients.Clear();
        }

        public GClient()
        {
            Instance = this;
        }
    }
}
