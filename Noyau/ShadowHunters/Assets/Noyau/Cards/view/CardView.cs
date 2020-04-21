using Assets.Noyau.Cards.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Cards.view
{
    public static class CardView
    {
        private static GCard gCard;
        private static Random rand;

        public static void Init()
        {
            rand = new Random();
            gCard = new GCard();
        }

        public static Card PickVision()
        {
            int r = rand.Next(0, gCard.visionDeck.Count);
            Card c = gCard.visionDeck[r];
            gCard.visionDeck.RemoveAt(r);
            return c;
        }
    }
}
