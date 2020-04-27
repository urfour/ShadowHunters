using Assets.Noyau.Cards.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    /// <summary>
    /// Event qui permet de choisir sur qui on souhaite utiliser une carte lumière
    /// Attribut : id du joueur et carte qui fait effet
    /// Renvoie l'event LightCardEffectEvent
    /// </summary>
    public class SelectLightCardTargetEvent : PlayerEvent
    {
        public int[] PossibleTargetId { get; set; }
        public Card LightCard { get; set; }
    }
}
