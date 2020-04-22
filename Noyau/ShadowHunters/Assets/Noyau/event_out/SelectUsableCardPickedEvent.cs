using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.event_out
{
    class SelectUsableCardPickedEvent
    {
        public int CardId { get; set; }

        public SelectUsableCardPickedEvent(int cardId)
        {
            CardId = cardId;
        }

        public SelectUsableCardPickedEvent()
        {

        }
    }
}
