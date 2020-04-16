using EventSystem;
using Network.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerInterface.RoomEvents;
using System.Threading;

namespace Network.controller
{
    public class Room
    {
        private List<Client> Clients { get; set; } = new List<Client>();
        private Mutex clients_mutex = new Mutex();

        public RoomData Data { get; set; }
        public Mutex RoomData_Mutex { get; private set; } = new Mutex();


        public Room()
        {

        }

        public Room(RoomData data)
        {
            ModifData(data);
        }

        public void ModifData(RoomData data)
        {
            this.Data = data;
        }

        public void BroadCast(Client sender, string data)
        {
            clients_mutex.WaitOne();
            foreach(Client c in Clients)
            {
                if (c != sender)
                    c.Send(data);
            }
            clients_mutex.ReleaseMutex();
        }

        public void AddClient(Client c)
        {
            clients_mutex.WaitOne();
            Clients.Add(c);
            clients_mutex.ReleaseMutex();
        }

        public void RemoveClient(Client c)
        {
            clients_mutex.WaitOne();
            Clients.Remove(c);
            clients_mutex.ReleaseMutex();
        }

        public void BroadCast(Client sender, Event e)
        {
            BroadCast(sender, e.Serialize());
        }
    }
}
