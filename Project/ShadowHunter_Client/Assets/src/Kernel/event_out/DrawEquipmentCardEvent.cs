using EventSystem;
using Scripts;

namespace Scripts.event_out
{
    /// <summary>
    /// Event qui transmet l'info qu'une carte équipement à été piochée
    /// </summary>
    public class DrawEquipmentCardEvent : PlayerEvent
    {
        public int CardId { get; set; }

        public DrawEquipmentCardEvent(int id)
        {
            this.CardId = id;
        }

        public DrawEquipmentCardEvent()
        {

        }
    }
}