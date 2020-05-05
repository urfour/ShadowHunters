using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Players.model
{
    public delegate void CharacterPower(Player owner);
    public delegate void CharacterAvailabilityPowerListeners(Player owner);
    public delegate void CharacterAvailabilityPower(Player owner);

    /// <summary>
    /// Définition d'un pouvoir
    /// </summary>
    public class Power
    {
        public readonly CharacterPower power;
        public readonly CharacterAvailabilityPowerListeners addListeners;
        public readonly CharacterAvailabilityPower availability;

        /// <summary>
        /// Constructeur d'une condition de victoire.
        /// </summary>
        /// <param name="power">Fonction de l'effet du pouvoir du personnage</param>
        /// <param name="addListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        /// <param name="availability">Fonction qui test quand le pouvoir est utilisable</param>
        public Power(CharacterPower power, CharacterAvailabilityPowerListeners addListeners, CharacterAvailabilityPower availability)
        {
            this.power = power;
            this.addListeners = addListeners;
            this.availability = availability;
        }
    }
}
