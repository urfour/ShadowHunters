using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_out
{
    /// <summary>
    /// Event qui permet de choisir un joueur à attaquer parmi une liste de joueur
    /// </summary>
    public class SelectAttackTargetEvent : PlayerEvent
    {
        public int[] PossibleTargetId { get; set; } = null;
        // Dans certains cas on connait directement le joueur qu'on attaque
        public int TargetID { get; set; } = -1;
        public bool PowerFranklin { get; set; } = false;
        public bool PowerGeorges { get; set; } = false;
        public bool PowerLoup { get; set; } = false;
        public bool PowerCharles { get; set; } = false;

        public SelectAttackTargetEvent(int playerId, int[] possibleTargetId, int targetID, bool powerFranklin, bool powerGeorges,
            bool powerLoup, bool powerCharles) : base()
        {
            this.PlayerId = playerId;
            this.PossibleTargetId = possibleTargetId;
            this.TargetID = targetID;
            this.PowerFranklin = powerFranklin;
            this.PowerGeorges = powerGeorges;
            this.PowerLoup = powerLoup;
            this.PowerCharles = powerCharles;
        }

        public SelectAttackTargetEvent() : base()
        {

        }
    }
}
