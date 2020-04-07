using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventExemple.Kernel.Players
{
    public class PlayerEvent : Event
    {
        public int PlayerId { get ; set; }
    }
}
