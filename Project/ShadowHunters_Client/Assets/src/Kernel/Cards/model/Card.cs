using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Noyau.Cards.controller
{
    public enum CardType { Location, Vision, Light, Darkness }

    /// <summary>
    /// Définition d'une carte
    /// (les cartes Lieu utilisent cette implémentation)
    /// </summary>
     
    public class Card
    {
        public readonly string cardLabel;
        public readonly CardType cardType;
        public readonly string description;
        public readonly int Id;

        /// <summary>
        /// Constructeur d'une carte.
        /// </summary>
        /// <param name="cardLabel">Label de la carte</param>
        /// <param name="cardType">Type de la carte</param>
        /// <param name="description">Sa description</param>
        /// <param name="id">Son identifiant</param>
        public Card(string cardLabel, CardType cardType, string description, int id)
        {
            this.cardLabel = cardLabel;
            this.cardType = cardType;
            this.description = description;
            Id = id;
        }
    }
}
