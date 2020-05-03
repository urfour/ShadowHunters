using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    /// <summary>
    /// Retour de l'evenement SelectAttackTargetEvent
    /// Renvoie l'id du joueur attaquant, l'id du joueur attaqué et 2 booléens pour savoir si l'event vient du pouvoir d'un personnage
    /// </summary>
    public class AttackPlayerEvent : PlayerEvent
    {
        public int PlayerAttackedId { get; set; }
        public bool PowerFranklin { get; set; }
        public bool PowerGeorges { get; set; }
        public bool PowerLoup { get; set; }
        public bool PowerCharles { get; set; }

        public AttackPlayerEvent(int playerId, int playerAttackedId, bool powerF, bool powerG, bool powerL, bool powerC) : base()
        {
            this.PlayerId = playerId;
            this.PlayerAttackedId = playerAttackedId;
            this.PowerFranklin = powerF;
            this.PowerGeorges = powerG;
            this.PowerLoup = powerL;
            this.PowerCharles = powerC;
        }

        public AttackPlayerEvent() : base()
        {

        }
    }
}
