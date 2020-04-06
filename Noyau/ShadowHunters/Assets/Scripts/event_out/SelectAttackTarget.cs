﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventExemple.Kernel.Players.event_out
{
    // Evenement qui permet de choisir un joueur parmis une liste de joueur
    public class SelectAttackTarget : PlayerEvent
    {
        public int AttackerId { get; set; }
        public int[] PossibleTargetId { get; set; } 
    }
}
