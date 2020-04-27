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

    /// <summary>
    /// Définition d'un pouvoir
    /// </summary>
    public class Power
    {
        public readonly CharaterPower power;
        public readonly CharaterAvailabilityPowerListeners addListeners;
        public readonly CharaterAvailabilityPower availability;

        /// <summary>
        /// Constructeur d'une condition de victoire.
        /// </summary>
        /// <param name="power">Fonction de l'effet du pouvoir du personnage</param>
        /// <param name="addListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        /// <param name="availability">Fonction qui test quand le pouvoir est utilisable</param>
        public Power(CharaterPower power, CharaterAvailabilityPowerListeners addListeners, CharaterAvailabilityPower availability)
        {
            this.power = power;
            this.addListeners = addListeners;
            this.availability = availability;
        }
    }
}
