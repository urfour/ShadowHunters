using Assets.Noyau.Cards.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    // Retour de l'evenement SelectLightCardTargetEvent
    // Attribut : id du joueur et carte qui fait effet
    public class LightCardEffectEvent : PlayerEvent
    {
        public int PlayerChoosenId { get; set; }
        public Card LightCard { get; set; }
    }
}
