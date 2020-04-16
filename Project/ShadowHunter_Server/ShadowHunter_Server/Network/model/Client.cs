using EventSystem;
using Kernel.Settings;
using Network.controller;
using Network.events;
using ServerInterface.AuthEvents;
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
    public class Client : ListenableObject
    {
        private TcpClient TcpClient { get; set; }
        private Thread ListenThread { get; set; }
        private NetworkStream Stream { get; set; }

        public ListenableObject OnDisconnect { get; private set; } = new ListenableObject();
        public static List<OnNotification> OnConnect { get; private set; } = new List<OnNotification>();

        public Room Room { get; private set; } = null;
        public Account Account { get; set; } = null;

        private Mutex room_mutex = new Mutex();
        private Mutex send_Mutex = new Mutex();

        public void JoinRoom(Room new_room)
        {
            room_mutex.WaitOne();
            if (Room != null)
            {
                Room.RemoveClient(this);
            }
            new_room.AddClient(this);
            this.Room = new_room;
            room_mutex.ReleaseMutex();
        }

        public void LeaveRoom()
        {
            room_mutex.WaitOne();
            if (this.Room != null)
            {
                Room.RemoveClient(this);
            }
            this.Room = null;
            room_mutex.ReleaseMutex();
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
            foreach (OnNotification o in OnConnect)
            {
                o(this);
            }
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
            byte[] buffer = SettingManager.Encoder.Value.GetBytes(data + '\0');

            send_Mutex.WaitOne();
            NetworkStream stream = TcpClient.GetStream();
            stream.Write(buffer, 0, buffer.Length);
            send_Mutex.ReleaseMutex();
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
                string data = "";
                while ((i = Stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    data += SettingManager.Encoder.Value.GetString(buffer, 0, i);
                    string[] msgs = data.Split('\0');
                    for (int j = 0; j < msgs.Length - 1; j++)
                    {
                        if (msgs[j].Length > 0)
                        {
                            Logger.Comment("[CLIENT " + TcpClient.Client.RemoteEndPoint + " ] : " + msgs[j]);
                            Event e = Event.Deserialize(msgs[j], true);
                            Logger.Comment("" + e);
                            if (e != null)
                            {
                                ((ServerOnlyEvent)e).SetSender(this);
                                EventView.Manager.Emit(e, "network.emitted");
                            }
                            else if (Room != null)
                            {
                                Room.BroadCast(this, msgs[j]);
                            }
                        }
                    }
                    data = msgs[msgs.Length - 1];
                }
            }

            catch (IOException e)
            {
                Logger.Error(e);
            }
            finally
            {
                if (Account != null)
                {
                    Logger.Info("[CLIENT " + Account.Login + "] : Disconnected");
                }
                else
                {
                    Logger.Info("[CLIENT not logged] : Disconnected");
                }
                //LeaveRoom();
                GClient.Instance.RemoveTCPClient(this.TcpClient);
                TcpClient.Close();
                OnDisconnect.Notify();
            }
        }
    }
}
