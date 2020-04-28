using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.event_in
{
    /// <summary>
    /// Event d'utilisation d'une carte à usage unique
    /// </summary>
    public class UsableCardUseEvent : PlayerEvent
    {
        public int Cardid { get; set; }
        public int EffectSelected { get; set; }
        public int PlayerSelected { get; set; }

        public UsableCardUseEvent(int card_id, int effect_selected, int player_selected)
        {
            this.Cardid = card_id;
            this.EffectSelected = effect_selected;
            this.PlayerSelected = player_selected;
        }

        public UsableCardUseEvent()
        {

        }
    }
}
