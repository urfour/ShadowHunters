using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Players.model
{
    public delegate void CharaterPower(Player owner);
    public delegate void CharaterAvailabilityPowerListeners(Player owner);
    public delegate void CharaterAvailabilityPower(Player owner);

    public class Power
    {
        public readonly CharaterPower power;
        public readonly CharaterAvailabilityPowerListeners addListeners;
        public readonly CharaterAvailabilityPower availability;

        public Power(CharaterPower power, CharaterAvailabilityPowerListeners addListeners, CharaterAvailabilityPower availability)
        {
            this.power = power;
            this.addListeners = addListeners;
            this.availability = availability;
        }
    }
}
