using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Noyau.Cards.controller;

namespace Scripts.event_out
{
    /// <summary>
    /// Retour de l'evenement SelectRevealOrNotEvent
    /// Attribut : id du joueur, carte qui fait effet et booléen qui dit s'il s'est révélé
    /// </summary>
    public class RevealOrNotEvent : PlayerEvent
    {
        public Card EffectCard { get; set; }
        public bool HasRevealed { get; set; }
        public bool PowerLoup { get; set; }
    }
}
