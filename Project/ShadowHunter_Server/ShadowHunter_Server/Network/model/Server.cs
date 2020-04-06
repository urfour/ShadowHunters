using Kernel.Settings;
using Network.controller;
using ShadowHunter_Server.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network.model
{
    class Server
    {

        private TcpListener Listener { get; set; }
        private Thread AcceptThread { get; set; }
        private GClient GClient { get; set; }
        private Room Global { get; set; } = new Room();

        public Server()
        {
            GClient = new GClient();

            Listener = new TcpListener(IPAddress.Parse("127.0.0.1"), SettingManager.Settings.Port.Value);


            Listener.Start();

            AcceptThread = new Thread(new ThreadStart(this.AcceptClients));
            AcceptThread.Start();

        }

        public void Stop()
        {
            GClient.Stop();
            Listener.Stop();
            //AcceptThread.Abort();
        }

        public void AcceptClients()
        {
            try
            {
                while (true)
                {
                    Logger.Info("Waiting for a connection...");
                    TcpClient client = Listener.AcceptTcpClient();
                    Client c = GClient.AddNewTCPClient(client);
                    c.JoinRoom(Global);
                }
            }
            catch (SocketException e)
            {
                Logger.Error(e);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            finally
            {
                Listener.Stop();
            }
        }
    }
}
