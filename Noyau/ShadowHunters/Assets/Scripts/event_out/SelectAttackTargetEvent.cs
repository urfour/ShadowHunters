﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    // Evenement qui permet de choisir un joueur à attaquer parmis une liste de joueur
    public class SelectAttackTargetEvent : PlayerEvent
    {
        public int[] PossibleTargetId { get; set; }
        public bool PowerFranklin { get; set; }
        public bool PowerGeorges { get; set; }
    }
}
