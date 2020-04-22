using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // event qui va donner le(s) lieu(x) et les résultats des lancers
    public class SelectMovement : PlayerEvent
    {
        public Position[] LocationAvailable;
    }
}
