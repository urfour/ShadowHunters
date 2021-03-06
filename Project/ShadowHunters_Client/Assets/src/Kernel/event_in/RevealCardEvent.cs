﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    /// <summary>
    /// Event qui se déclenche que le joueur appuie sur le bouton se révéler
    /// </summary>
    public class RevealCardEvent : PlayerEvent
    {
        public RevealCardEvent(int playerId) : base()
        {
            this.PlayerId = playerId;
        }

        public RevealCardEvent() : base()
        {
        }
    }
}
