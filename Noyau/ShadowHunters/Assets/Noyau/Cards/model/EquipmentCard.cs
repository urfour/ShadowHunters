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

    public class EquipmentCard : Card
    {
        public readonly Equipe equipe;
        public readonly Unequipe unequipe;
        public readonly EquipmentCondition condition;
        public readonly EquipmentEffect effect;

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
