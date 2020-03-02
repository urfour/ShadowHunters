using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.MainMenuUI.SearchGame
{
    [Serializable]
    class RoomData
    {
        public RoomData(int code, string name, int maxNbPlayers, int currentNbPlayers, bool isSuppressed, bool hasPassword)
        {
            Code = code;
            Name = name;
            MaxNbPlayer = maxNbPlayers;
            CurrentNbPlayer = currentNbPlayers;
            IsSuppressed = isSuppressed;
            HasPassword = hasPassword;
        }

        public int Code { get; set; }
        public string Name { get; set; }
        public int MaxNbPlayer { get; set; }
        public int CurrentNbPlayer { get; set; }
        public bool IsSuppressed { get; set; }
        public bool HasPassword { get; set; }
    }
}
