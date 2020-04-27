using EventSystem;
using Kernel.Settings;
using Network.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network.model
{
    public partial class Client : IListener<Event>
    {
        private TcpClient TcpClient { get; set; }
        private Thread ListenThread { get; set; }
        private NetworkStream Stream { get; set; }

        public Client()
        {
            TcpClient = new TcpClient(SettingManager.Settings.ServerIP.Value, SettingManager.Settings.ServerPort.Value);
            
            ListenThread = new Thread(new ThreadStart(Listen));
            ListenThread.Start();
            EventView.Manager.AddListener(this);
        }

        private void Send(string data)
        {
            byte[] buffer = SettingManager.Encoder.Value.GetBytes(data + '\0');

            NetworkStream stream = TcpClient.GetStream();
            stream.Write(buffer, 0, buffer.Length);
            Logger.Info("[CLIENT] : Send : " + data);
        }

        public void Send(Event e)
        {
            Send(e.Serialize());
        }

        public void Stop()
        {
            Stream.Close();
            TcpClient.Close();
            ListenThread.Join();
            Logger.Info("Client stopped");
        }

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
                    for (int j = 0; j < msgs.Length-1; j++)
                    {
                        Logger.Comment("[CLIENT " + TcpClient.Client.RemoteEndPoint + " ] : " + data);
                        Event e = Event.Deserialize(data, true);
                        Logger.Comment("" + e);
                        if (e != null) EventView.Manager.Emit(e, "network.emitted");
                    }
                    data = msgs[msgs.Length-1];
                }

                Logger.Info("[CLIENT " + TcpClient.Client.RemoteEndPoint + "] : Disconnected");
                TcpClient.Close();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                EventView.Manager.Emit(new ServerOnlyEvent() { Msg = "message.network.error.global&" + e.GetType().FullName + "&" + e.Message.Replace(';', ',').Replace('&', '+') + "&" + e.StackTrace.Replace(';', ',').Replace('&', '+') });
            }
            finally
            {
                TcpClient.Close();
            }
        }

        public void OnEvent(Event e, string[] tags = null)
        {
            if (tags != null && tags.Length > 0 && tags[0].Equals("network.emitted")) return;

            Send(e.Serialize());
        }
    }
}
