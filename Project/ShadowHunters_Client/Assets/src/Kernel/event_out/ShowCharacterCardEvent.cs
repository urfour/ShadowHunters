using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Noyau.Cards.model;

namespace Scripts.event_out
{
    public class ShowCharacterCardEvent : PlayerEvent
    {
        public string CardLabel { get; set; }

        public ShowCharacterCardEvent(string cardLabel, int playerId) : base(playerId)
        {
            this.CardLabel = cardLabel;
        }

        public ShowCharacterCardEvent() : base()
        {

        }
    }

}