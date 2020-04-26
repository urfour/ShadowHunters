﻿using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    public class SelectUsableCardPickedEvent : PlayerEvent
    {
        public int CardId { get; set; }
        public bool IsHidden { get; set; }

        public SelectUsableCardPickedEvent(int cardId, bool isHidden)
        {
            CardId = cardId;
            IsHidden = isHidden;
        }

        public SelectUsableCardPickedEvent()
        {

        }
    }
}
