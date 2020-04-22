using Assets.Noyau.Cards.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Cards.model
{
    public delegate bool LightCardCondition(Player player);
    public delegate void LightCardEffect(Player player, LightCard card);

    public class LightCard : Card
    {
        public readonly LightCardCondition condition;
        public readonly LightCardEffect effect;

        public LightCard(LightCardCondition condition, LightCardEffect effect)
        {
            this.condition = condition;
            this.effect = effect;
        }
    }
}

