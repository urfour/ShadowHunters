using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Players.model
{
    public delegate void CheckWinningCondition(Player owner);
    public delegate void SetWinningListeners(Player owner);

    public class Goal
    {
        public readonly CheckWinningCondition checkWinning;
        public readonly SetWinningListeners setWinningListeners;

        public Goal(CheckWinningCondition checkWinning, SetWinningListeners setWinningListeners)
        {
            this.checkWinning = checkWinning;
            this.setWinningListeners = setWinningListeners;
        }
    }
}
