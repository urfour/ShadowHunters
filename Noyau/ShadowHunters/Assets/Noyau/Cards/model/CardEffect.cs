using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Cards.model
{
    public delegate void Effect(Player target);
    public delegate bool PlayerTargetable(Player target);
    public class CardEffect
    {
        public readonly string description;
        public readonly Effect effect;
        public readonly PlayerTargetable targetableCondition;

        public CardEffect(string description, Effect effect, PlayerTargetable targetableCondition)
        {
            this.description = description;
            this.effect = effect;
            this.targetableCondition = targetableCondition;
        }
    }
}
