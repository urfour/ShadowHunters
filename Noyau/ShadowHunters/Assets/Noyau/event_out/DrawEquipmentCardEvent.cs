using EventSystem;
using Scripts;

namespace Scripts.event_out
{
    public class DrawEquipmentCardEvent : PlayerEvent
    {
        public int CardId { get; set; }

        public DrawEquipmentCardEvent(int id)
        {
            this.CardId = id;
        }
    }
}