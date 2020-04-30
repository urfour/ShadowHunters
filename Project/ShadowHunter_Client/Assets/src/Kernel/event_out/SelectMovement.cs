using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Noyau.Manager.view;

namespace Scripts.event_out
{
    /// <summary>
    /// Event qui va donner le(s) lieu(x) et les résultats des lancers
    /// </summary>
    public class SelectMovement : PlayerEvent
    {
        public int[] LocationAvailable { get; set; }
    }
}
