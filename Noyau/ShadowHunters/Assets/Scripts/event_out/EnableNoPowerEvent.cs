using EventExemple.Kernel.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.event_out
{
    public class EnableNoPowerEvent : PlayerEvent
    {
        public bool enabled { get; set; }
    }
}
