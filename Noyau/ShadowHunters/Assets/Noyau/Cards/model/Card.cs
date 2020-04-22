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
    /// 
    public class Card
    {
        public readonly string cardLabel;
        public readonly CardType cardType;
        public readonly string description;
        public readonly int Id;

        public Card(string cardLabel, CardType cardType, string description, int id)
        {
            this.cardLabel = cardLabel;
            this.cardType = cardType;
            this.description = description;
            Id = id;
        }
    }
}
