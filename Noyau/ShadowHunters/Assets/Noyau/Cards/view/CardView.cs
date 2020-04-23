using Assets.Noyau.Cards.controller;
using Assets.Noyau.Cards.model;
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
        private static Random rand;

        public static void Init()
        {
            rand = new Random();
            GCard = new GCard();
        }

        public static Card PickVision()
        {
            int r = rand.Next(0, GCard.visionDeck.Count);
            Card c = GCard.visionDeck[r];
            GCard.visionDeck.RemoveAt(r);
            return c;
        }

        public static Card PickLight()
        {
            int r = rand.Next(0, GCard.lightDeck.Count);
            Card c = GCard.lightDeck[r];
            GCard.lightDeck.RemoveAt(r);
            return c;
        }

        public static Card PickDarkness()
        {
            int r = rand.Next(0, GCard.darknessDeck.Count);
            Card c = GCard.darknessDeck[r];
            GCard.darknessDeck.RemoveAt(r);
            return c;
        }
    }
}
