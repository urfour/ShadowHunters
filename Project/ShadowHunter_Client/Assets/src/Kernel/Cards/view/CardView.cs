using Assets.Noyau.Cards.controller;
using Assets.Noyau.Cards.model;
using Assets.Noyau.Manager.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Cards.view
{
    public static class CardView
    {
        public static GCard GCard;
        //private static Random rand;

        /// <summary>
        /// Initialise toute les cartes du jeu.
        /// </summary>
        public static void Init()
        {
            //rand = GameManager.rand;
            GCard = new GCard();
        }

        public static void Clean()
        {
            GCard = null;
        }

        /// <summary>
        /// Fonction qui permet de piocher une carte Vision.
        /// </summary>
        public static Card PickVision()
        {
            int r = GameManager.rand.Next(0, GCard.visionDeck.Count);
            Card c = GCard.visionDeck[r];
            GCard.visionDeck.RemoveAt(r);
            return c;
        }

        /// <summary>
        /// Fonction qui permet de piocher une carte Lumière.
        /// </summary>
        public static Card PickLight()
        {
            int r = GameManager.rand.Next(0, GCard.lightDeck.Count);
            Card c = GCard.lightDeck[r];
            GCard.lightDeck.RemoveAt(r);
            return c;
        }

        /// <summary>
        /// Fonction qui permet de piocher une carte Ténèbre.
        /// </summary>
        public static Card PickDarkness()
        {
            int r = GameManager.rand.Next(0, GCard.darknessDeck.Count);
            Card c = GCard.darknessDeck[r];
            GCard.darknessDeck.RemoveAt(r);
            return c;
        }
        public static Card GetCard(int idCard)
        {
            foreach (Card c in GCard.cards)
                if (c.Id == idCard)
                    return c;

            return null;
        }

    }
}
