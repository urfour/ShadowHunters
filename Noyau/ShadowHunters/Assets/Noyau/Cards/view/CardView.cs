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

        public static VisionCard PickVision()
        {
            int r = rand.Next(0, GCard.visionDeck.Count);
            VisionCard c = GCard.visionDeck[r];
            GCard.visionDeck.RemoveAt(r);
            return c;
        }

        public static LightCard PickLight()
        {
            int r = rand.Next(0, GCard.lightDeck.Count);
            LightCard c = GCard.lightDeck[r];
            GCard.lightDeck.RemoveAt(r);
            return c;
        }

        public static DarknessCard PickDarkness()
        {
            int r = rand.Next(0, GCard.darknessDeck.Count);
            DarknessCard c = GCard.darknessDeck[r];
            GCard.darknessDeck.RemoveAt(r);
            return c;
        }
    }
}
