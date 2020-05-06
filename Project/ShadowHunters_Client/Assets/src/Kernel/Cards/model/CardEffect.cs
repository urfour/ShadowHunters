using Assets.Noyau.Cards.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Cards.model
{
    public delegate void Effect(Player target, Player owner, UsableCard card);
    public delegate bool PlayerTargetable(Player target, Player owner);

    /// <summary>
    /// Définition de l'effet d'une carte et de la cible.
    /// </summary>
    public class CardEffect
    {
        public readonly string description;
        public readonly Effect effect;
        public readonly PlayerTargetable targetableCondition;

        /// <summary>
        /// Constructeur de l'effet d'une carte et de la cible.
        /// </summary>
        /// <param name="description">Sa description</param>
        /// <param name="effect">Fonction qui implémente l'effet de la carte</param>
        /// <param name="targetableCondition">Fonction qui désigne les cibles</param>
        public CardEffect(string description, Effect effect, PlayerTargetable targetableCondition)
        {
            this.description = description;
            this.effect = effect;
            this.targetableCondition = targetableCondition;
        }
    }
}
