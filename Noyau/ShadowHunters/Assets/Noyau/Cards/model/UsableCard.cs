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

        public UsableCard(string cardLabel, CardType cardType, string description, int id, bool canDismiss, params CardEffect[] cardEffect) : 
            base(cardLabel, cardType, description, id)
        {
            this.cardEffect = cardEffect;
            this.canDismiss = canDismiss;
        }
    }
}
