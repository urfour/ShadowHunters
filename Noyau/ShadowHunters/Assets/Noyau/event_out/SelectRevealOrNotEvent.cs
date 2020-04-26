using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Noyau.Cards.controller;

namespace Scripts.event_out
{
    /// <summary>
    /// Event qui permet au joueur de choisir s'il veut se révéler ou pas 
    /// Attribut : id du joueur et carte qui fait effet
    /// Renvoie l'event RevealOrNotEvent
    /// </summary>
    public class SelectRevealOrNotEvent : PlayerEvent
    {
        public Card EffectCard { get; set; } = null;
        public bool PowerDaniel { get; set; } = false;
        public bool PowerLoup { get; set; } = false;
    }
}
