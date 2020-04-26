using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    /// <summary>
    /// Event qui déplace le joueur sur le lieu
    /// </summary>
    public class MoveOn : PlayerEvent
    {
        public int Location { get; set; }
    }
}
