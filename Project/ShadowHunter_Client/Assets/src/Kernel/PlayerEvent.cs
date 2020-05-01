using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts
{
    public class PlayerEvent : Event
    {
        public int PlayerId { get ; set; }

        public PlayerEvent() : base()
        {

        }

        public PlayerEvent(int PlayerId) : base();
        {
            this.PlayerId = PlayerId;
        }
    }
}
