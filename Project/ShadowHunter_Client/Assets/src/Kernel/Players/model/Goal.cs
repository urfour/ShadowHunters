using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Players.model
{
    /// <summary>
    /// Définition d'une condition de victoire
    /// </summary>
    public delegate void CheckWinningCondition(Player owner);
    public delegate void SetWinningListeners(Player owner);

    public class Goal
    {
        public readonly CheckWinningCondition checkWinning;
        public readonly SetWinningListeners setWinningListeners;

        /// <summary>
        /// Constructeur d'une condition de victoire.
        /// </summary>
        /// <param name="checkWinning">Fonction qui test la condition de victoire</param>
        /// <param name="setWinningListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        public Goal(CheckWinningCondition checkWinning, SetWinningListeners setWinningListeners)
        {
            this.checkWinning = checkWinning;
            this.setWinningListeners = setWinningListeners;
        }
    }
}
