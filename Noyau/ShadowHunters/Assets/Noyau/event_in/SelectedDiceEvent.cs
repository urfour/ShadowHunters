using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    // Event retour pour SelectDiceThrow
    public class SelectedDiceEvent : PlayerEvent
    {
        public int D6Dice { get; set; }
        public int D4Dice { get; set; }
    }
}