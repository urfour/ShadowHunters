using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Evenement appelé lorsque le joueur pioche une carte
    // Envoie l'id du joueur et le type de carte piochée
    public class DrawCardEvent : PlayerEvent
    {
        public CardType SelectedCardType { get; set; }
    }
}
