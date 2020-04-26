using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    /// <summary>
    /// Event qui permet de choisir une carte à voler parmi les cartes d'un joueur
    /// </summary>
    public class SelectStealCardFromPlayerEvent : PlayerEvent
    {
        public int PlayerStealedId { get; set; } 
    }
}
