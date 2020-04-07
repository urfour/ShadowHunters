﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Evenement qui permet de choisir sur qui on souhaite utiliser une carte lumière
    // Attribut : id du joueur et carte qui fait effet
    // Renvoie l'event LightCardEffectEvent
    public class SelectLightCardTargetEvent : PlayerEvent
    {
        public int[] PossibleTargetId { get; set; }
        public LightCard LightCard { get; set; }
    }
}
