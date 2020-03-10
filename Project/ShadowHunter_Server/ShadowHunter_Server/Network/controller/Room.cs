using EventSystem;
using Network.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.controller
{
    public class Room
    {
        private List<Client> Clients { get; set; } = new List<Client>();

        public void BroadCast(Client sender, string data)
        {
            foreach(Client c in Clients)
            {
                if (c != sender)
                    c.Send(data);
            }
        }


        public void BroadCast(Client sender, Event e)
        {
            BroadCast(sender, e.Serialize());
        }

        public void ClientJoin(Client c)
        {
            this.Clients.Add(c);
        }

        public void ClientLeave(Client c)
        {
            this.Clients.Remove(c);
        }
    }
}
