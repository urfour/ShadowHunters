﻿using Assets.Noyau.Cards.controller;
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

        /// <summary>
        /// Initialise toute les cartes du jeu.
        /// </summary>
        public static void Init()
        {
            GCard = new GCard();
        }

        public static void Clean()
        {
            GCard = null;
        }

        /// <summary>
        /// Fonction qui permet de piocher une carte Vision.
        /// </summary>
        /// <returns>Carte piochée</returns>
        public static Card PickVision()
        {
            if (GCard.visionDeck.Count == 0)
            {
                GCard.visionDeck.AddRange(GCard.visionDiscard);
                GCard.visionDiscard.Clear();
            }

            int r = GameManager.rand.Next(0, GCard.visionDeck.Count);
            Card c = GCard.visionDeck[r];
            GCard.visionDeck.RemoveAt(r);
            GCard.visionDiscard.Add(c);
            return c;
        }

        /// <summary>
        /// Fonction qui permet de piocher une carte Lumière.
        /// </summary>
        /// <returns>Carte piochée</returns>
        public static Card PickLight()
        {
            if (GCard.lightDeck.Count == 0)
            {
                GCard.lightDeck.AddRange(GCard.lightDiscard);
                GCard.lightDiscard.Clear();
            }

            int r = GameManager.rand.Next(0, GCard.lightDeck.Count);
            Card c = GCard.lightDeck[r];
            GCard.lightDeck.RemoveAt(r);
            if (c is UsableCard)
            {
                GCard.lightDiscard.Add(c);
            }
            return c;
        }

        /// <summary>
        /// Fonction qui permet de piocher une carte Ténèbre.
        /// </summary>
        /// <returns>Carte piochée</returns>
        public static Card PickDarkness()
        {
            if (GCard.darknessDeck.Count == 0)
            {
                GCard.darknessDeck.AddRange(GCard.darknessDiscard);
                GCard.darknessDiscard.Clear();
            }

            int r = GameManager.rand.Next(0, GCard.darknessDeck.Count);
            Card c = GCard.darknessDeck[r];
            GCard.darknessDeck.RemoveAt(r);
            if (c is UsableCard)
            {
                GCard.darknessDiscard.Add(c);
            }
            return c;
        }

        /// <summary>
        /// Fonction qui permet d'obtenir une carte par son Id.
        /// </summary>
        /// <param name="idCard">Id de la carte choisie</param>
        /// <returns>Carte piochée</returns>
        public static Card GetCard(int idCard)
        {
            foreach (Card c in GCard.cards)
                if (c.Id == idCard)
                    return c;

            return null;
        }
    }
}
