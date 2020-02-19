using EventSystem;
using Kernel.Settings;
using Network.controller;
using Network.events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network.model
{
    public class Client
    {
        private TcpClient TcpClient { get; set; }
        private Thread ListenThread { get; set; }
        private NetworkStream Stream { get; set; }

        public Room Room { get; private set; }

        public void JoinRoom(Room new_room)
        {
            if (Room != null)
            {
                Room.ClientLeave(this);
            }
            new_room.ClientJoin(this);
            this.Room = new_room;
        }

        public void LeaveRoom()
        {
            if (this.Room != null)
            {
                Room.ClientLeave(this);
            }
        }


        /// <summary>
        /// Récupère la connection TCP d'un Server.Accept() et s'occupe de reçevoir/émettre les différents événements
        /// </summary>
        /// <param name="tcpClient"></param>
        public Client(TcpClient tcpClient)
        {
            this.TcpClient = tcpClient;
            ListenThread = new Thread(new ThreadStart(Listen));
            ListenThread.Start();
        }

        /// <summary>
        /// Ferme la connection avec le client
        /// </summary>
        public void Stop()
        {
            Stream.Close();
            TcpClient.Close();
            ListenThread.Join();
            Logger.Info("Client stopped");
        }

        /// <summary>
        /// Envoie un string vers le client
        /// </summary>
        /// <param name="data"></param>
        public void Send(string data)
        {
            byte[] buffer = SettingManager.Encoder.Value.GetBytes(data);

            NetworkStream stream = TcpClient.GetStream();
            stream.Write(buffer, 0, buffer.Length);
            Logger.Info("[CLIENT] : Send : " + data);
        }

        public void Send(Event e)
        {
            Send(e.Serialize());
        }

        /// <summary>
        /// écoute les messages du client, tente de sérialiser en Event. Si la sérialisation fonctionne : envoie l'event dans l'IEventManager; sinon : transmet le message dans la room du client
        /// </summary>
        private void Listen()
        {
            try
            {
                byte[] buffer = new byte[SettingManager.Settings.BufferSize.Value];
                int i;
                Stream = TcpClient.GetStream();

                Logger.Info("[CLIENT " + TcpClient.Client.RemoteEndPoint + "] : Connected");
                string data = null;
                while ((i = Stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    data = SettingManager.Encoder.Value.GetString(buffer, 0, i);
                    Logger.Comment("[CLIENT " + TcpClient.Client.RemoteEndPoint + " ] : " + data);
                    Event e = Event.Deserialize(data, true);
                    Logger.Comment("" + e);
                    if (e != null)
                    {
                        ((ServerOnlyEvent)e).SetSender(this);
                        EventView.Manager.Emit(e, "network.emitted");
                    } 
                    else if (Room != null)
                    {
                        Room.BroadCast(this, data);
                    }
                }
            }
            catch (IOException e)
            {
                //Logger.Error(e);
            }
            finally
            {
                Logger.Info("[CLIENT " + TcpClient.Client.RemoteEndPoint + "] : Disconnected");
                LeaveRoom();
                GClient.Instance.RemoveTCPClient(this.TcpClient);
                TcpClient.Close();
            }
        }
    }
}
