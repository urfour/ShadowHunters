using Assets.Noyau.Cards.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    /// <summary>
    /// Event qui permet de choisir un joueur à voler parmi une liste de joueurs
    /// </summary>
    public class SelectStealCardEvent : PlayerEvent
    {
        public (Card equipment, Player owner)[] PossiblePlayerTargetId { get; set; } 
    }
}
