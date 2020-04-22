using Assets.Noyau.Cards.model;
using Assets.Noyau.Cards.view;
using Assets.Noyau.Manager.view;
using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scripts.event_out;

namespace Assets.Noyau.Cards.controller
{
    public class GCard
    {
        public List<Card> cards = new List<Card>();

        public List<VisionCard> visionDeck;

        public List<Card> darknessDeck = new List<Card>();
        public List<Card> lightnessDeck = new List<Card>();

        public UsableCard Antre;
        public UsableCard Monastere;
        public UsableCard Cimetiere;

        public GCard()
        {
            Antre = CreateUsableCard("card.location.antre", CardType.Location, "card.location.antre.description", true,
                new CardEffect("card.location.antre.effect.pickvision",
                effect: (target) => 
                {
                    VisionCard c = CardView.PickVision();
                    EventView.Manager.Emit(new SelectVisionTargetEvent() { PlayerId=GameManager.PlayerTurn.Value.Id , cardId=c.Id });
                },
                targetableCondition:null));

            Monastere = CreateUsableCard("")

            visionDeck = new List<VisionCard>()
            {
                CreateVision("card.vision.vision_destructrice", "card.vision.vision_destructrice.description", 
                condition: (player) =>
                {
                    return player.Character.characterHP >= 12;
                },
                effect: (player) =>
                {
                    player.Wounded(1);
                }),
            };
        }

        public VisionCard CreateVision(string cardLabel, string description, VisionCardCondition condition, VisionCardEffect effect)
        {
            int id = cards.Count;
            VisionCard c = new VisionCard(cardLabel, CardType.Vision, description, id, condition, effect);
            cards.Add(c);
            visionDeck.Add(c);
            return c;
        }

        public UsableCard CreateUsableCard(string cardLabel, CardType cardType, string description, bool canDismiss, params CardEffect[] cardEffect)
        {
            int id = cards.Count;
            UsableCard c = new UsableCard(cardLabel, cardType, description, id, canDismiss, cardEffect);
            cards.Add(c);
            return c;
        }
    }
}
