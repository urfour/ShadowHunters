using Assets.Noyau.Cards.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Cards.model
{
    public class UsableCard : Card
    {
        public readonly CardEffect[] cardEffect;
        public readonly bool canDismiss;

        /// <summary>
        /// Définition d'une carte à usage unique
        /// (les cartes Lieu utilisent cette implémentation)
        /// </summary>
        /// <param name="cardLabel">Label de la carte</param>
        /// <param name="cardType">Type de la carte</param>
        /// <param name="description">Sa description</param>
        /// <param name="id">Son identifiant</param>
        /// <param name="canDismiss">Booléen pour dire si la carte peut être défaussée</param>
        /// <param name="cardEffect">Tableau d'effets de la carte</param>
        public UsableCard(string cardLabel, CardType cardType, string description, int id, bool canDismiss, params CardEffect[] cardEffect) : 
            base(cardLabel, cardType, description, id)
        {
            this.cardEffect = cardEffect;
            this.canDismiss = canDismiss;
        }
    }
}
