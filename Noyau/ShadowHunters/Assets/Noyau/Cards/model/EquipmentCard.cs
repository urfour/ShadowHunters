using Assets.Noyau.Cards.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Cards.model
{

    public delegate void Equipe(Player player, EquipmentCard card);
    public delegate void Unequipe(Player player, EquipmentCard card);
    public delegate bool EquipmentCondition(Player player);
    public delegate void EquipmentEffect(Player player, Card card);

    /// <summary>
    /// Définition d'une carte équipement
    /// </summary>

    public class EquipmentCard : Card
    {
        public readonly Equipe equipe;
        public readonly Unequipe unequipe;
        public readonly EquipmentCondition condition;
        public readonly EquipmentEffect effect;

        /// <summary>
        /// Constructeur d'une carte équipement.
        /// </summary>
        /// <param name="cardLabel">Label de la carte</param>
        /// <param name="cardType">Type de la carte</param>
        /// <param name="description">Sa description</param>
        /// <param name="id">Son identifiant</param>
        /// <param name="equipe">Fonction qui équipe le joueur de la carte</param>
        /// <param name="unequipe">Fonction qui enlève l'quipement au joueur</param>
        /// <param name="condition">Fonction qui désigne les cibles</param>
        /// <param name="effect">Fonction qui implémente l'effet de la carte</param>
        public EquipmentCard(string cardLabel, CardType cardType, string description, int id, Equipe equipe, Unequipe unequipe, EquipmentCondition condition, EquipmentEffect effect) :
            base(cardLabel, cardType, description, id)
        {
            this.equipe = equipe;
            this.unequipe = unequipe;
            this.condition = condition;
            this.effect = effect;
        }
    }
}
