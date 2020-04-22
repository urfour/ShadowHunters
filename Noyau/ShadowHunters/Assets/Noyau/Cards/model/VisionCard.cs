using Assets.Noyau.Cards.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Cards.model
{
    public struct VisionEffect
    {
        /*
        // Types de cartes vision
        public bool effectOnShadow; // "Je pense que tu es Shadow"
        public bool effectOnHunter; // "Je pense que tu es Hunter"
        public bool effectOnNeutral; // "Je pense que tu es Neutre"
        public bool effectOnLowHP; // "Je pense que tu es un personnage de 11 Points de vie ou moins"
        public bool effectOnHighHP; // "Je pense que tu es un personnage de 12 Points de vie ou plus"

        // Effet des cartes
        public bool effectSupremeVision; // Carte Vision Suprême
        public bool effectGivingEquipementCard; // Donner une carte équipement ou subir 1 Blessure
        public bool effectHealingOneWound; // Soigner 1 Blessure
        public bool effectTakeWounds; // Subir x Blessures
        public int nbWounds; // Nombre de Blessures subies
        */
    }

    public delegate bool VisionCardCondition(Player player);
    public delegate void VisionCardEffect(Player player);

    public class VisionCard : Card
    {
        public readonly VisionCardCondition condition;
        public readonly VisionCardEffect effect;

        public VisionEffect visionEffect;

        public VisionCard(string cardLabel, CardType cardType, string description, int id, VisionCardCondition condition, VisionCardEffect effect) : 
            base(cardLabel, cardType, description, id)
        {
            this.condition = condition;
            this.effect = effect;
        }
    }
}

