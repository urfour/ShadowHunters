using Assets.Noyau.Cards.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Cards.model
{
    public delegate bool DarknessCardCondition(Player player);
    public delegate void DarknessCardEffect(Player player, LightCard card);

    public class DarknessCard : Card
    {
        public readonly DarknessCardCondition condition;
        public readonly DarknessCardEffect effect;

        public DarknessCard(string cardLabel, CardType cardType, string description, int id, DarknessCardCondition condition, DarknessCardEffect effect) :
            base(cardLabel, cardType, description, id)
        {
            this.condition = condition;
            this.effect = effect;
        }
    }
}

