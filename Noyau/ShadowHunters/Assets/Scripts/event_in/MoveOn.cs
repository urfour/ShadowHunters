using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    // déplace le joueur sur le lieu
    public class MoveOn : PlayerEvent
    {
        public int Location { get; set; }
    }
}
