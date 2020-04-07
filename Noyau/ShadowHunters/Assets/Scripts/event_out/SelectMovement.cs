using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    class SelectMovement : PlayerEvent
    {
        public int[] LocationAvailable;
        public int D6Dice { get; set; }
        public int D4Dice { get; set; }
    }
}
