using Assets.Noyau.Cards.controller;
using Assets.Noyau.Cards.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    /// <summary>
    /// Retour de l'evenement SelectVisionPowerEvent
    /// Renvoie le joueur qui a donné la carte, celui qui a reçu la carte,
    /// si ce dernier est métamorphe et la carte vision
    /// </summary>
    public class VisionCardEffectEvent : PlayerEvent
    {
        public int TargetId { get; set; }
        public Card VisionCard { get; set; }
        //public bool MetamorphePower { get; set; }
    }
}
