using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventExemple.Kernel.Players.event_out
{
    public class EnablePowerEvent : PlayerEvent
    {
        public bool enabled { get; set; }
    }
}
