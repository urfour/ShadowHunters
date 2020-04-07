using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // event pour le cas où le joueur à la boussole et choisi entre 2 lancers
    public class SelectDiceThrow : PlayerEvent
    {
        public int D6Dice1 { get; set; }
        public int D4Dice1 { get; set; }
        public int D6Dice2 { get; set; }
        public int D4Dice2 { get; set; }
    }
}
